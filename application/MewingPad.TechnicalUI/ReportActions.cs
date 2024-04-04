using MewingPad.Common.Entities;
using MewingPad.Common.Enums;
using MewingPad.Services.AudiotrackService;
using MewingPad.Services.ReportService;
using MewingPad.Services.UserService;

namespace MewingPad.TechnicalUI.Actions;

internal class ReportActions(AudiotrackService audiotrackService,
                             UserService userService,
                             ReportService reportService)
{
    private readonly AudiotrackService _audiotrackService = audiotrackService;
    private readonly UserService _userService = userService;
    private readonly ReportService _reportService = reportService;

    public async Task RunMenu()
    {
        Console.WriteLine("\n========== Действия с жалобами ==========");
        Console.WriteLine("1. Просмотреть все жалобы");
        Console.WriteLine("2. Изменить статус жалобы");
        Console.Write("Ввод: ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine();
            switch (choice)
            {
                case 1:
                    await ViewAllReports();
                    break;
                case 2:
                    await ChangeReportStatus();
                    break;
                default:
                    Console.WriteLine($"[!] Нет пункта с номером {choice}");
                    break;
            }
        }
    }

    private async Task<List<Report>> ViewAllReports()
    {
        var reports = await _reportService.GetAllReports();
        if (reports.Count == 0)
        {
            Console.WriteLine("Список жалоб пуст");
        }
        else
        {
            int iitem = 0;
            foreach (var r in reports)
            {
                var user = await _userService.GetUserById(r.AuthorId);
                var audio = await _audiotrackService.GetAudiotrackById(r.AudiotrackId);
                Console.WriteLine($"{++iitem}. Жалоба пользователя {user.Username} на аудиотрек \"{audio.Title}\"");
                Console.WriteLine($"   Причина: \"{r.Text}\"");
                Console.WriteLine($"   Статус: \"{r.Status}\"");
            }
        }
        return reports;
    }

    private async Task ChangeReportStatus()
    {
        var reports = await ViewAllReports();
        if (reports.Count == 0)
        {
            return;
        }
        Console.Write("Введите номер жалобы: ");
        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > reports.Count)
        {
            Console.WriteLine($"[!] Жалобы с номером {choice} не существует");
            return;
        }

        var report = reports[choice - 1];
        string? selection;
        ReportStatus newStatus;

        Console.Write("Отметить непрочитанным? [y/n] ");
        selection = Console.ReadLine();
        if (selection == "y")
        {
            newStatus = ReportStatus.NotViewed;
        }
        else
        {
            Console.Write("Принять жалобу? [y/n/[empty]] ");
            selection = Console.ReadLine();
            if (selection == "y")
            {
                newStatus = ReportStatus.Accepted;
            }
            else if (selection == "n")
            {
                newStatus = ReportStatus.Declined;
            }
            else if (selection == "")
            {
                newStatus = ReportStatus.Viewed;
            }
            else
            {
                Console.WriteLine("[!] Введенно некорректное значение");
                return;
            }
        }

        await _reportService.UpdateReportStatus(report.Id, newStatus);
        Console.WriteLine("Статус жалобы изменен");
    }
}