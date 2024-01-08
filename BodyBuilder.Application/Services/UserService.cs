using AutoMapper;
using BodyBuilder.Application.Dtos.User;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
using BodyBuilder.Domain.Utilities;
using BodyBuilderApp.Communication;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Services {
    public class UserService : IUserService {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper) {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Response<UserDto>> AddAsync(UserAddDto userAddDto) {

            //Password salt
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userAddDto.Password, out passwordHash, out passwordSalt);
            //user informations
            User user = new() {
                CreatedDate = DateTime.Now,
                PhoneNumber = userAddDto.PhoneNumber,
                Gender = userAddDto.Gender,
                DateOfBirth = userAddDto.DateOfBirth,
                Email = userAddDto.Email,
                IsActive = true,
                MailConfirm = false,
                MailConfirmValue = Guid.NewGuid().ToString(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = new Role() { RoleName = "Admin" }
            };

            await _userRepository.CreateAsync(user);
            await _userRepository.SaveAsync();
            return new Response<UserDto>(_mapper.Map<UserDto>(user));
        }

        public async Task<bool> DeleteAsync(Guid id) {
            bool deleted = await _userRepository.DeleteAsync(id);
            await _userRepository.DeleteAsync(id);
            return deleted;
        }

        public async Task<Response<List<UserDto>>> GetAllAsync(bool isTracking = true) {
            var users = await _userRepository.GetAllAsync(isTracking).ToListAsync();
            return new Response<List<UserDto>>(_mapper.Map<List<UserDto>>(users));
        }

        public async Task<Response<UserDto>> GetById(Guid id) {
            var user = await _userRepository.GetById(id);
            return new Response<UserDto>(_mapper.Map<UserDto>(user));
        }

        public async Task<UserDto> GetUserByEMail(string email) {
            var user = await _userRepository.Table.Include(r => r.Role).FirstOrDefaultAsync(u => u.Email == email);
            if (user != null) {
                var mappedValue = _mapper.Map<UserDto>(user);
                mappedValue.RoleName = user.Role.RoleName;
                return mappedValue;
            }
            return null;

        }

        public async Task<Response<UserDto>> UpdateAsync(UserDto userDto) {
            var user = _userRepository.UpdateAsync(_mapper.Map<User>(userDto));
            await _userRepository.SaveAsync();
            return new Response<UserDto>(_mapper.Map<UserDto>(user));
        }
    }
}
