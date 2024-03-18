using Application.Services;
using Application.UseCases.Managers;
using Application.UseCases.TaskEntityUseCases;
using Application.UseCases.UserUsecases;
using Asp.Versioning;
using AutoMapper;
using Domain.IRepositories;
using Domain.Repositories;
using Infrustructure.Identity.JWT;
using Infrustructure.Persistance.DbContexts;
using Infrustructure.Persistance.Repositories;
using Infrustructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApiDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("WebApiConn")));

builder.Services.RegisterServices(builder.Configuration);

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WebApi_Project_v1",
        Version = "v1",
        Description = "Swagger for v1 of api."
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "WebApi_Project_v2",
        Version = "v2",
        Description = "Swagger for v2 of api."
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi_Project_v1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "WebApi_Project_v2");
    });
}

app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();