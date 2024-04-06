
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.AudiotrackActions;

public class EditCommentaryCommand : Command
{
    public override string? Announce()
    {
        return "Изменить комментарий";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}

