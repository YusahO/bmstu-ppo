
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.TagActions;

public class ViewAllTagsCommand : Command
{
    public override string? Announce()
    {
        return "Просмотреть все теги";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}

