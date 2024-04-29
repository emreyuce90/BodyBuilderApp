using BodyBuilder.Application.Dtos.Bodypart;
using BodyBuilderApp.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Interfaces {
    public interface IBodyPartService {
        Task<Response> GetAllAsync();
        Task<Response> GetByIdAsync(Guid Id);
        Task<Response> AddAsync(BodyPartAddDto bodyPartAddDto);
        Task<Response> UpdateAsync(BodyPartUpdateDto bodyPartUpdateDto);
    }
}
