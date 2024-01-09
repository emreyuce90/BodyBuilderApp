using BodyBuilder.Application.Dtos.User;
using BodyBuilderApp.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Interfaces
{
    public interface IUserService {
        Task<Response<List<UserDto>>> GetAllAsync(bool isTracking = true);
        Task<Response<UserDto>> GetById(Guid id);
        Task<Response<UserDto>> UpdateAsync(UserDto userDto);
        Task<bool> DeleteAsync(Guid id);
        Task<UserDto> GetUserByEMail(string email);

    }
}
