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

public struct Context(OAuthService oauthService,
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

abstract public class Command
{
    abstract public Task Execute(Context context);
    abstract public string? Announce();
}

// Console.Write($"========= {_announce} =========");

public class MenuLabel(string announce, List<Command> commands)
{
    string _announce = announce;
    List<Command> _commands = commands;

    public void Announce()
    {
        Console.WriteLine(_announce);
        foreach (var c in _commands)
        {
            Console.WriteLine($"|- {c.Announce()}");
        }
    }

    public void Execute(Context context)
    {
        Console.WriteLine($"============ {_announce} ============");
        int iitem = 1;
        foreach (var c in _commands)
        {
            Console.WriteLine($"{iitem++}. {c.Announce()}");
        }
        Console.Write("Ввод: ");
        if (!int.TryParse(Console.ReadLine(), out int no))
        {
            Console.WriteLine("[!] Error");
            return;
        }
        if (0 >= no || no > _commands.Count)
        {
            Console.WriteLine("[!] Error");
            return;
        }
        _commands[no - 1].Execute(context);
    }
}

public class Menu(Context context)
{
    List<MenuLabel> _labels = [];
    Context _context = context;

    public void AddLabel(MenuLabel label)
    {
        _labels.Add(label);
    }

    public void Execute()
    {
        int iitem = 1;
        foreach (var l in _labels)
        {
            Console.Write($"[{iitem++}] ");
            l.Announce();
        }
        Console.Write("Ввод: ");
        if (!int.TryParse(Console.ReadLine(), out int no))
        {
            Console.WriteLine("[!] Error");
            return;
        }

        if (0 >= no || no > _labels.Count)
        {
            Console.WriteLine("[!] Error");
            return;
        }
        _labels[no - 1].Execute(_context);
    }
}

abstract public class MenuBuilder
{
    public abstract Menu BuildMenu(Context context);
}