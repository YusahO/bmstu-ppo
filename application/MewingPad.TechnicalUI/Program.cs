using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MewingPad.Database.Context;
using Microsoft.EntityFrameworkCore;
using MewingPad.Common.IRepositories;
using MewingPad.Database.NpgsqlRepositories;
using MewingPad.Services.UserService;
using MewingPad.Services.OAuthService;
using MewingPad.Services.PlaylistService;
using MewingPad.Services.AudiotrackService;
using MewingPad.Services.TagService;
using MewingPad.Services.ScoreService;
using MewingPad.Services.ReportService;
using MewingPad.Services.CommentaryService;

namespace MewingPad.TechnicalUI;

internal static class Program
{
    [STAThread]
    static async Task Main()
    {
        IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var builder = new HostBuilder().ConfigureServices((hostContext, services) =>
        {
            services.AddDbContext<MewingPadDbContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("default"));
            });

            services.AddSingleton(config);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAudiotrackRepository, AudiotrackRepository>();
            services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IScoreRepository, ScoreRepository>();
            services.AddScoped<ICommentaryRepository, CommentaryRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();

            services.AddScoped<UserService>();
            services.AddScoped<OAuthService>();
            services.AddScoped<PlaylistService>();
            services.AddScoped<AudiotrackService>();
            services.AddScoped<TagService>();
            services.AddScoped<ScoreService>();
            services.AddScoped<ReportService>();
            services.AddScoped<CommentaryService>();

            services.AddSingleton<Startup>();
        });

        var host = builder.Build();

        await using var context = host.Services.GetRequiredService<MewingPadDbContext>();
        await context.Database.MigrateAsync();

        using (var serviceScope = host.Services.CreateAsyncScope())
        {
            var services = serviceScope.ServiceProvider;
            try
            {
                Console.WriteLine("Запуск приложения...");
                var techUiApp = services.GetRequiredService<Startup>();
                await techUiApp.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[!] " + ex.Message);
            }
        }
    }
} 