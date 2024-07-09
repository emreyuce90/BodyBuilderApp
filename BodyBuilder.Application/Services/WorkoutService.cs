using AutoMapper;
using BodyBuilder.Application.Dtos.Workout;
using BodyBuilder.Application.Dtos.WorkoutMovement;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
using BodyBuilder.Infrastructure.Persistence.Context;
using BodyBuilderApp.Communication;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BodyBuilder.Application.Services {
    public class WorkoutService : IWorkoutService {
        private readonly BodyBuilderContext _context;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<WorkoutAddDto> _validator;
        private readonly IWorkoutMovementRepository _workoutMovementRepository;
        private readonly ISubProgrammeRepository _subProgrammeRepository;
        public WorkoutService(IWorkoutRepository workoutRepository, IMapper mapper, IValidator<WorkoutAddDto> validator, IWorkoutMovementRepository workoutMovementRepository, ISubProgrammeRepository subProgrammeRepository, BodyBuilderContext context) {
            _workoutRepository = workoutRepository;
            _mapper = mapper;
            _validator = validator;
            _workoutMovementRepository = workoutMovementRepository;
            _subProgrammeRepository = subProgrammeRepository;
            _context = context;
        }

        public async Task<Response> CreateWorkoutMovement(Guid workoutId, List<WorkoutMovementAddDto> movementAddDtos) {
            try {
                if (movementAddDtos.Count > 0) {
                    var workoutMovementList = new List<WorkoutMovement>();
                    foreach (var workoutMovement in movementAddDtos) {
                        var wm = new WorkoutMovement {
                            WorkoutId = workoutId,
                            MovementId = workoutMovement.MovementId,
                        };
                        var workoutSetList = new List<WorkoutMovementSet>();
                        foreach (var set in workoutMovement.MovementSetDtos) {
                            var movementSet = new WorkoutMovementSet() {
                                Reps = set.Reps,
                                SetNumber = set.SetNumber,
                                Weight = set.Weight,
                            };
                            workoutSetList.Add(movementSet);
                        }
                        wm.WorkoutMovementSets = workoutSetList;
                        workoutMovementList.Add(wm);
                    }
                    await _workoutMovementRepository.Table.AddRangeAsync(workoutMovementList);
                    await _workoutMovementRepository.SaveAsync();
                    //kullanıcının antrenmanları geriye dönülür
                    //var workoutCount = await _workoutRepository.GetAllAsync(w => w.UserId.ToString() == "7aaf453f-56ea-4f7d-8877-4cec29072bfe" && !w.IsDeleted && w.IsActive,false).CountAsync(); ;
                    var workoutCount =await  GetWorkoutCountByUserIdAsync(Guid.Parse("7aaf453f-56ea-4f7d-8877-4cec29072bfe"));
                    return new Response() { Code = 200, Message = "Workout başarıyla kaydedildi" ,Resource=workoutCount.Resource };
                } else {
                    return new Response() { Code = 400, Message = "Workout listesi boş!" };

                }
                //init list


            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
            return new Response();
        }

        public async Task<Response> FinishWorkout(Guid workoutId, TimeSpan endTime,int duration) {
            try {
                //find workout from db without tracking
                var dbWorkout = await _workoutRepository.GetSingle(w => w.IsActive == true && w.IsDeleted == false && w.Id == workoutId, false);
                if (dbWorkout == null) {
                    return new Response() { Success = false, Message = "Veritabanında böyle bir kayıt bulunamadı" };
                }
                dbWorkout.EndTime = endTime;
                dbWorkout.Duration = duration;
                _workoutRepository.UpdateAsync(dbWorkout);
                await _workoutRepository.SaveAsync();
                return new Response() { Code = 200, Success = true };
            } catch (Exception ex) {
                return new Response(ex.Message);
                throw;
            }
        }

        public async Task<Response> GetWorkoutByIdAsync(Guid workoutId) {
            try {
                var dbWorkout = await _workoutRepository.GetSingle(w => w.IsActive == true && w.IsDeleted == false && w.Id == workoutId, false);
                if (dbWorkout == null) {
                    return new Response() { Success = false, Message = "Veritabanında böyle bir kayıt bulunamadı" };
                }
                return new Response(_mapper.Map<WorkoutDto>(dbWorkout));
            } catch (Exception ex) {
                return new Response(ex.Message);

                throw;
            }
        }

        public async Task<Response> GetWorkoutBySubprogrammeIdAsync(Guid subProgrammeId) {
            try {
                var dbWorkout = await _workoutRepository.GetSingle(w => w.IsActive == true && w.IsDeleted == false && w.SubProgrammeId == subProgrammeId, false);
                if (dbWorkout == null) {
                    return new Response() { Success = false, Message = "Veritabanında böyle bir kayıt bulunamadı" };
                }
                return new Response(_mapper.Map<WorkoutDto>(dbWorkout));
            } catch (Exception ex) {
                return new Response(ex.Message);

                throw;
            }
        }

        public async Task<Response> GetWorkoutByUserIdAsync(Guid userId) {
            try {
                var dbWorkout = await _workoutRepository.GetAllAsync(w => w.IsActive == true && w.IsDeleted == false && w.UserId == userId, false).ToListAsync();
                if (dbWorkout == null) {
                    return new Response() { Success = false, Message = "Veritabanında böyle bir kayıt bulunamadı" };
                }

                var w = _context.WorkoutLogs.FromSqlRaw("SELECT * from gymguru.GetWorkoutLogsByUserId({0})  ORDER BY 1 desc", userId).ToList();

                return new Response(w);
            } catch (Exception ex) {
                return new Response(ex.Message);

                throw;
            }
        }

        public async Task<Response> GetWorkoutCountByUserIdAsync(Guid userId) {
            try {
                var dbWorkout = await _workoutRepository.GetAllAsync(w => w.IsActive == true && w.IsDeleted == false && w.UserId == userId, false).ToListAsync();
                if (dbWorkout == null) {
                    return new Response() { Success = false, Message = "Veritabanında böyle bir kayıt bulunamadı" };
                }

                var w = _context.WorkoutLogs.FromSqlRaw("SELECT * from gymguru.GetWorkoutLogsByUserId({0})  ORDER BY 1 desc", userId).ToList();

                return new Response(w.Count);
            } catch (Exception ex) {
                return new Response(ex.Message);

                throw;
            }
        }

        public async Task<Response> GetWorkoutDetailByWorkoutIdAsync(Guid workoutId) {
            try {
                var dbWorkout = await _workoutRepository.GetAllAsync(w => w.IsActive == true && w.IsDeleted == false && w.Id== workoutId, false).ToListAsync();
                if (dbWorkout == null) {
                    return new Response() { Success = false, Message = "Veritabanında böyle bir kayıt bulunamadı" };
                }
                var workoutLogDetail = await _context.WorkoutLogDetails.FromSqlRaw("SELECT * FROM gymguru.GetWorkoutLogDetail({0})",workoutId).ToListAsync();

                if (workoutLogDetail == null) {
                    return new Response() { Code = 200, Message = "Bu workoutId ye ait antrenman kayıtları bulunamadı" };
                }

                var workoutLogDetailDto = new WorkoutLogDetailDto() {
                    WorkoutId = workoutLogDetail.First().WorkoutId,
                    WorkoutName = workoutLogDetail.First().WorkoutName,
                    WorkoutDate = workoutLogDetail.First().WorkoutDate,
                    WorkoutTime = workoutLogDetail.First().WorkoutTime,
                    Duration = workoutLogDetail.First().Duration,
                    MovementDetails = workoutLogDetail.GroupBy(w => w.Title).Select(g=> new MovementDetail {
                        MovementName = g.Key,
                        MovementSets = g.Select(m=> new MovementSets {
                            Reps=m.Reps,
                            SetNumber=m.SetNumber,
                            Weight = m.Weight
                        }).ToList()
                    }).ToList()
                };


                return new Response() { Code=200,Resource= workoutLogDetailDto };
            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }

        public async Task<Response> RemoveWorkoutAsync(Guid workoutId) {
            try {
                var dbWorkout = await _workoutRepository.GetSingle(w => w.IsActive == true && w.IsDeleted == false && w.Id == workoutId, false);
                if (dbWorkout == null) {
                    return new Response() { Success = false, Message = "Veritabanında böyle bir kayıt bulunamadı" };
                }
                dbWorkout.IsDeleted = true;
                dbWorkout.IsActive = false;
                _workoutRepository.UpdateAsync(dbWorkout);
                await _workoutRepository.SaveAsync();
                return new Response() { Code = 200, Message = "Kayıt başarıyla silindi", Success = true };
            } catch (Exception ex) {
                return new Response(ex.Message);

                throw;
            }
        }

        public async Task<Response> StartWorkoutAsync(WorkoutAddDto workoutAddDto) {
            try {
                var validationResult = await _validator.ValidateAsync(workoutAddDto);
                if (!validationResult.IsValid) {
                    var message = string.Empty;
                    foreach (var item in validationResult.Errors) {
                        message += item.ErrorMessage;
                    }
                    return new Response() { Message = message, Success = false };
                }

                var workout = await _workoutRepository.CreateAsync(_mapper.Map<Workout>(workoutAddDto));
                await _workoutRepository.SaveAsync();
                return new Response(workout);
            } catch (Exception ex) {
                return new Response(ex.Message);

                throw;
            }
        }

        public async Task<Response> StopWorkout(Guid workoutId) {
            try {
                var workout = _workoutRepository.GetSingle(w => w.Id == workoutId && !w.IsDeleted && w.IsActive);
                if (workout == null) {
                    return new Response() { Code = 400, Message = "Bu workoutId ye ait veritabanı kaydı bulunamadı" };
                }
                await _workoutRepository.DeleteAsync(workoutId);
                await _workoutRepository.SaveAsync();
                return new Response(workout);
            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }
    }
}
