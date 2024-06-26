﻿using Application.Services;
using Application.Services_Interfaces;
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
using Microsoft.AspNetCore.Routing;
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
            //Jwt Services
            var jwtSetting = new JwtSetting();
            configuration.GetSection("JwtSetting").Bind(jwtSetting);
            services.AddSingleton(jwtSetting);
            services.AddScoped<JwtGenerator>();
            services.AddScoped<JwtValidator>();


            //AutoMapper
            var mapperConfigs = new MapperConfiguration(cfgs =>
            {
                cfgs.AddProfile(typeof(AutomapperProfile));
            });
            services.AddSingleton<IMapper>(ss => mapperConfigs.CreateMapper());

            // Sevices
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();
            services.AddScoped<ITaskEntityRepository, TaskEntityRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IServiceWrapper, ServiceWrapper>();

            services.AddScoped<ITokenServices, TokenServices>();
            services.AddScoped<ISecurityHelper, SecurityHelper>();

            //UseCeses
            services.AddScoped<CreateUserUC>();
            services.AddScoped<GetAllUserUC>();
            services.AddScoped<GetByIdUserUC>();
            services.AddScoped<UpdateUserUC>();
            services.AddScoped<DeleteUserUC>();
            services.AddScoped<AuthenticationUC>();
            services.AddScoped<IUserUseCaseManager, UserUseCaseManager>();

            services.AddScoped<CreateTaskEntityUC>();
            services.AddScoped<GetAllTaskEntityUC>();
            services.AddScoped<ITaskEntityUseCaseManager, TaskEntityUseCaseManager>();

            services.AddScoped<SmsService>();

            return services;
        }
    }
}
