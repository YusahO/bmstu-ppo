using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.PlaylistActions;

public class ViewUserPlaylistsCommand : Command
{
    public override string? Announce()
    {
        return "Просмотреть свои плейлисты";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}