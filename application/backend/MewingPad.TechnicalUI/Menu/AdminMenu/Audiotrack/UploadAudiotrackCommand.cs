using System.Globalization;
using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.AdminMenu.AudiotrackCommands;

public class UploadAudiotrackCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<UploadAudiotrackCommand>();
    private readonly NumberFormatInfo _nfi = new()
    {
        NumberDecimalSeparator = ","
    };

    public override string? Description()
    {
        return "Добавить новый";
    }

    public override async Task Execute(Context context)
    {
        string? title, filepath;

        Console.Write("Введите название аудиотрека: ");

        title = Console.ReadLine();
        _logger.Information($"User input title \"{title}\"");

        if (title is null)
        {
            _logger.Error("User input is invalid");
            Console.WriteLine("[!] Введено некорректное значение");
            return;
        }

        Console.Write("Введите длительность: ");

        var inpCheck = float.TryParse(Console.ReadLine(), NumberStyles.Any, _nfi, out float duration);
        _logger.Information($"User input duration \"{duration}\"");

        if (!inpCheck)
        {
            _logger.Error("User input is invalid");
            Console.WriteLine("[!] Введено некорректное значение");
            return;
        }

        Console.Write("Введите путь к файлу аудиотрека: ");
        
        filepath = Console.ReadLine();
        _logger.Information($"User input filepath \"{filepath}\"");

        if (filepath is null)
        {
            _logger.Error("User input is invalid");
            Console.WriteLine("[!] Введено некорректное значение");
            return;
        }

        var audio = new Audiotrack(
            Guid.NewGuid(),
            title!,
            (float)duration!,
            context.CurrentUser!.Id,
            filepath!);

        try
        {
            await context.AudiotrackService.CreateAudiotrack(audio);
            Console.WriteLine("Аудиотрек загружен");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[!] {ex.Message}\n");
        }
    }
}

