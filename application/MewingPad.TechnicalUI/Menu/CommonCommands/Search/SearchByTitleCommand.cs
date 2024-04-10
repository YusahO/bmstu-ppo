using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.CommonCommands.Search;

public class SearchByTitleCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<SearchByTitleCommand>();
    public override string? Description()
    {
        return "По названию";
    }

    public override async Task Execute(Context context)
    {
        Console.Write("\nВведите название: ");

        var title = Console.ReadLine();
        _logger.Information($"User searches by title \"{title}\"");

        if (title is null)
        {
            _logger.Error("User input is invalid");
            Console.WriteLine("[!] Введенное название должно быть непустым");
            return;
        }

        var audiotracks = await context.AudiotrackService.GetAudiotracksByTitle(title!);
        if (audiotracks.Count == 0)
        {
            Console.WriteLine("\nНичего не найдено");
            return;
        }
        
        Console.WriteLine("\nНайденные соотвествия: ");
        int iitem = 0;
        foreach (var a in audiotracks)
        {
            Console.WriteLine($"   {++iitem}) {a.Title}");
            Console.WriteLine($"      {a.Duration} сек.");
            Console.WriteLine($"      {a.Filepath}");
        }
    }
}

