using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.AdminMenu.TagCommands;

public class AddTagCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<AddTagCommand>();
    public override string? Description()
    {
        return "Добавить";
    }

    public override async Task Execute(Context context)
    {
        Console.Write("Введите название тега: ");

        var name = Console.ReadLine();
        _logger.Information($"User input tag name \"{name}\"");

        if (name is null)
        {
            _logger.Error("User input is invalid");
            Console.WriteLine("[!] Название тега должно быть непустым");
            return;
        }

        var tag = new Tag(Guid.NewGuid(), context.CurrentUser!.Id, name);
        await context.TagService.CreateTag(tag);
        Console.WriteLine("Тег создан");
    }
}

