using BodyBuilder.Domain.Interfaces;
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
            services.AddDbContext<BodyBuilderContext>(opt => opt.UseSqlServer("Server=localhost;Database=bbdatabase;Integrated Security=True;TrustServerCertificate=True;"));
            services.AddScoped<IUserRepository, EfUserRepository>();
            services.AddScoped<IUserRefreshToken, EfUserRefreshToken>();
        }
    }
}
