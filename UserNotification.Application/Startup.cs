using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using UserNotification.Application.Authentication;
using UserNotification.Application.Authentication.Interfaces;
using UserNotification.Application.Filters;
using UserNotification.Application.Services;
using UserNotification.Domain.Interfaces.Services;
using UserNotification.Domain.Validators;
using UserNotification.Infra.DBContext;
using UserNotification.Infra.Dependencies;

namespace UserNotification
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration["ConnectionStrings:SQLServer"];

            services.AddDbContext<SQLDBContext>(opt => opt.UseSqlServer(connectionString));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddRepositoriesDependency();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ITokenServices, TokenServices>();

            services.AddMvc(x =>
                {
                    x.Filters.Add(typeof(FluentValidationFilter));
                })
                .AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    x.SerializerSettings.Converters.Add(new StringEnumConverter());
                    x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .AddFluentValidation(x =>
                {
                    x.RegisterValidatorsFromAssemblyContaining<LoginCommandValidator>();
                });

            services.AddControllers();

            services.AddHttpClient();

            services.AddApiVersioning(x =>
            {
                x.ReportApiVersions = true;
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.DefaultApiVersion = new ApiVersion(2, 0);
            });


            var secret = Encoding.ASCII.GetBytes(Settings.Secrets);

            services.AddAuthentication(x =>
              {
                  x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              }).AddJwtBearer(x =>
              {
                  x.RequireHttpsMetadata = false;
                  x.SaveToken = true;
                  x.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = false,
                      IssuerSigningKey = new SymmetricSecurityKey(secret),
                      ValidateIssuer = false,
                      ValidateAudience = false
                  };
              });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("2.0", new OpenApiInfo
                {
                    Version = "2.0",
                    Title = "API Notificações de Usuários",
                    Description = $@"Uma API feita ASP.NET Core Web API com objetivo de cadastramento de Lembretes/Notificações do Usuário. 
                                     Aplicado estrutura de DDD e Design Patterns SOLID e Handlers.",
                    Contact = new OpenApiContact
                    {
                        Name = "Alan Evandro Barboza",
                        Email = string.Empty,
                        Url = new Uri("https://github.com/alanebarboza"),
                    },
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public static void EnsureMigrationOfContext<T>(IApplicationBuilder app) where T : DbContext
        {
            var context = app.ApplicationServices.GetService<T>();
            context.Database.Migrate();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/2.0/swagger.json", "User Notifications - Versão 2.0"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
