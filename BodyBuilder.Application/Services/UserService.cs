using AutoMapper;
using BodyBuilder.Application.Dtos.User;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Domain.Interfaces;
using BodyBuilderApp.Communication;
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

        public Task<bool> DeleteAsync(Guid id) {
            throw new NotImplementedException();
        }

        public async Task<Response<List<UserDto>>> GetAllAsync(bool isTracking = true) {
            var users = await _userRepository.GetAllAsync(false).ToListAsync();
            return new Response<List<UserDto>>(_mapper.Map<List<UserDto>>(users));
        }

        public Task<Response<UserDto>> GetById(Guid id) {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetUserByEMail(string email) {
            throw new NotImplementedException();
        }

        public Task<Response<UserDto>> UpdateAsync(UserDto userDto) {
            throw new NotImplementedException();
        }
    }
}
