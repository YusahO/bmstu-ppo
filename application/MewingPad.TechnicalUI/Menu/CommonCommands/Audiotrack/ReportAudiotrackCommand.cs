
using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.CommonCommands.AudiotrackCommands;

public class ReportAudiotrackCommand : Command
{
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
        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > audiotracks.Count)
        {
            Console.WriteLine($"[!] Аудиотрека с номером {choice} не существует");
            return;
        }

        Console.Write("Введите причину жалобы: ");
        var reportText = Console.ReadLine();
        if (reportText is null)
        {
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

