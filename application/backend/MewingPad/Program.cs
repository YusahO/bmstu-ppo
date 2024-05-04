using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.NpgsqlRepositories;
using MewingPad.Services.UserService;
using Serilog;
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
using Microsoft.OpenApi.Models;
using MewingPad.Database.Models;
using Microsoft.AspNetCore.Identity;

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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddDbContext<MewingPadDbContext>(opt =>
            {
                opt.UseNpgsql(configuration.GetConnectionString("default"));
            });

            builder.Services.AddDefaultIdentity<UserDbModel>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<MewingPadDbContext>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                var secret = configuration["Jwt:Secret"]!;
                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidAudience = "http://localhost:3000",
                    ValidIssuer = "http://localhost:9898",
                    IssuerSigningKey = key,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddSingleton<AudioManager>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAudiotrackRepository, AudiotrackRepository>();
            builder.Services.AddScoped<IPlaylistAudiotrackRepository, PlaylistAudiotrackRepository>();
            builder.Services.AddScoped<ITagAudiotrackRepository, TagAudiotrackRepository>();
            builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<IScoreRepository, ScoreRepository>();
            builder.Services.AddScoped<ICommentaryRepository, CommentaryRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();

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
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowSpecificOrigin");
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            using (var db = scope.ServiceProvider.GetRequiredService<MewingPadDbContext>())
            {
                db.Database.Migrate();
            }

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