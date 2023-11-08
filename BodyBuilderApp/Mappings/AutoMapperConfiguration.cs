using AutoMapper;
using BodyBuilder.Application.Dtos.User;
using BodyBuilder.Domain.Entities;
using BodyBuilderApp.Communication;

namespace BodyBuilderApp.Mappings {
    public class AutoMapperConfiguration : Profile {
        public AutoMapperConfiguration() {
            CreateMap(typeof(Response<>), typeof(Response<>)).ConvertUsing(typeof(ResponseConverter<,>));
            
        }
        private class ResponseConverter<T, K> : ITypeConverter<Response<T>, Response<K>> {
            public Response<K> Convert(Response<T> sourceMember, Response<K> destination, ResolutionContext context) =>
                sourceMember.Success
                    ? new Response<K>(context.Mapper.Map<K>(sourceMember.Resource)) {
                        Message = sourceMember.Message,
                        Comment = sourceMember.Comment
                    }
                    : new Response<K>() {
                        Code = sourceMember.Code,
                        Comment = sourceMember.Comment,
                        Details = sourceMember.Details,
                        Message = sourceMember.Message,
                        Success = false,
                    };
        }
    }
}

