using BodyBuilder.Application.Dtos.Movement;
using BodyBuilderApp.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Interfaces
{
    public interface IMovementService
    {
        Task<Response> GetAllAsync();
        Task<Response> GetByIdAsync(Guid Id);
        Task<Response> AddAsync(MovementAddDto entity);
        Task<Response> UpdateAsync(MovementUpdateDto movementUpdateDto);
        Task<Response> GetMovementByBodypartId(Guid bodypartId);
    }
}
