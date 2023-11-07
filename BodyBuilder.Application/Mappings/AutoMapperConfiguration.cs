using AutoMapper;
using BodyBuilder.Application.Dtos;
using BodyBuilder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Mappings {
    public class AutoMapperConfiguration :Profile{
        public AutoMapperConfiguration()
        {
            CreateMap<User, UserDto>();
        }
    }
}
