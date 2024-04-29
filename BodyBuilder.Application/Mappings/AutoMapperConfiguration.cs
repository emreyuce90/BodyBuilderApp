using AutoMapper;
using BodyBuilder.Application.Dtos.Bodypart;
using BodyBuilder.Application.Dtos.Movement;
using BodyBuilder.Application.Dtos.User;
using BodyBuilder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Mappings
{
    public class AutoMapperConfiguration :Profile{
        public AutoMapperConfiguration()
        {
            CreateMap<BodyPart, BodyPartDto>().ReverseMap();
            CreateMap<BodyPartUpdateDto,BodyPart>().ReverseMap();
            CreateMap<BodyPartAddDto, BodyPart>().ReverseMap();
            CreateMap<Movement, MovementDto>().ReverseMap();
            CreateMap<MovementAddDto, Movement>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserAddDto, User>().ReverseMap();

        }
    }
}
