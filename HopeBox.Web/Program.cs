using Duende.IdentityModel;
using HopeBox.Core.Config;
using HopeBox.Core.Email;
using HopeBox.Core.IAspModelService;
using HopeBox.Core.IdentityModelService;
using HopeBox.Core.IService;
using HopeBox.Core.Service;
using HopeBox.Core.Token;
using HopeBox.Domain.Configuration;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Models;
using HopeBox.Infrastructure.DataContext;
using HopeBox.Infrastructure.Repository;
using HopeBox.Infrastructure.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<HopeBoxDataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("HopeBox")));

builder.Services.AddSingleton<IConfig, Config>();
builder.Services.AddScoped<IHopeBoxDataContext, HopeBoxDataContext>();

builder.Services.AddScoped(typeof(IConverter<,>), typeof(HopeBox.Domain.Converter.Converter<,>));

builder.Services.Configure<OpenMapConfig>(
    builder.Configuration.GetSection("OpenMap"));

builder.Services.AddHttpClient<IOpenMapService, OpenMapService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "HopeBox/1.0");
});

#region Add Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
#endregion

#region Add Services
builder.Services.AddScoped(typeof(IBaseService<,>), typeof(BaseService<,>));
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IVolunteerService, VolunteerService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IDonationService, DonationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICauseService, CauseService>();
builder.Services.AddScoped<IOpenMapService, OpenMapService>();
#endregion

#region Add Converter
builder.Services.AddScoped(typeof(IConverter<,>), typeof(HopeBox.Domain.Converter.Converter<,>));
#endregion

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<HopeBoxDataContext>()
    .AddDefaultTokenProviders();

var config = new Config(builder.Configuration);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? "")),
        RoleClaimType = ClaimTypes.Role
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("accessToken"))
            {
                context.Token = context.Request.Cookies["accessToken"];
            }
            return Task.CompletedTask;
        }
    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMultipleOrigins", policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",
            "https://hopebox.pages.dev")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseCors("AllowMultipleOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HopeBoxDataContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        dbContext.Database.Migrate();
        logger.LogInformation("Database migration applied successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while resetting and migrating the database.");
    }
}


app.Run();
