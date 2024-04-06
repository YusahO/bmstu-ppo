
using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.AudiotrackActions;

public class AddCommentaryCommand : Command
{
    public override string? Announce()
    {
        return "Написать комментарий";
    }

    public override async Task Execute(Context context)
    {
        await new ViewAllAudiotracksCommand().Execute(context);
        var audiotracks = (List<Audiotrack>)context.UserObject!;
        Console.Write("Введите номер аудиотрека: ");
        if (audiotracks.Count == 0)
        {
            return;
        }
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

        Console.Write("Введите содержимое комментария: ");
        var commentaryText = Console.ReadLine();
        if (commentaryText is null)
        {
            Console.WriteLine("[!] Текст комментария должен быть непустым");
        }
        else
        {
            var commentary = new Commentary(Guid.NewGuid(), context.CurrentUser!.Id, audiotracks[choice - 1].Id, commentaryText!);
            await context.CommentaryService.CreateCommentary(commentary);
            Console.WriteLine("Комментарий создан");
        }
    }
}

