﻿using BodyBuilder.Domain.Interfaces;
using BodyBuilder.Infrastructure.Persistence.Context;
using BodyBuilder.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Infrastructure.Persistence.Extensions {
    public static  class AddInfrastructureDependencies {
        public static void AddInfraDependencies(this IServiceCollection services) {
            services.AddScoped<IUserRepository, EfUserRepository>();
            services.AddScoped<IUserRefreshToken, EfUserRefreshToken>();
            services.AddScoped<IBodyPartRepository, EfBodyPartRepository>();
            services.AddScoped<IMovementRepository, EfMovementRepository>();
            services.AddScoped<ISubProgrammeMovementRepository, EfSubProgrammeMovementRepository>();
            services.AddScoped<ISubProgrammeRepository, EfSubProgrammeRepository>();
            services.AddScoped<IProgrammeRepository, EfProgrammeRepository>();
            services.AddScoped<IWorkoutRepository,EfWorkoutRepository>();
            services.AddScoped<IWorkoutMovementRepository,EfWorkoutMovementRepository>();
            services.AddScoped<IWorkoutMovementSetRepository,EfWorkoutMovementSetRepository>();
            services.AddScoped<IMetricsRepository, EfMetricsRepository>();
            services.AddScoped<IBodyMetrics,EfBodyMetrics>();
            services.AddScoped<IRoleRepository, EfRoleRepository>();
        }
    }
}
