using Backend.Database;
using Carter;
using DotNetEnv;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MentorHub API",
        Version = "v1",
        Description = "API for MentorHub application"
    });

    c.CustomSchemaIds(type => type.FullName);
    c.UseAllOfToExtendReferenceSchemas();


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
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
            Array.Empty<string>()
        }
    });
});
Env.Load();
builder.Services.AddCarter();

builder.Services.AddMediatR(cfg => {
        cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    });

var connectionString = $"Host={Env.GetString("DB_HOST")};" +
                       $"Database={Env.GetString("DB_NAME")};" +
                       $"Username={Env.GetString("DB_USER")};" +
                       $"Password={Env.GetString("DB_PASSWORD")}";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddHttpContextAccessor();





builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7035",
            ValidAudience = "https://localhost:7035",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Env.GetString("JWT_SECRET")))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Dozvoli Angular frontend
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();


app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapCarter();

app.Run();
