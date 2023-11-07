using AutoMapper;
using BodyBuilder.Application.Dtos;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
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

        public async Task<UserDto> AddAsync(UserDto userDto) {
           var user= await _userRepository.CreateAsync(_mapper.Map<User>(userDto));
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

        public async Task<UserDto> UpdateAsync(UserDto userDto) {
            var user = _userRepository.UpdateAsync(_mapper.Map<User>(userDto));
            await _userRepository.SaveAsync();
            return _mapper.Map<UserDto>(user) ;
        }
    }
}
