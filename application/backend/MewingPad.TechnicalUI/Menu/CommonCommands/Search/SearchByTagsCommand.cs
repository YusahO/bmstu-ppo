using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.CommonCommands.Search;

public class SearchByTagsCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<SearchByTagsCommand>();
    public override string? Description()
    {
        return "По тегу";
    }

    public override async Task Execute(Context context)
    {
        var tags = await context.TagService.GetAllTags();

        Console.WriteLine("\nВыберите номер одного из предложенных тегов:");
        int iitem = 0;
        foreach (var t in tags)
        {
            Console.WriteLine($"   {++iitem}. {t.Name}");
        }

        Console.Write("Ввод: ");
        List<Guid> chosenTagIds = [];
        while (int.TryParse(Console.ReadLine(), out int choice) &&
               0 < choice && choice <= tags.Count)
        {
            chosenTagIds.Add(tags[choice - 1].Id);
        }
        _logger.Information("User input tags to search by: {Tags}", chosenTagIds);

        var audiotracks = await context.TagService.GetAudiotracksWithTags(chosenTagIds);
        if (audiotracks.Count == 0)
        {
            Console.WriteLine("\nНичего не найдено");
        }
        else
        {
            Console.WriteLine("\nНайденные соотвествия: ");
            iitem = 0;
            foreach (var a in audiotracks)
            {
                Console.WriteLine($"   {++iitem}) {a.Title}");
                Console.WriteLine($"      {a.Duration} сек.");
                Console.WriteLine($"      {a.Filepath}");
            }
        }
    }
}

