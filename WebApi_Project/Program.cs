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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;

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


// Not neccesary if you wrote you own JwtValidator and Middleware.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(configureOptions =>
{
    configureOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration.GetSection("JwtSetting:Issuer").Value?.ToString(),
        ValidAudience = builder.Configuration.GetSection("JwtSetting:Audience").Value?.ToString(),
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSetting:SecretKey").Value)),
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero,
    };
    // You can access token whenever you need it :
    configureOptions.SaveToken = true; //HttpContext.GetTokenAsync();
    configureOptions.Events = new JwtBearerEvents
    {
        OnChallenge = context => { return Task.CompletedTask; },
        OnForbidden = context => { return Task.FromResult(false); },
        OnAuthenticationFailed = context => 
        {
            var refreshToken = context.HttpContext.RequestServices.GetRequiredService<JwtRefreshToken>();
            return refreshToken.RefreshToken(context);
        },
        OnMessageReceived = context => { return Task.CompletedTask; },
        // Run after token validated:
        OnTokenValidated = context =>
        {
            var tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<JwtValidator>();
            return tokenValidatorService.Execute(context);
        }
    };
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    //Includes comments in swagger.
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "WebApi_Project.xml"), true);
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WebApi_Project",
        Version = "v1",
        Description = "Swagger for v1 of api."
    });
    //c.SwaggerDoc("v2", new OpenApiInfo
    //{
    //    Title = "WebApi_Project",
    //    Version = "v2",
    //    Description = "Swagger for v2 of api."
    //});

    // To create swagger for different versions of api
    //c.DocInclusionPredicate((doc, apiDescrition) =>
    //{
    //    if (!apiDescrition.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

    //    var version = methodInfo.DeclaringType?
    //        .GetCustomAttributes<ApiVersionAttribute>(false)
    //        .SelectMany(attr => attr.Versions);


    //    return version.Any(v => $"v{v}" == doc);
    //});


    // To authorize and set Jwt in swagger:
    var security = new OpenApiSecurityScheme
    {
        Name = "Jwt Auth",
        Description = "Enter your access token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(security.Reference.Id, security);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {security , Array.Empty<string>()}
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
        //options.SwaggerEndpoint("/swagger/v2/swagger.json", "WebApi_Project_v2");
    });
}


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

//app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();