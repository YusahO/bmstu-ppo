
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.TagActions;

public class DeleteTagCommand : Command
{
    public override string? Announce()
    {
        return "Удалить";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}

