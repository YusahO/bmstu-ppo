using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.TagCommands;

public class ViewUserTagsCommand : Command
{
    public override string? Description()
    {
        return "Просмотреть все теги пользователя";
    }

    public override async Task Execute(Context context)
    {
        var tags = await context.TagService.GetAllTags();
        tags = (from t in tags
                where t.AuthorId == context.CurrentUser!.Id
                select t).ToList();
        if (tags.Count == 0)
        {
            Console.WriteLine("Список тегов пуст");
        }
        else
        {
            for (int i = 0; i < tags.Count; ++i)
            {
                Console.WriteLine($"{i + 1}. {tags[i].Name}");
            }
        }
        context.UserObject = tags;
    }
}

