using MewingPad.TechnicalUI.BaseMenu;
using MewingPad.TechnicalUI.AdminMenu.AudiotrackCommands;
using MewingPad.TechnicalUI.AdminMenu.ReportCommands;
using MewingPad.TechnicalUI.AdminMenu.TagCommands;
using MewingPad.TechnicalUI.CommonCommands.AudiotrackCommands;
using MewingPad.TechnicalUI.CommonCommands.Search;
using MewingPad.TechnicalUI.CommonCommands.PlaylistCommands;
using MewingPad.TechnicalUI.GuestMenu.AuthActions;

namespace MewingPad.TechnicalUI.AdminMenu;

public class AdminMenuBuilder : MenuBuilder
{
    public override Menu BuildMenu()
    {
        Menu menu = new();
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
        menu.AddLabel(new("Выйти из аккаунта",
        [
            new SignOutUserCommand()
        ]));
        return menu;
    }
}