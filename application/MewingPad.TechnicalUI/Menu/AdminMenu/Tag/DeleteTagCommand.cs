using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.AdminMenu.TagCommands;

public class DeleteTagCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<DeleteTagCommand>();
    public override string? Description()
    {
        return "Удалить";
    }

    public override async Task Execute(Context context)
    {
        await new ViewUserTagsCommand().Execute(context);
        var tags = (List<Tag>)context.UserObject!;
        if (tags.Count == 0)
        {
            return;
        }

        Console.Write("Введите номер тега: ");

        var inpCheck = int.TryParse(Console.ReadLine(), out int choice);
        _logger.Information($"User input tag number \"{choice}\"");

        if (!inpCheck)
        {
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > tags.Count)
        {
            _logger.Error($"User input is out of range [1, {tags.Count}]");
            Console.WriteLine($"[!] Тега с номером {choice} не существует");
            return;
        }

        await context.TagService.DeleteTag(tags[choice - 1].Id);
        Console.WriteLine("Тег удален");
    }
}

