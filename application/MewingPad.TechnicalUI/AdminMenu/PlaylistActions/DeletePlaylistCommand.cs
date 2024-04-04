using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.PlaylistActions;

public class DeletePlaylistCommand : Command
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