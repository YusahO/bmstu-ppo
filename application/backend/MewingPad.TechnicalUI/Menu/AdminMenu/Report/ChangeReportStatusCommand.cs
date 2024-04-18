using MewingPad.Common.Entities;
using MewingPad.Common.Enums;
using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.AdminMenu.ReportCommands;

public class ChangeReportStatusCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<ChangeReportStatusCommand>();
    public override string? Description()
    {
        return "Изменить статус жалобы";
    }

    public override async Task Execute(Context context)
    {
        await new ViewAllReportsCommand().Execute(context);
        var reports = (List<Report>)context.UserObject!;
        if (reports.Count == 0)
        {
            return;
        }

        Console.Write("Введите номер жалобы: ");

        var inpCheck = int.TryParse(Console.ReadLine(), out int choice);
        _logger.Information($"User input report number \"{choice}\"");

        if (!inpCheck)
        {
            _logger.Error("User input is invalid");
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > reports.Count)
        {
            _logger.Error($"User input is out of range [1, {reports.Count}]");
            Console.WriteLine($"[!] Жалобы с номером {choice} не существует");
            return;
        }

        var report = reports[choice - 1];
        string? selection;
        ReportStatus newStatus;

        Console.Write("Отметить непрочитанным? [y/n] ");

        selection = Console.ReadLine();
        _logger.Information($"User input mark unread \"{selection}\"");

        if (selection == "y")
        {
            newStatus = ReportStatus.NotViewed;
        }
        else
        {
            Console.Write("Принять жалобу? [y/n/[empty]] ");

            selection = Console.ReadLine();
            _logger.Information($"User input accept report \"{selection}\"");

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
                _logger.Error("User input is invalid");
                Console.WriteLine("[!] Введенно некорректное значение");
                return;
            }
        }

        await context.ReportService.UpdateReportStatus(report.Id, newStatus);
        Console.WriteLine("Статус жалобы изменен");
    }
}

