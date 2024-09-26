using BodyBuilder.Application.Interfaces;
using BodyBuilder.Application.Mappings;
using BodyBuilder.Application.Services;
using BodyBuilder.Application.Utilities.JWT;
using BodyBuilder.Application.Validations;
using BodyBuilderApp.Resources;
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
            services.AddScoped<IWorkoutService, WorkoutService>();
            services.AddScoped<ISubProgrammeMovementService, SubProgrammeMovementService>();
            services.AddScoped<ISubProgrammeService, SubProgrammeService>();
            services.AddAutoMapper(typeof(AutoMapperConfiguration));
            services.AddScoped<IAuthService,AuthService>();
            services.AddScoped<ITokenCreate, TokenCreate>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IMovementService,MovementService>();
            services.AddScoped<IBodyPartService,BodyPartService>();
            services.AddScoped<IProgrammeService,ProgrammeService>();
            services.AddHttpContextAccessor();
            services.AddScoped<IMetricsService, MetricsService>();
            //services.AddSingleton<IRedisService, RedisService>();

            #region Validations Register 

            services.AddValidatorsFromAssemblyContaining(typeof(UserLoginDtoValidator));
            var serviceDescriptors = services
                .Where(descriptor => typeof(IValidator) != descriptor.ServiceType
                       && typeof(IValidator).IsAssignableFrom(descriptor.ServiceType)
                       && descriptor.ServiceType.IsInterface)
                .ToList();

            foreach (var descriptor in serviceDescriptors) {
                services.Add(new ServiceDescriptor(
                    typeof(IValidator),
                    p => p.GetRequiredService(descriptor.ServiceType),
                    descriptor.Lifetime));
            }

            #endregion Validations Register
        }
    }
}
