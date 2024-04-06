
using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.CommonCommands.AudiotrackCommands;

public class ViewAudiotrackCommentariesCommand : Command
{
    public override string? Description()
    {
        return "Просмотреть комментарии";
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

        var comms = await context.CommentaryService.GetAudiofileCommentaries(audiotracks[choice - 1].Id);
        if (comms.Count == 0)
        {
            Console.WriteLine("Список комментариев пуст");
            return;
        }
        foreach (var c in comms)
        {
            var username = (await context.UserService.GetUserById(c.AuthorId)).Username;
            Console.WriteLine($"-> {username}");
            Console.WriteLine($"   {c.Text}");
        }
    }
}

