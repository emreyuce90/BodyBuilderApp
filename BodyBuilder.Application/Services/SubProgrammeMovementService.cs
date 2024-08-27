using AutoMapper;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Domain.Entities;
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

        private readonly ISubProgrammeRepository _subProgrammeRepository;
        private readonly ISubProgrammeMovementRepository _subProgrammeMovementRepository;
        private readonly IMapper _mapper;

        public SubProgrammeMovementService(ISubProgrammeMovementRepository subProgrammeMovementRepository, IMapper mapper, ISubProgrammeRepository subProgrammeRepository) {
            _subProgrammeMovementRepository = subProgrammeMovementRepository;
            _mapper = mapper;
            _subProgrammeRepository = subProgrammeRepository;
        }

        public async Task<Response> GetAllBySubProgrammeIdAsync(Guid subProgrammeId) {
            try {
                var spm = await _subProgrammeMovementRepository.GetAllAsync(sp => sp.IsActive == true && sp.IsDeleted == false && sp.SubProgrammeId == subProgrammeId).Include(x => x.Movement).ToListAsync();

                return new Response(spm ?? new List<SubProgrammeMovement>());
            } catch (Exception ex) {

                return new Response(ex.Message);
                throw;
            }
        }

        public async Task<Response> UpdateSubProgrammeMovementByIdAsync(Guid subProgrammeId, List<SubProgrammeMovement> subProgrammeMovements) {
            try {
                if (String.IsNullOrWhiteSpace(subProgrammeId.ToString())) {
                    return new Response() { Code = 400, Message = "SubprogrammeId boş olamaz" };

                }
                
                var sp = await _subProgrammeRepository.Table.Include(x => x.SubProgrammeMovements).FirstOrDefaultAsync(sp => sp.Id == subProgrammeId);
                if (sp == null) {
                    return new Response() { Code = 200, Message = "Bu subprogrammeId 'ye ait veritabanında kayıt yok" };
                }

                foreach (var movement in sp.SubProgrammeMovements) {
                    await _subProgrammeMovementRepository.DeleteAsync(movement.Id);
                }

                var newMovements = new List<SubProgrammeMovement>();

                foreach (var movement in subProgrammeMovements)
                {
                    var mv = new SubProgrammeMovement();
                    mv.SubProgrammeId = subProgrammeId;
                    mv.MovementId = movement.MovementId;
                    mv.Sets = movement.Sets;
                    mv.Reps = movement.Reps;  
                    newMovements.Add(mv);
                }

                sp.SubProgrammeMovements = newMovements;
                await _subProgrammeRepository.SaveAsync();
                return new Response() { Code = 200, Resource = subProgrammeMovements };
                
            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }
    }
}
