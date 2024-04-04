using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.PlaylistActions;

public class ViewUserPlaylistAudiotracksCommand : Command
{
    public override string? Announce()
    {
        return "Просмотреть аудиотреки в плейлисте";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}