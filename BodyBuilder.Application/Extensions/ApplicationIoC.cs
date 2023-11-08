using BodyBuilder.Application.Interfaces;
using BodyBuilder.Application.Mappings;
using BodyBuilder.Application.Services;
using BodyBuilder.Application.Utilities.JWT;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BodyBuilder.Application.Extensions {
    public static class ApplicationIoC {
        public static void AddApplicationDependencies(this IServiceCollection services) {
            services.AddScoped<IUserService,UserService>();
            services.AddAutoMapper(typeof(AutoMapperConfiguration));
            services.AddScoped<IAuthService,AuthService>();
            services.AddScoped<ITokenHelper, JWTHelper>();
        }
    }
}
