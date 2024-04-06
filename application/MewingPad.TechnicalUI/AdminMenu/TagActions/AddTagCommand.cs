
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.TagActions;

public class AddTagCommand : Command
{
    public override string? Announce()
    {
        return "Добавить";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}

