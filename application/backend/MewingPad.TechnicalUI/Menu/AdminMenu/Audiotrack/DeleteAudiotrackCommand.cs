using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using MewingPad.TechnicalUI.CommonCommands.AudiotrackCommands;
using Serilog;

namespace MewingPad.TechnicalUI.AdminMenu.AudiotrackCommands;

public class DeleteAudiotrackCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<DeleteAudiotrackCommand>();
    public override string? Description()
    {
        return "Удалить";
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

        var audio = audiotracks[choice - 1];
        _logger.Information("User chose audiotrack {@Audio}", audio);

        try
        {
            await context.AudiotrackService.DeleteAudiotrack(audio.Id);
            Console.WriteLine("Аудиотрек удален");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[!] {ex.Message}\n");
        }
    }
}

