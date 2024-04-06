
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.AudiotrackActions;

public class DeleteCommentaryCommand : Command
{
    public override string? Announce()
    {
        return "Удалить комментарий";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}

