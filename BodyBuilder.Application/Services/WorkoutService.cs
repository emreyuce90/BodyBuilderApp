using AutoMapper;
using BodyBuilder.Application.Dtos.Workout;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
using BodyBuilderApp.Communication;
using BodyBuilderApp.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Services {
    public class WorkoutService : IWorkoutService {

        private readonly IWorkoutRepository _workoutRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<WorkoutAddDto> _validator;
        public WorkoutService(IWorkoutRepository workoutRepository, IMapper mapper, IValidator<WorkoutAddDto> validator) {
            _workoutRepository = workoutRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Response> FinishWorkout(Guid workoutId, TimeSpan endTime) {
            try {
                //find workout from db without tracking
                var dbWorkout = await _workoutRepository.GetSingle(w=>w.IsActive == true && w.IsDeleted == false && w.Id == workoutId,false);
                if(dbWorkout == null) {
                    return new Response() { Success = false, Message = "Veritabanında böyle bir kayıt bulunamadı" };
                }
                dbWorkout.EndTime = endTime;
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
                dbWorkout.IsActive= false;
                _workoutRepository.UpdateAsync(dbWorkout);
                await _workoutRepository.SaveAsync();
                return new Response() { Code=200,Message="Kayıt başarıyla silindi",Success=true};
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
    }
}
