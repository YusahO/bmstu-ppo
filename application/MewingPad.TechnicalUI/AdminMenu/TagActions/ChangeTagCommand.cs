
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.TagActions;

public class ChangeTagCommand : Command
{
    public override string? Announce()
    {
        return "Изменить";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}

