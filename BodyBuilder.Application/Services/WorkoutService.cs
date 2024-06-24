using AutoMapper;
using BodyBuilder.Application.Dtos.Movement;
using BodyBuilder.Application.Dtos.Workout;
using BodyBuilder.Application.Dtos.WorkoutMovement;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
using BodyBuilderApp.Communication;
using BodyBuilderApp.Resources;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Services {
    public class WorkoutService : IWorkoutService {

        private readonly IWorkoutRepository _workoutRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<WorkoutAddDto> _validator;
        private readonly IWorkoutMovementRepository _workoutMovementRepository;
        public WorkoutService(IWorkoutRepository workoutRepository, IMapper mapper, IValidator<WorkoutAddDto> validator, IWorkoutMovementRepository workoutMovementRepository) {
            _workoutRepository = workoutRepository;
            _mapper = mapper;
            _validator = validator;
            _workoutMovementRepository = workoutMovementRepository;
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
                    var workoutCount = await _workoutRepository.GetAllAsync(w => w.UserId.ToString() == "7aaf453f-56ea-4f7d-8877-4cec29072bfe" && !w.IsDeleted && w.IsActive,false).CountAsync(); ;
                    return new Response() { Code = 200, Message = "Workout başarıyla kaydedildi" ,Resource=workoutCount };
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
                var dbWorkout = await _workoutRepository.GetSingle(w => w.IsActive == true && w.IsDeleted == false && w.UserId == userId, false);
                if (dbWorkout == null) {
                    return new Response() { Success = false, Message = "Veritabanında böyle bir kayıt bulunamadı" };
                }
                return new Response(_mapper.Map<WorkoutDto>(dbWorkout));
            } catch (Exception ex) {
                return new Response(ex.Message);

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
