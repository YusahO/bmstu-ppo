using MewingPad.TechnicalUI.BaseMenu;

using MewingPad.TechnicalUI.AdminMenu.PlaylistActions;
using MewingPad.TechnicalUI.AdminMenu.AudiotrackActions;
using MewingPad.TechnicalUI.AdminMenu.ReportActions;
using MewingPad.TechnicalUI.AdminMenu.TagActions;
using MewingPad.TechnicalUI.AdminMenu.Search;

namespace MewingPad.TechnicalUI.AdminMenu;

public class AdminMenuBuilder : MenuBuilder
{
    public override Menu BuildMenu(Context context)
    {
        Menu menu = new(context);
        menu.AddLabel(new("Поиск",
        [
            new SearchByTitleCommand(),
            new SearchByTagsCommand(),
        ]));
        menu.AddLabel(new("Действия с аудиотреками",
        [
            new ViewAllAudiotracksCommand(),
            new DownloadAudiotrackCommand(),
            new AddScoreCommand(),
            new ViewAudiotrackCommentariesCommand(),
            new AddCommentaryCommand(),
            new EditCommentaryCommand(),
            new DeleteCommentaryCommand(),
            new ReportAudiotrackCommand(),
            new UploadAudiotrackCommand(),
            new ChangeAudiotrackCommand(),
            new DeleteAudiotrackCommand()
        ]));
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
        menu.AddLabel(new("Действия с жалобами",
        [
            new ViewAllReportsCommand(),
            new ChangeReportStatusCommand()
        ]));
        menu.AddLabel(new("Действия с тегами",
        [
            new ViewAllTagsCommand(),
            new AddTagCommand(),
            new ChangeTagCommand(),
            new DeleteTagCommand()
        ]));
        return menu;
    }
}