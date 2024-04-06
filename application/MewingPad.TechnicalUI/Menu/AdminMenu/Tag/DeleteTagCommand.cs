
using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.TagCommands;

public class DeleteTagCommand : Command
{
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

        await context.TagService.DeleteTag(tags[choice - 1].Id);
        Console.WriteLine("Тег удален");
    }
}

