using MewingPad.Common.IRepositories;
using MewingPad.Database.PgSQL.Context;
using MewingPad.Database.PgSQL.Repositories;
using MewingPad.Services.UserService;
using Microsoft.EntityFrameworkCore;
using MewingPad.Services.AudiotrackService;
using MewingPad.Utils.AudioManager;
using MewingPad.Services.ScoreService;
using MewingPad.Services.TagService;
using MewingPad.Services.OAuthService;
using MewingPad.Services.PlaylistService;
using MewingPad.Services.CommentaryService;
using MewingPad.Services.ReportService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using MongoDB.Driver;
using MewingPad.Database.MongoDB.Context;

internal class Program
{
    private static void Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        try
        {
            Log.Information("Starting web application");

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSerilog();
            builder.Configuration.AddConfiguration(configuration);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MewingPad.Api", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                                Enter 'Bearer' [space] and then your token in the text input below.
                                \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services.AddCors();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]!))
                    };
                    options.Events = new()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["token"];
                            if (context.Token is not null)
                            {
                                var parsedToken = new JwtSecurityToken(context.Token);
                                context.HttpContext.Items["userId"] = parsedToken.Claims.ElementAt(0).Value;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            builder.Services.AddAuthorization();

            var db = configuration["ApiSettings:Database"]!;
            if (db is "PostgreSQL")
            {
                builder.Services.AddDbContext<MewingPadPgSQLDbContext>(opt =>
                {
                    opt.UseNpgsql(configuration.GetSection("Postgresql").GetConnectionString("default"));
                });

                builder.Services.AddScoped<IPlaylistAudiotrackRepository, MewingPad.Database.PgSQL.Repositories.PlaylistAudiotrackRepository>();
                builder.Services.AddScoped<ITagAudiotrackRepository, MewingPad.Database.PgSQL.Repositories.TagAudiotrackRepository>();

                builder.Services.AddScoped<IUserRepository, MewingPad.Database.PgSQL.Repositories.UserRepository>();
                builder.Services.AddScoped<IAudiotrackRepository, MewingPad.Database.PgSQL.Repositories.AudiotrackRepository>();
                builder.Services.AddScoped<IPlaylistRepository, MewingPad.Database.PgSQL.Repositories.PlaylistRepository>();
                builder.Services.AddScoped<ITagRepository, MewingPad.Database.PgSQL.Repositories.TagRepository>();
                builder.Services.AddScoped<IScoreRepository, MewingPad.Database.PgSQL.Repositories.ScoreRepository>();
                builder.Services.AddScoped<ICommentaryRepository, MewingPad.Database.PgSQL.Repositories.CommentaryRepository>();
                builder.Services.AddScoped<IReportRepository, MewingPad.Database.PgSQL.Repositories.ReportRepository>();
            }
            else if (db is "MongoDB")
            {
                var client = new MongoClient(builder.Configuration.GetSection("MongoDB").GetConnectionString("default"));
                builder.Services.AddDbContext<MewingPadMongoDbContext>(opt =>
                {
                    opt.UseMongoDB(client, "MewingPadDB");
                });

                builder.Services.AddScoped<IPlaylistAudiotrackRepository, MewingPad.Database.MongoDB.Repositories.PlaylistAudiotrackRepository>();
                builder.Services.AddScoped<ITagAudiotrackRepository, MewingPad.Database.MongoDB.Repositories.TagAudiotrackRepository>();

                builder.Services.AddScoped<IUserRepository, MewingPad.Database.MongoDB.Repositories.UserRepository>();
                builder.Services.AddScoped<IAudiotrackRepository, MewingPad.Database.MongoDB.Repositories.AudiotrackRepository>();
                builder.Services.AddScoped<IPlaylistRepository, MewingPad.Database.MongoDB.Repositories.PlaylistRepository>();
                builder.Services.AddScoped<ITagRepository, MewingPad.Database.MongoDB.Repositories.TagRepository>();
                builder.Services.AddScoped<IScoreRepository, MewingPad.Database.MongoDB.Repositories.ScoreRepository>();
                builder.Services.AddScoped<ICommentaryRepository, MewingPad.Database.MongoDB.Repositories.CommentaryRepository>();
                builder.Services.AddScoped<IReportRepository, MewingPad.Database.MongoDB.Repositories.ReportRepository>();
            }

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddSingleton<AudioManager>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IOAuthService, OAuthService>();
            builder.Services.AddScoped<IAudiotrackService, AudiotrackService>();
            builder.Services.AddScoped<IPlaylistService, PlaylistService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<IScoreService, ScoreService>();
            builder.Services.AddScoped<ICommentaryService, CommentaryService>();
            builder.Services.AddScoped<IReportService, ReportService>();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(b => b.WithOrigins("http://localhost:3000")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials()
                              .Build());

            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}

namespace MewingPad
{
    public class Program { }
}