using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.PlaylistActions;

public class CreatePlaylistCommand : Command
{
    public override string? Announce()
    {
        return "Создать";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}