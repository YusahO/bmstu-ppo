using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MewingPad.Database.Context;
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
using MewingPad.TechnicalUI.BaseMenu;
using MewingPad.TechnicalUI.GuestMenu;
using MewingPad.TechnicalUI.AdminMenu;
using Serilog;
using MewingPad.Utils.AudioManager;

namespace MewingPad.TechnicalUI;

internal static class Program
{
    [STAThread]
    static async Task Main()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();

        var builder = new HostBuilder().ConfigureServices((hostContext, services) =>
        {
            services.AddDbContext<MewingPadDbContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("default"));
            });

            var menus = new List<Menu>
            {
                new GuestMenuBuilder().BuildMenu(),
                new AuthorizedMenuBuilder().BuildMenu(),
                new AdminMenuBuilder().BuildMenu()
            };

            services.AddSingleton(config);
            services.AddSingleton(menus);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAudiotrackRepository, AudiotrackRepository>();
            services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IScoreRepository, ScoreRepository>();
            services.AddScoped<ICommentaryRepository, CommentaryRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IPlaylistAudiotrackRepository, PlaylistAudiotrackRepository>();
            services.AddScoped<ITagAudiotrackRepository, TagAudiotrackRepository>();

            services.AddScoped<Context>();
            services.AddScoped<AudioManager>();

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