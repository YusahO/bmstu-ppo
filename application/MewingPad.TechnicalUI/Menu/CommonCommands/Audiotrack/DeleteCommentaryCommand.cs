
using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.CommonCommands.AudiotrackCommands;

public class DeleteCommentaryCommand : Command
{
    public override string? Description()
    {
        return "Удалить комментарий";
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

        var comms = (await context.CommentaryService.GetAudiofileCommentaries(audiotracks[choice - 1].Id))
            .FindAll(c => c.AuthorId == context.CurrentUser!.Id);
        int iitem = 0;
        foreach (var c in comms)
        {
            var username = (await context.UserService.GetUserById(c.AuthorId)).Username;
            Console.WriteLine($"{++iitem}. {username}");
            Console.WriteLine($"   {c.Text}");
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

        await context.CommentaryService.DeleteCommentary(comms[choice - 1].Id);
        Console.WriteLine("Комментарий удален");
    }
}

