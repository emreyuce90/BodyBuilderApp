
using BodyBuilder.Application.Extensions;
using BodyBuilder.Application.Utilities.JWT;
using BodyBuilder.Application.ValidationRules.User;
using BodyBuilder.Infrastructure.Persistence.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BodyBuilderApp {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Cors Policy
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("AllowOrigin", builder => builder.WithOrigins("https://localhost:7031/"));
            });
            #endregion

            #region Validations Register 

            builder.Services.AddValidatorsFromAssemblyContaining(typeof(UserAddDtoValidator));
            var serviceDescriptors = builder.Services
                .Where(descriptor => typeof(IValidator) != descriptor.ServiceType
                       && typeof(IValidator).IsAssignableFrom(descriptor.ServiceType)
                       && descriptor.ServiceType.IsInterface)
                .ToList();

            foreach (var descriptor in serviceDescriptors) {
                builder.Services.Add(new ServiceDescriptor(
                    typeof(IValidator),
                    p => p.GetRequiredService(descriptor.ServiceType),
                    descriptor.Lifetime));
            }

            #endregion Validations Register

            #region Jwt
            var token = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,

                    ValidAudience = token.Audience,
                    ValidIssuer = token.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(token.SecurityKey)
                };
            });

            #endregion

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddInfraDependencies();
            builder.Services.AddApplicationDependencies();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors(builder => builder.WithOrigins("https://localhost:7031/").AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}