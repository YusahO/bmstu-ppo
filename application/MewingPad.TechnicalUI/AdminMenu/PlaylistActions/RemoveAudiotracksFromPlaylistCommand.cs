using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.PlaylistActions;

public class RemoveAudiotracksFromPlaylistCommand : Command
{
    public override string? Announce()
    {
        return "Удалить аудиотрек(и) из плейлиста";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}