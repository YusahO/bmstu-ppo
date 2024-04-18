
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.ReportCommands;

public class ViewAllReportsCommand : Command
{
    public override string? Description()
    {
        return "Просмотреть все жалобы";
    }

    public override async Task Execute(Context context)
    {
        var reports = await context.ReportService.GetAllReports();
        if (reports.Count == 0)
        {
            Console.WriteLine("Список жалоб пуст");
            return;
        }
        else
        {
            int iitem = 0;
            foreach (var r in reports)
            {
                var user = await context.UserService.GetUserById(r.AuthorId);
                var audio = await context.AudiotrackService.GetAudiotrackById(r.AudiotrackId);
                Console.WriteLine($"{++iitem}. Жалоба пользователя {user.Username} на аудиотрек \"{audio.Title}\"");
                Console.WriteLine($"   Причина: \"{r.Text}\"");
                Console.WriteLine($"   Статус: \"{r.Status}\"");
            }
        }
        context.UserObject = reports;
    }
}

