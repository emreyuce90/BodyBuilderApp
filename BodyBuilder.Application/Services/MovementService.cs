using AutoMapper;
using BodyBuilder.Application.Dtos.Bodypart;
using BodyBuilder.Application.Dtos.Movement;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
using BodyBuilderApp.Communication;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Services {
    public class MovementService : IMovementService {
        private readonly IMovementRepository _movementRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<MovementAddDto> _validator;
        private readonly IValidator<MovementUpdateDto> _updatevalidator;

        public MovementService(IMovementRepository movementRepository, IMapper mapper, IValidator<MovementAddDto> validator, IValidator<MovementUpdateDto> updatevalidator) {
            _movementRepository = movementRepository;
            _mapper = mapper;
            _validator = validator;
            _updatevalidator = updatevalidator;
        }

        public async Task<Response> AddAsync(MovementAddDto entity) {
            try {
                var validator = await _validator.ValidateAsync(entity);
                if (!validator.IsValid) {
                    var message = string.Empty;
                    foreach (var item in validator.Errors) {
                        message += item.ErrorMessage;
                    }
                    return new Response() { Message = message, Success = false };
                }

                var movement = await _movementRepository.CreateAsync(_mapper.Map<Movement>(entity));
                movement.CreatedDate = DateTime.Now;
                movement.IsActive = true;
                await _movementRepository.SaveAsync();
                return new Response() { Code = 200, Resource = _mapper.Map<MovementDto>(movement) };
            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }

        public async Task<Response> GetAllAsync() {
            try {
                var movements = await _movementRepository.GetAllAsync(m => m.IsActive == true).ToListAsync();
                return new Response() { Code = 200, Resource = _mapper.Map<List<MovementDto>>(movements) };
            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }

        public async Task<Response> GetByIdAsync(Guid Id) {
            try {
                var movement = await _movementRepository.GetSingle(m => m.IsActive == true && m.Id == Id);
                return new Response { Code = 200, Resource = _mapper.Map<MovementDto>(movement), Success = true };
            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }

        public async Task<Response> UpdateAsync(MovementUpdateDto movementUpdateDto) {
            try {
                var validator = await _updatevalidator.ValidateAsync(movementUpdateDto);
                if (!validator.IsValid) {
                    var message = string.Empty;
                    foreach (var item in validator.Errors) {
                        message += item.ErrorMessage;
                    }
                    return new Response() { Message = message, Success = false };
                }

                //check Id exist
                var movementdb = await _movementRepository.GetById(movementUpdateDto.Id);
                if (movementdb == null) {
                    return new Response() { Success = false, Message = "Böyle bir Id ye ait kayıt bulunamadı" };
                }

                //update data
                movementdb.VideoUrl = movementUpdateDto.VideoUrl;
                movementdb.UpdatedDate = DateTime.Now;
                movementdb.Description = movementUpdateDto.Description;
                movementdb.ImageUrl = movementUpdateDto.ImageUrl;
                movementdb.Tip = movementUpdateDto.Tip;
                movementdb.Title = movementUpdateDto.Title;
                var updatedData = _movementRepository.UpdateAsync(movementdb);
                await _movementRepository.SaveAsync();
                return new Response<MovementDto>(_mapper.Map<MovementDto>(updatedData));

            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }



    }
}
