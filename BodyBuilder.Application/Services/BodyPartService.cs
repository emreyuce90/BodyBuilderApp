using AutoMapper;
using BodyBuilder.Application.Dtos.Bodypart;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
using BodyBuilderApp.Communication;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Services {
    public class BodyPartService : IBodyPartService {

        private readonly IDistributedCache _distributedCache;
        private readonly IBodyPartRepository _bodyPartRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<BodyPartAddDto> _validator;
        private readonly IValidator<BodyPartUpdateDto> _updatevalidator;
        private readonly ILogger<BodyPartService> _logger;

        public BodyPartService(IBodyPartRepository bodyPartRepository, IMapper mapper, IValidator<BodyPartAddDto> validator, IValidator<BodyPartUpdateDto> updatevalidator, ILogger<BodyPartService> logger,IDistributedCache distributedCache) {
            _bodyPartRepository = bodyPartRepository;
            _mapper = mapper;
            _validator = validator;
            _updatevalidator = updatevalidator;
            _logger = logger;
            _distributedCache = distributedCache;
        }

        public async Task<Response> AddAsync(BodyPartAddDto bodyPartAddDto) {

            try {
                var validator = await _validator.ValidateAsync(bodyPartAddDto);
                if (!validator.IsValid) {
                    var message = string.Empty;
                    foreach (var item in validator.Errors) {
                        message += item.ErrorMessage;
                    }
                    return new Response() { Message = message, Success = false };
                }

                var addedBodyPart = await _bodyPartRepository.CreateAsync(_mapper.Map<BodyPart>(bodyPartAddDto));
                addedBodyPart.CreatedDate = DateTime.Now;
                addedBodyPart.IsActive = true;
                await _bodyPartRepository.SaveAsync();
                return new Response<BodyPartDto>() {
                    Code = 200,
                    Success = true,
                    Resource = _mapper.Map<BodyPartDto>(addedBodyPart)
                };
            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }

        public async Task<Response> GetAllAsync() {
            try {
                //redis integration
                string cacheKey = "allBodyParts";

                string? cachedData = _distributedCache.GetString(cacheKey);

                List<BodyPart>? bodyParts = null;


                if (cachedData == null) {

                    bodyParts = await _bodyPartRepository.GetAllAsync(b => b.IsActive == true).ToListAsync();
                    var stringData = JsonSerializer.Serialize(bodyParts);
                    await _distributedCache.SetStringAsync(key:cacheKey,value:stringData);
                } else {
                    bodyParts = JsonSerializer.Deserialize<List<BodyPart>>(cachedData);
                }


                
                var mappedValue = _mapper.Map<List<BodyPartDto>>(bodyParts);
                return new Response<List<BodyPartDto>>(mappedValue);
            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }

        public async Task<Response> GetByIdAsync(Guid Id) {
            try {
                var bodyPart = await _bodyPartRepository.GetSingle(b => b.IsActive == true && b.Id == Id);
                return new Response<BodyPartDto> { Code = 200, Resource = _mapper.Map<BodyPartDto>(bodyPart) };
            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }

        public async Task<Response> GetSubBodyPartsByBodyPartIdAsync(Guid bodypartId) {

            if (String.IsNullOrWhiteSpace(bodypartId.ToString())) {
                return new Response() {
                    Code = 400,
                    Message = "BodypartId boş geçilemez"
                };
            }
            try {
                var bodyParts = await _bodyPartRepository.Table.Include(b => b.SubBodyParts).FirstOrDefaultAsync(b => b.Id == bodypartId && b.IsDeleted == false && b.IsActive == true);
                if (bodyParts == null) {
                    return new Response() { Code = 400, Message = "Göndermiş olduğunuz bodypartId ye ait kayıt veritabanında bulunamadı" };
                } else {

                    return new Response() {
                        Code = 200,
                        Resource = bodyParts.SubBodyParts.ToList() ?? new List<SubBodyPart>()
                    };
                }
            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }

        public async Task<Response> UpdateAsync(BodyPartUpdateDto bodyPartUpdateDto) {
            //check data is valid
            try {
                var validator = await _updatevalidator.ValidateAsync(bodyPartUpdateDto);
                if (!validator.IsValid) {
                    var message = string.Empty;
                    foreach (var item in validator.Errors) {
                        message += item.ErrorMessage;
                    }
                    return new Response() { Message = message, Success = false };
                }

                //check Id exist
                var bodyPart = await _bodyPartRepository.GetSingle(b => b.IsActive == true && b.Id == bodyPartUpdateDto.Id, false);
                if (bodyPart == null) {
                    return new Response() { Success = false, Message = "Böyle bir Id ye ait kayıt bulunamadı" };
                }

                //update data
                bodyPart.Name = bodyPartUpdateDto.Name;
                bodyPart.UpdatedDate = DateTime.Now;
                _bodyPartRepository.UpdateAsync(bodyPart);
                await _bodyPartRepository.SaveAsync();
                return new Response<BodyPartDto>(_mapper.Map<BodyPartDto>(bodyPart));

            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }

        }
    }
}
