using Application.Services;
using Application.UseCases.AuthUseCases;
using Application.UseCases.Managers;
using Application.UseCases.TaskEntityUseCases;
using Application.UseCases.UserUsecases;
using AutoMapper;
using Domain.IRepositories;
using Domain.Repositories;
using Infrustructure.Identity.JWT;
using Infrustructure.Persistance.DbContexts;
using Infrustructure.Persistance.Repositories;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration) 
        {

            #region Register AutomapperProfile

            var mapperConfigs = new MapperConfiguration(cfgs =>
            {
                cfgs.AddProfile(typeof(AutomapperProfile));
            });
            services.AddSingleton<IMapper>(ss => mapperConfigs.CreateMapper());

            #endregion

            services.AddScoped<ITaskEntityRepository, TaskEntityRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IServiceWrapper, ServiceWrapper>();
            services.AddScoped<IAuthUser,AuthUser>();

            services.AddScoped<CreateUserUC>();
            services.AddScoped<GetAllUserUC>();
            services.AddScoped<GetByIdUserUC>();
            services.AddScoped<UpdateUserUC>();
            services.AddScoped<DeleteUserUC>();
            services.AddScoped<AuthUserUC>();
            services.AddScoped<IUserUseCaseManager, UserUseCaseManager>();

            services.AddScoped<CreateTaskEntityUC>();
            services.AddScoped<GetAllTaskEntityUC>();
            services.AddScoped<ITaskEntityUseCaseManager, TaskEntityUseCaseManager>();

            var jwtSetting = new JwtSetting();
            configuration.GetSection("JwtSetting").Bind(jwtSetting);
            services.AddSingleton(jwtSetting);
            services.AddTransient<JwtValidator>();
            services.AddScoped<JwtGenerator>();

            return services;
        }
    }
}
