using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.CommonCommands.Search;

public class SearchByTitleCommand : Command
{
    public override string? Description()
    {
        return "По названию";
    }

    public override async Task Execute(Context context)
    {
        Console.Write("\nВведите название: ");
        var title = Console.ReadLine();
        if (title is not null)
        {
            var audiotracks = await context.AudiotrackService.GetAudiotracksByTitle(title!);
            if (audiotracks.Count == 0)
            {
                Console.WriteLine("\nНичего не найдено");
            }
            else
            {
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
    }
}

