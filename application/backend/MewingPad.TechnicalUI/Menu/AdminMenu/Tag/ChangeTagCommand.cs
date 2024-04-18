using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.AdminMenu.TagCommands;

public class ChangeTagCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<ChangeTagCommand>();
    public override string? Description()
    {
        return "Изменить";
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
        _logger.Information($"User input option \"{choice}\"");

        if (!inpCheck)
        {
            _logger.Error("User input is invalid");
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > tags.Count)
        {
            _logger.Error($"User input is out of range [1, {tags.Count}]");
            Console.WriteLine($"[!] Тега с номером {choice} не существует");
            return;
        }

        Console.Write("Введите название тега: ");
        var name = Console.ReadLine();
        if (name is null)
        {
            Console.WriteLine("[!] Название тега должно быть непустым");
            return;
        }

        var tagId = tags[choice - 1].Id;
        await context.TagService.UpdateTagName(tagId, name);
        Console.WriteLine("Тег обновлен");
    }
}

