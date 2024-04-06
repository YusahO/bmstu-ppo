
using MewingPad.Common.Entities;
using MewingPad.Common.Enums;
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.ReportCommands;

public class ChangeReportStatusCommand : Command
{
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
            Console.WriteLine("Список жалоб пуст");
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

        await context.ReportService.UpdateReportStatus(report.Id, newStatus);
        Console.WriteLine("Статус жалобы изменен");
    }
}

