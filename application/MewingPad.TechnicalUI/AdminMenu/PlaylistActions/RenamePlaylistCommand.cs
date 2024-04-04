using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.PlaylistActions;

public class RenamePlaylistCommand : Command
{
    public override string? Announce()
    {
        return "Переименовать";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}