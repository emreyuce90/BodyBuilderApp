using AutoMapper;
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
    public class SubProgrammeMovementService : ISubProgrammeMovementService {

        private readonly ISubProgrammeMovementRepository _subProgrammeMovementRepository;
        private readonly IMapper _mapper;

        public SubProgrammeMovementService(ISubProgrammeMovementRepository subProgrammeMovementRepository, IMapper mapper) {
            _subProgrammeMovementRepository = subProgrammeMovementRepository;
            _mapper = mapper;
        }

        public async Task<Response> GetAllBySubProgrammeIdAsync(Guid subProgrammeId) {
            try {
               var spm= await _subProgrammeMovementRepository.GetAllAsync(sp => sp.IsActive == true && sp.IsDeleted == false && sp.SubProgrammeId == subProgrammeId).Include(x=>x.Movement).ToListAsync();
                return new Response(spm);
            } catch (Exception ex) {

                return new Response(ex.Message);
                throw;
            }
        }
    }
}
