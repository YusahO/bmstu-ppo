
using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.CommonCommands.AudiotrackCommands;

public class EditCommentaryCommand : Command
{
    public override string? Description()
    {
        return "Изменить комментарий";
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

        var comms = (await context.CommentaryService.GetAudiotrackCommentaries(audiotracks[choice - 1].Id))
            .FindAll(c => c.AuthorId == context.CurrentUser!.Id);
        if (comms.Count == 0)
        {
            Console.WriteLine("Список комментариев пуст");
            return;
        }
        for (int i = 0; i < comms.Count; ++i)
        {
            var username = (await context.UserService.GetUserById(comms[i].AuthorId)).Username;
            Console.WriteLine($"{i}. {username}");
            Console.WriteLine($"   {comms[i].Text}");
        }

        Console.Write("Введите номер комментария: ");
        if (!int.TryParse(Console.ReadLine(), out choice))
        {
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > comms.Count)
        {
            Console.WriteLine($"[!] Комментария с номером {choice} не существует");
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
            comms[choice - 1].Text = commentaryText;
            await context.CommentaryService.UpdateCommentary(comms[choice - 1]);
            Console.WriteLine("Комментарий обновлен");
        }
    }
}

