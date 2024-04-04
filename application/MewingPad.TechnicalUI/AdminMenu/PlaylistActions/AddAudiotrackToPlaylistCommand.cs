using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.PlaylistActions;

public class AddAudiotrackToPlaylistCommand : Command
{
    public override string? Announce()
    {
        return "Добавить аудиотрек в плейлист";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}