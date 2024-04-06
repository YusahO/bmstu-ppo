
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.Search;

public class SearchByTitleCommand : Command
{
    public override string? Announce()
    {
        return "По названию";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}

