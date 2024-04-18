using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.CommonCommands.AudiotrackCommands;

public class ViewAudiotrackCommentariesCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<ViewAudiotrackCommentariesCommand>();
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

        var comms = await context.CommentaryService.GetAudiotrackCommentaries(audiotracks[choice - 1].Id);
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

