using AutoMapper;
using BodyBuilder.Application.Dtos.Programme;
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

    public class ProgrammeService : IProgrammeService {

        private readonly IProgrammeRepository _programmeRepository;
        private readonly IMapper _mapper;

        public ProgrammeService(IProgrammeRepository programmeRepository, IMapper mapper) {
            _programmeRepository = programmeRepository;
            _mapper = mapper;
        }

        public Task<Response> AddAsync(ProgrammeAddDto programmeAddDto) {
            throw new NotImplementedException();
        }

        public async Task<Response> GetAllAsync() {
            try {

            var programmes = await _programmeRepository.GetAllAsync().ToListAsync();
                return new Response(_mapper.Map<List<ProgrammeDto>>(programmes));
            } catch (Exception ex) {
                return new Response(ex.ToString());
                throw;
            }
        }

        public Task<Response> GetByIdAsync(Guid programmeId) {
            throw new NotImplementedException();
        }

        public async Task<Response> GetByUserIdAsync(Guid userId) {
            try {
                var userProgrammes = await _programmeRepository.GetAllAsync(p => p.IsActive == true && p.IsDeleted == false && p.UserId == userId).ToListAsync();
                return new Response(_mapper.Map<List<ProgrammeDto>>(userProgrammes));
            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }
    }
}
