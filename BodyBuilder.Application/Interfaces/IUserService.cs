using BodyBuilder.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Interfaces {
    public interface IUserService {
        Task<List<UserDto>> GetAllAsync(bool isTracking = true);
        Task<UserDto> GetById(Guid id);
        Task<UserDto> AddAsync(UserDto userDto);
        Task<UserDto> UpdateAsync(UserDto userDto);
        Task<bool> DeleteAsync(Guid id);

    }
}
