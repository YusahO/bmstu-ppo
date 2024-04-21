using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.NpgsqlRepositories;
using MewingPad.Services.UserService;
using Serilog;
using Microsoft.EntityFrameworkCore;
using MewingPad.Services.AudiotrackService;
using MewingPad.Utils.AudioManager;

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
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.WithOrigins("http://localhost:3000"));
            });

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddDbContext<MewingPadDbContext>(opt =>
            {
                opt.UseNpgsql(configuration.GetConnectionString("default"));
            });

            builder.Services.AddSingleton<AudioManager>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAudiotrackRepository, AudiotrackRepository>();
            builder.Services.AddScoped<IPlaylistAudiotrackRepository, PlaylistAudiotrackRepository>();
            builder.Services.AddScoped<ITagAudiotrackRepository, TagAudiotrackRepository>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAudiotrackService, AudiotrackService>();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyOrigin());
            app.UseSwagger();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.MapControllers();

            app.MapGet("/", () => "Hello World!");

            app.UseSwaggerUI();
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