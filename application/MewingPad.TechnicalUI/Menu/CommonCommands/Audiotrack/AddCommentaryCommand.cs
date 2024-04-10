using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.CommonCommands.AudiotrackCommands;

public class AddCommentaryCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<AddCommentaryCommand>();
    public override string? Description()
    {
        return "Написать комментарий";
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

        Console.Write("Введите содержимое комментария: ");
        var commentaryText = Console.ReadLine();
        _logger.Information($"User input commentary text \"{commentaryText}\"");
        if (commentaryText is null)
        {
            _logger.Error("User input is invalid");
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

