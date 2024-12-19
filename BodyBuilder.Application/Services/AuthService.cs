using AutoMapper;
using BodyBuilder.Application.Dtos.User;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Application.Utilities.JWT;
using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
using BodyBuilder.Domain.Utilities;
using BodyBuilder.Infrastructure.Infrastructure.RabbitMq;
using BodyBuilderApp.Communication;
using BodyBuilderApp.Resources;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Services {
    public class AuthService : IAuthService {

        private readonly IUserRepository _userRepository;
        private readonly ITokenCreate _tokenCreate;
        private readonly IUserRefreshToken _userRefreshToken;
        private readonly IMapper _mapper;
        private readonly IValidator<UserLoginDto> _validator;
        private readonly IValidator<UserAddDto> _userRegisterValidator;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly IMessageBus _messageBus;
        private readonly RabbitMqSettings _rabbitMqSettings;
        public AuthService(IUserRepository userRepository, ITokenCreate tokenCreate, IMapper mapper, IUserRefreshToken userRefreshToken, IValidator<UserLoginDto> validator, IValidator<UserAddDto> userRegisterValidator, IRoleRepository roleRepository, ILogger<AuthService> logger, IMessageBus messageBus, RabbitMqSettings rabbitMqSettings) {
            _userRepository = userRepository;
            _tokenCreate = tokenCreate;
            _mapper = mapper;
            _userRefreshToken = userRefreshToken;
            _validator = validator;
            _userRegisterValidator = userRegisterValidator;
            _roleRepository = roleRepository;
            _logger = logger;
            _messageBus = messageBus;
            _rabbitMqSettings = rabbitMqSettings;
        }

        public async Task<Response> CleanRefreshToken(string refreshToken) {
            var existRefreshToken = await _userRefreshToken.Table.FirstOrDefaultAsync(rt => rt.Code == refreshToken);
            if (existRefreshToken == null) return new Response("Refresh token bulunamadı");
            await _userRefreshToken.DeleteAsync(existRefreshToken.Id);
            await _userRefreshToken.SaveAsync();
            return new Response();
        }

        public async Task<Response> CreateToken(UserLoginDto userLoginDto) {
            //check model validation
            var validationResult = await _validator.ValidateAsync(userLoginDto);
            if (!validationResult.IsValid) {
                var message = string.Empty;
                foreach (var item in validationResult.Errors) {
                    message += item.ErrorMessage;
                }
                return new Response() { Message = message, Success = false };
            }
            //check user existence
            var user = await _userRepository.Table.Include(r => r.Role).FirstOrDefaultAsync(u => u.Email == userLoginDto.Email);
            if (user == null) return new Response("Böyle bir kullanıcı bulunamadı");
            //Eğer user var ise ve silinmemiş ise
            if (user != null && !user.IsDeleted) {
                //access token üret
                var accessToken = _tokenCreate.CreateToken(user);
                //accessToken üretildiğinde refresh token da üretilmiş olur
                //kullanıcının refresh token ı yok ise bu üreilen refresh token ı refresh token tablosuna yaz
                //eğer kullanıcının refresh tokenı var ise tablodaki refresh tokenı güncelle
                var refreshToken = await _userRefreshToken.Table.FirstOrDefaultAsync(r => r.UserId == user.Id);

                if (refreshToken == null) {
                    await _userRefreshToken.CreateAsync(new UserRefreshToken { UserId = user.Id, Code = accessToken.RefreshToken, Expiration = accessToken.RefreshTokenExpiration });
                } else {
                    refreshToken.Code = accessToken.RefreshToken;
                    refreshToken.Expiration = accessToken.RefreshTokenExpiration;
                    _userRefreshToken.UpdateAsync(refreshToken);
                }
                await _userRefreshToken.SaveAsync();

                var userResource = new UserResource {
                    Email = user.Email,
                    Id = user.Id,
                    RoleName = user.Role.RoleName,
                    Token = accessToken.Token,
                    RefreshToken= refreshToken?.Code,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                };
                return new Response {
                    Success = true,
                    Code = 200,
                    Resource = userResource
                };
            }

            return new Response("Kullanıcı silinmiş veya aktif değil");
        }

        public async Task<Response> CreateTokenByRefreshToken(string refreshToken) {
            var existRefreshToken = await _userRefreshToken.GetSingle(rt => rt.Code == refreshToken);
            if (existRefreshToken?.Expiration.Date <= DateTime.Now.Date) return new Response("Refresh token süresi bitmiş");
            if (existRefreshToken == null) return new Response("Refresh token bulunamadı");

            var user = await _userRepository.Table.Include(u => u.Role).FirstOrDefaultAsync(x => x.Id == existRefreshToken.UserId);
            if (user == null) return new Response("Kullanıcı  bulunamadı");

            var accessToken = _tokenCreate.CreateToken(user);
            existRefreshToken.Code = accessToken.RefreshToken;
            existRefreshToken.Expiration = accessToken.RefreshTokenExpiration;
            await _userRefreshToken.SaveAsync();

            var userResource = new UserResource {
                Email = user.Email,
                Token = accessToken.Token,
                Id = user.Id,
                RoleName = user.Role.RoleName,
                RefreshToken = accessToken.RefreshToken,
                
            };
            return new Response<UserResource>(userResource);
        }

        public async Task<Response> RegisterUser(UserAddDto userAddDto) {
            //check model validation
            var validationResult = await _userRegisterValidator.ValidateAsync(userAddDto);
            if (!validationResult.IsValid) {
                var message = string.Empty;
                foreach (var item in validationResult.Errors) {
                    message += item.ErrorMessage;
                }
                return new Response(message);
            }

            //check if user already exist
            var dbuser = await _userRepository.Table.FirstOrDefaultAsync(u => u.Email == userAddDto.Email);
            if (dbuser != null) return new Response("Kayıt olmaya çalıştığınız kullanıcı zaten mevcut, aynı kullanıcıyla en fazla bir hesap açabilirsiniz");

            //Password salt
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userAddDto.Password, out passwordHash, out passwordSalt);
            //user informations

            //TODO: Get all roles and select one
            var rolesInDb = (await _roleRepository.GetAllAsync(r => r.IsActive && !r.IsDeleted).ToListAsync())!;
            User user = new() {
                FirstName = userAddDto.FirstName,
                LastName = userAddDto.LastName,
                RegistrationDate = userAddDto.RegistrationDate,
                EndDate = userAddDto.EndDate,
                CreatedDate = DateTime.Now,
                PhoneNumber = userAddDto.PhoneNumber,
                DateOfBirth = userAddDto.DateOfBirth,
                Email = userAddDto.Email,
                IsActive = true,
                MailConfirm = false,
                MailConfirmValue = Guid.NewGuid().ToString(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RoleId = rolesInDb.First(r => r.RoleName == "User").Id
            };

            await _userRepository.CreateAsync(user);
            await _userRepository.SaveAsync();
            _logger.LogInformation("Registering user process implemented. The user registered successfully with this address {@email}", user.Email); //Parametreyi bu şekilde verdiğimizde elasticsearch bu userId yi indexler
           await _messageBus.PublishAsync(user.Email, _rabbitMqSettings.QueueName);
            return new Response<UserDto>(_mapper.Map<UserDto>(user));

        }

        public async Task<Response> VerifyUser(UserLoginDto userLoginDto) {
            //check model validation
            var validationResult = await _validator.ValidateAsync(userLoginDto);
            if (!validationResult.IsValid) {
                var message = string.Empty;
                foreach (var item in validationResult.Errors) {
                    message += item.ErrorMessage;
                }
                return new Response() { Message = message, Success = false };
            }
            var user = await _userRepository.GetSingle(u => u.Email == userLoginDto.Email);
            if (user == null) {
                return new Response("Kullanıcı adı veya şifre hatalıdır");
            }
            bool isTrue = HashingHelper.VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt);
            return isTrue ? new Response() : new Response("Kullanıcı adı veya şifre hatalıdır");


        }

    }



}

