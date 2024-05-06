using AutoMapper;
using BodyBuilder.Application.Dtos.SubProgramme;
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
    public class SubProgrammeService : ISubProgrammeService {

        private readonly ISubProgrammeRepository _subProgrammeRepository;
        private readonly IMapper _mapper;

        public SubProgrammeService(ISubProgrammeRepository subProgrammeRepository, IMapper mapper) {
            _subProgrammeRepository = subProgrammeRepository;
            _mapper = mapper;
        }

        public async Task<Response> GetAllAsync() {
            throw new NotImplementedException();
        }

        public async Task<Response> GetByIdAsync(Guid programmeId) {
            try {
               var subProgrammes = await _subProgrammeRepository.GetAllAsync(sp => sp.ProgrammeId == programmeId && sp.IsActive == true).OrderBy(o=>o.Name).ToListAsync();
                return new Response(_mapper.Map<List<SubProgrammeDto>>(subProgrammes));
            } catch (Exception ex) {
                return new Response(ex.Message);
                throw;
            }
        }
    }
}
