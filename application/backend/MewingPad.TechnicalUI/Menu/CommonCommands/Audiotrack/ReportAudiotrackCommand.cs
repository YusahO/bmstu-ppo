using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.CommonCommands.AudiotrackCommands;

public class ReportAudiotrackCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<ReportAudiotrackCommand>();
    public override string? Description()
    {
        return "Пожаловаться";
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

        Console.Write("Введите причину жалобы: ");
        var reportText = Console.ReadLine();
        _logger.Information($"User input report text \"{reportText}\"");
        if (reportText is null)
        {
            _logger.Error("User input is invalid");
            Console.WriteLine("[!] Текст должен быть непустым");
        }
        else
        {
            var report = new Report(Guid.NewGuid(), context.CurrentUser!.Id, audiotracks[choice - 1].Id, reportText!);
            await context.ReportService.CreateReport(report);
            Console.WriteLine("Жалоба отправлена");
        }
    }
}

