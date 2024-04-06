
using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.TagCommands;

public class ChangeTagCommand : Command
{
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
        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > tags.Count)
        {
            Console.WriteLine($"[!] Аудиотрека с номером {choice} не существует");
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

