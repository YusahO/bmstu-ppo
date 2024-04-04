using MewingPad.TechnicalUI.BaseMenu;

using MewingPad.TechnicalUI.AdminMenu.PlaylistActions;

namespace MewingPad.TechnicalUI.AdminMenu;

public class AdminMenuBuilder : MenuBuilder
{
    public override Menu BuildMenu(Context context)
    {
        Menu menu = new(context);
        menu.AddLabel(new("Действия с плейлистами",
        [
            new ViewUserPlaylistsCommand(),
            new CreatePlaylistCommand(),
            new RenamePlaylistCommand(),
            new DeletePlaylistCommand(),
            new ViewUserPlaylistAudiotracksCommand(),
            new AddAudiotrackToPlaylistCommand(),
            new RemoveAudiotracksFromPlaylistCommand()
        ]));
        return menu;
    }
}