using AutoMapper;
using BodyBuilder.Application.Dtos.User;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
using BodyBuilder.Domain.Utilities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Services
{
    public class UserService : IUserService {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper) {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> AddAsync(UserAddDto userAddDto) {

            //Password salt
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userAddDto.Password, out passwordHash, out passwordSalt);
            //user informations
            User user = new() {
                CreatedDate = DateTime.Now,
                PhoneNumber= userAddDto.PhoneNumber,
                Gender=userAddDto.Gender,
                DateOfBirth=userAddDto.DateOfBirth,
                Email = userAddDto.Email,
                IsActive = true,
                MailConfirm = false,
                MailConfirmValue = Guid.NewGuid().ToString(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _userRepository.CreateAsync(user);
            await _userRepository.SaveAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> DeleteAsync(Guid id) {
            bool deleted = await _userRepository.DeleteAsync(id);
            await _userRepository.DeleteAsync(id);
            return deleted;
        }

        public  async Task<List<UserDto>> GetAllAsync(bool isTracking = true) {
            var users =await  _userRepository.GetAllAsync(isTracking).ToListAsync();
            return  _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetById(Guid id) {
            var user = await _userRepository.GetById(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByEMail(string email) {
           var user = await _userRepository.GetSingle(u=>u.Email == email);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateAsync(UserDto userDto) {
            var user = _userRepository.UpdateAsync(_mapper.Map<User>(userDto));
            await _userRepository.SaveAsync();
            return _mapper.Map<UserDto>(user) ;
        }
    }
}
