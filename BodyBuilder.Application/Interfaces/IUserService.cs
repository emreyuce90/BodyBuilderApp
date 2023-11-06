using BodyBuilder.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Interfaces {
    public interface IUserService {
        List<UserDto> GetAllAsync(bool isTracking = true);
        UserDto GetById(Guid id);
        Task<UserDto> AddAsync(UserDto userDto);
        Task<UserDto> UpdateAsync(UserDto userDto);
        Task<UserDto> DeleteAsync(Guid id);
        Task<UserDto> DeleteAsync(UserDto userDto);
    }
}
