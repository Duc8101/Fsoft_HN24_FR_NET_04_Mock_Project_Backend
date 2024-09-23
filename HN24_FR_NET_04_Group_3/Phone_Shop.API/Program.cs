using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Phone_Shop.API.Middleware;
using Phone_Shop.Common.Configuration;
using Phone_Shop.DataAccess.DBContext;
using Phone_Shop.DataAccess.Repositories.Common;
using Phone_Shop.DataAccess.UnitOfWorks;
using Phone_Shop.Services.Base;
using System.Reflection;
using System.Text;

namespace Phone_Shop.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Here Enter JWT Token with Bearer format: Bearer[space][token]"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[]{ }
                    }
                });
            }
);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = ConfigData.JwtAudience,
                    ValidIssuer = ConfigData.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigData.JwtKey))
                };
            });

            builder.Services.AddCors(o => o.AddPolicy("AllowOrigin", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            // register db context
            builder.Services.AddDbContext<PhoneShopContext>();

            // register auto mapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            // register repository and UoW
            builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            // -------------------------- register service ------------------------
            Assembly assembly = typeof(BaseService).Assembly;
            Func<string, bool> condition = name => name.EndsWith("Service");
            List<Type> types = assembly.GetTypes().Where(t => condition(t.Name) && t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (Type type in types)
            {
                Type[] implementedInterfaces = type.GetInterfaces();
                foreach (Type serviceType in implementedInterfaces)
                {
                    builder.Services.AddScoped(serviceType, type);
                }
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
            app.UseMiddleware<UnauthorizedMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();  
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("AllowOrigin");
            app.MapControllers();

            app.Run();

        }
    }
}

