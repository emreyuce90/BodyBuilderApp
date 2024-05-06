using BodyBuilderApp.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Interfaces {
    public interface ISubProgrammeService {
        Task<Response> GetAllAsync();
        Task<Response> GetByIdAsync(Guid programmeId);
    }
}
