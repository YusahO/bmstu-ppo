using MewingPad.Common.Entities;
using MewingPad.Services.AudiotrackService;
using MewingPad.Services.CommentaryService;
using MewingPad.Services.OAuthService;
using MewingPad.Services.PlaylistService;
using MewingPad.Services.ReportService;
using MewingPad.Services.ScoreService;
using MewingPad.Services.TagService;
using MewingPad.Services.UserService;

namespace MewingPad.TechnicalUI.BaseMenu;

public class Context(OAuthService oauthService,
                      UserService userService,
                      TagService tagService,
                      PlaylistService playlistService,
                      AudiotrackService audiotrackService,
                      ReportService reportService,
                      CommentaryService commentaryService,
                      ScoreService scoreService)
{
    public OAuthService OAuthService { get; } = oauthService;
    public UserService UserService { get; } = userService;
    public TagService TagService { get; } = tagService;
    public PlaylistService PlaylistService { get; } = playlistService;
    public AudiotrackService AudiotrackService { get; } = audiotrackService;
    public ReportService ReportService { get; } = reportService;
    public CommentaryService CommentaryService { get; } = commentaryService;
    public ScoreService ScoreService { get; } = scoreService;

    public object? UserObject { get; set; }
    public User? CurrentUser { get; set; }
}