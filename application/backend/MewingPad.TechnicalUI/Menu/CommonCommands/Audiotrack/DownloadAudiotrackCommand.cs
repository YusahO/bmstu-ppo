using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.CommonCommands.AudiotrackCommands;

public class DownloadAudiotrackCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<DownloadAudiotrackCommand>();
    public override string? Description()
    {
        return "Скачать";
    }

    public override async Task Execute(Context context)
    {
        await new ViewAllAudiotracksCommand().Execute(context);
        var audiotracks = (List<Audiotrack>)context.UserObject!;
        if (audiotracks.Count == 0)
        {
            return;
        }

        Console.Write("Введите номер аудиотрека: ");

        var inpCheck = int.TryParse(Console.ReadLine(), out int choice);
        _logger.Information($"User input audiotrack number \"{choice}\"");

        if (!inpCheck)
        {
            _logger.Error("User input is invalid");
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > audiotracks.Count)
        {
            _logger.Error($"User input is out of range [1, {audiotracks.Count}]");
            Console.WriteLine($"[!] Аудиотрека с номером {choice} не существует");
            return;
        }

        try
        {
            await context.AudiotrackService.DownloadAudiotrack(audiotracks[choice - 1].Filepath, "/home/daria/Загрузки");
            Console.WriteLine($"Аудиотрек сохранен в директории \"/home/daria/Загрузки\"");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[!] {ex.Message}\n");
        }
    }
}

