
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.Search;

public class SearchByTagsCommand : Command
{
    public override string? Announce()
    {
        return "По тегу";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}

