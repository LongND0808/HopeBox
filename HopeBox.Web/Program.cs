using HopeBox.Core.Config;
using HopeBox.Core.IAspModelService;
using HopeBox.Core.IdentityModelService;
using HopeBox.Core.IService;
using HopeBox.Core.Service;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Models;
using HopeBox.Infrastructure.DataContext;
using HopeBox.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<HopeBoxDataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HopeBox")));

builder.Services.AddSingleton<IConfig, Config>();
builder.Services.AddScoped<IHopeBoxDataContext, HopeBoxDataContext>();

builder.Services.AddScoped(typeof(IConverter<,>), typeof(HopeBox.Domain.Converter.Converter<,>));
builder.Services.AddScoped(typeof(IBaseService<,>), typeof(BaseService<,>));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICauseService, CauseService>();

#region Add Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
#endregion

#region Add Services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IVolunteerService, VolunteerService>();
#endregion

#region Add Converter
builder.Services.AddScoped(typeof(IConverter<,>), typeof(HopeBox.Domain.Converter.Converter<,>));
#endregion


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<HopeBoxDataContext>()
    .AddDefaultTokenProviders();

var config = new Config(builder.Configuration);

builder.Services.AddIdentityServer()
    .AddInMemoryIdentityResources(config.GetIdentityResources())
    .AddInMemoryApiScopes(config.GetApiScopes())
    .AddInMemoryClients(config.GetClients())
    .AddDeveloperSigningCredential()
    .AddAspNetIdentity<User>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,

        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? ""))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
