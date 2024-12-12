
using BodyBuilder.Application.Extensions;
using BodyBuilder.Application.Utilities.JWT;
using BodyBuilder.Application.ValidationRules.User;
using BodyBuilder.Infrastructure.Persistence.Context;
using BodyBuilder.Infrastructure.Persistence.Extensions;
using BodyBuilderApp.Extensions;
using BodyBuilderApp.OpenTelemetry;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using System.Text;

namespace BodyBuilderApp {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            //builder.Services.AddElasticExt();
            builder.Services.Configure<ElasticSearchSettings>(builder.Configuration.GetSection(nameof(ElasticSearchSettings)));

            var elasticConfigration = builder.Configuration
     .GetSection(nameof(ElasticSearchSettings))
     .Get<ElasticSearchSettings>();

            #region Serilog
            // Serilog'u enrichers ile yapılandırıyoruz
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()          //Hata detayları
                .Enrich.WithEnvironmentName()          // Ortam (development, production vs.) bilgisi
                .Enrich.WithProperty("Appname",builder.Environment.EnvironmentName)
                .Enrich.WithMachineName()              // Makine adı
                .Enrich.WithProcessId()                // İşlem ID'si
                .Enrich.WithProcessName()              // İşlem adı
                .Enrich.WithThreadId()                 // Thread ID'si
                .Enrich.WithThreadName()               // Thread adı
                .WriteTo.Console()                     // Logları console'a yaz
                .WriteTo.File("logs/logfile.txt", rollingInterval: RollingInterval.Day)  // Günlük dosyalara yaz
                .WriteTo.Seq("http://172.19.165.27:5342")  // Seq'e gönder
                .WriteTo.Elasticsearch(new(new Uri(elasticConfigration!.BaseUrl)) {
                    AutoRegisterTemplate= true,
                    OverwriteTemplate = true,
                    TemplateName = "bodybuilderapp",
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    IndexFormat=$"{elasticConfigration.IndexName}-{builder.Environment.EnvironmentName}-log-"+ "{0:yyy.MM.dd}",
                    TypeName=null,
                    BatchAction=ElasticOpType.Create,
                    ModifyConnectionSettings = x=>x.BasicAuthentication(elasticConfigration.UserName,elasticConfigration.Password),
                    CustomFormatter = new ElasticsearchJsonFormatter(),
                    
                })
                .CreateLogger();

            builder.Host.UseSerilog();

            #endregion

            #region Cors Policy
            builder.Services.AddCors(options => {
                options.AddPolicy("ReactAppPolicy",
                    builder => {
                        builder.WithOrigins("http://localhost:8081","http://192.168.1.93:8081")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });
            #endregion

            #region Jwt
            var token = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero,
                    ValidAudience = token.Audience,
                    ValidIssuer = token.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.SecurityKey))
                };
            });

            #endregion


            #region OpenTelemetry
            builder.Services.AddOpenTelemetryExt(builder.Configuration);
            #endregion


            //appsettings.json dosyas?n?n yap?land?rmas?n? yükler
            var configuration = builder.Configuration;
            //dependency injection için ba??ml?l??? yükler
            builder.Services.AddSingleton<IConfiguration>(configuration);

            builder.Services.AddDbContext<BodyBuilderContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("remoteDb")));


            builder.Services.AddControllers()
                       .AddNewtonsoftJson(options => {
                           options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                           options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                       });



            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Version = "v1",
                    Title = "BodyBuilder App Token Based Auth Project",
                    Description = "BodyBuilder App Token Based Auth Project",
                    TermsOfService = new Uri("https://www.linkedin.com/in/mreyuce/"),
                    Contact = new OpenApiContact {
                        Email = "emreyuce9039@gmail.com",
                        Name = "Emre Yüce",
                        Url = new Uri("https://www.linkedin.com/in/mreyuce/")
                    },
                    License = new OpenApiLicense { Name = "Emre Yüce Licence", Url = new Uri("https://www.linkedin.com/in/mreyuce/") }

                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                // Döngüleri önlemek için
                c.UseAllOfToExtendReferenceSchemas();

                // Model döngü ve karmaşık yapıları daha iyi ele almak için
                c.CustomSchemaIds(type => type.FullName);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    new string[] {}
                        }
                    });

            });

            //builder.Services.AddStackExchangeRedisCache(opt =>
            //{
            //    //opt.Configuration = "redisService:6379";
            //    opt.Configuration = "172.19.165.27:6379";
            //});


            builder.Services.AddInfraDependencies();
            builder.Services.AddApplicationDependencies();
            var app = builder.Build();
            app.UseCors("ReactAppPolicy");
            app.ConfigureExceptionHandler();
            app.UseStaticFiles(new StaticFileOptions {
                FileProvider = new PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath, "wwwroot")),
                RequestPath = "/api.gymguru.com.tr"
            });


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            } else {
                // Üretim ortamında da Swagger'i etkinleştirmek için bu kısmı kaldırın
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty; // Ana sayfada Swagger UI'yı göstermek için
                });
            }

            app.UseMiddleware<RequestAndResponseTelemetryMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}