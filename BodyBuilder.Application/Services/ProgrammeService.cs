using AutoMapper;
using BodyBuilder.Application.Dtos.Programme;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
using BodyBuilder.Infrastructure.Persistence.Context;
using BodyBuilderApp.Communication;
using Microsoft.EntityFrameworkCore;

namespace BodyBuilder.Application.Services {

    public class ProgrammeService : IProgrammeService {

        private readonly IProgrammeRepository _programmeRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ISubProgrammeRepository _subProgrammeRepository;
        private readonly ISubProgrammeMovementRepository _subProgrammeMovementRepository;
        private readonly BodyBuilderContext _context;

        public ProgrammeService(IProgrammeRepository programmeRepository, IMapper mapper, IUserService userService, ISubProgrammeRepository subProgrammeRepository, ISubProgrammeMovementRepository subProgrammeMovementRepository, BodyBuilderContext context) {
            _programmeRepository = programmeRepository;
            _mapper = mapper;
            _userService = userService;
            _subProgrammeRepository = subProgrammeRepository;
            _subProgrammeMovementRepository = subProgrammeMovementRepository;
            _context = context;
        }

        public Task<Response> AddAsync(ProgrammeAddDto programmeAddDto) {
            throw new NotImplementedException();
        }

        public async Task<Response> CreateCustomWorkoutAsync(Guid userId, CreateCustomProgramme customProgramme) {

            using (var transaction = await _context.Database.BeginTransactionAsync()) {
                try {
                    var user = await _userService.GetById(userId);
                    if (!user) {

                        return new Response() { Message = "Böyle bir kullanıcı sistemimizde mevcut değil" };
                    }


                    var programme = new Domain.Entities.Programme() {
                        Name = customProgramme.ProgrammeName,
                        UserId = userId,
                    };

                    await _programmeRepository.CreateAsync(programme);
                    await _programmeRepository.SaveAsync();

                    foreach (var sp in customProgramme.SubProgramme) {
                        var newSp = new Domain.Entities.SubProgramme() {

                            Name = sp.SubProgrammeName,
                            ProgrammeId = programme.Id
                        };

                        await _subProgrammeRepository.CreateAsync(newSp);
                        await _subProgrammeRepository.SaveAsync();

                        foreach (var spm in sp.SubProgrammeMovements) {
                            var newspmMv = new Domain.Entities.SubProgrammeMovement() {
                                MovementId = spm.MovementId,
                                SubProgrammeId = newSp.Id,
                                Reps = spm.Reps,
                                Sets = spm.Sets

                            };

                            await _subProgrammeMovementRepository.CreateAsync(newspmMv);
                            await _subProgrammeMovementRepository.SaveAsync();
                        }

                    }

                    var subProgrammeList = new List<Domain.Entities.SubProgramme>();

                    foreach (var item in customProgramme.SubProgramme) {
                        //subprogramme name created
                        var sp = new Domain.Entities.SubProgramme() {
                            Name = item.SubProgrammeName,
                        };

                        //subprogramme movements list
                        var ml = new List<BodyBuilder.Domain.Entities.SubProgrammeMovement>();

                        foreach (var m in item.SubProgrammeMovements) {
                            var spm = new SubProgrammeMovement() {
                                MovementId = m.MovementId,
                                SubProgrammeId = sp.Id,
                                Reps = m.Reps,
                                Sets = m.Sets
                            };
                            ml.Add(spm);
                        }
                        sp.SubProgrammeMovements = ml;
                        subProgrammeList.Add(sp);
                    }
                    await transaction.CommitAsync();
                    return new Response() {
                        Code = 200,
                        Resource = programme ?? new Programme()
                    };
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    return new Response(ex);
                    throw;
                }
            }

        }

        public async Task<Response> DeleteProgrammeAsync(Guid programmeId) {
            try {
                bool programmeExist = await _programmeRepository.Table.AnyAsync(p => p.Id == programmeId);
                if (!programmeExist) {
                    return new Response() { Success = false, Message = "Veritabanında böyle bir program bulunamadı" };
                }

                await _programmeRepository.DeleteAsync(programmeId);
                await _programmeRepository.SaveAsync();
                return new Response() { Code = 200, Message = "Program silme işlemi başarıyla gerçekleştirildi" };

            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
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
