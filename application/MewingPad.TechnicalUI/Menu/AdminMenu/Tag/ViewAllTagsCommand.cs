using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.TagCommands;

public class ViewAllTagsCommand : Command
{
    public override string? Description()
    {
        return "Просмотреть все теги";
    }

    public override async Task Execute(Context context)
    {
        var tags = await context.TagService.GetAllTags();
        if (tags.Count == 0)
        {
            Console.WriteLine("Список тегов пуст");
        }
        else
        {
            for (int i = 0; i < tags.Count; ++i)
            {
                var u = await context.UserService.GetUserById(tags[i].AuthorId);
                Console.WriteLine($"{i + 1}. {tags[i].Name}");
                Console.WriteLine($"   Автор: {u.Username}");
            }
        }
        context.UserObject = tags;
    }
}

