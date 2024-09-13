using AutoMapper;
using BodyBuilder.Application.Dtos.Programme;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
using BodyBuilderApp.Communication;
using Microsoft.EntityFrameworkCore;

namespace BodyBuilder.Application.Services {

    public class ProgrammeService : IProgrammeService {

        private readonly IProgrammeRepository _programmeRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public ProgrammeService(IProgrammeRepository programmeRepository, IMapper mapper, IUserService userService) {
            _programmeRepository = programmeRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public Task<Response> AddAsync(ProgrammeAddDto programmeAddDto) {
            throw new NotImplementedException();
        }

        public async Task<Response> CreateCustomWorkoutAsync(Guid userId, CreateCustomProgramme customProgramme) {
            try {
                //check user exist
                var user = await _userService.GetById(userId);
                //if (!user) {

                //    return new Response() { Message = "Böyle bir kullanıcı sistemimizde mevcut değil" };
                //}

                //sp list
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
                            Sets = m.Sets,
                            //Movement = new Movement() {
                            //    BodyPartId = m.Movement.BodyPartId,
                            //    Description = m.Movement.Description,
                            //    ImageUrl = m.Movement.ImageUrl,
                            //    SubBodyPartId= m.Movement.SubBodyPartId,
                            //    VideoUrl = m.Movement.VideoUrl,
                            //    Title = m.Movement.Title,
                            //    Tip = m.Movement.Tip,
                                
                            //}
                        };
                        ml.Add(spm);
                    }
                    sp.SubProgrammeMovements = ml;
                    subProgrammeList.Add(sp);
                }

                //programı create etme
                var p = new Programme {
                    Name = customProgramme.ProgrammeName,
                    UserId = userId,
                    SubProgrammes = subProgrammeList
                };

                var newProgramme = await _programmeRepository.CreateAsync(p);
                await _programmeRepository.SaveAsync();

                return new Response() {
                    Code = 200,
                    Resource = newProgramme ?? new Programme()
                };
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
