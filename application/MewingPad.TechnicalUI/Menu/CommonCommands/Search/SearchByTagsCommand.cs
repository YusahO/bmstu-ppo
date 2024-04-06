using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.CommonCommands.Search;

public class SearchByTagsCommand : Command
{
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
        List<Guid> chosedTagIds = [];
        while (int.TryParse(Console.ReadLine(), out int chosenTag))
        {
            if (0 >= chosenTag || chosenTag > tags.Count)
            {
                Console.WriteLine($"[!] Тега с номером {chosenTag} не существует");
            }
            else
            {
                chosedTagIds.Add(tags[chosenTag - 1].Id);
            }
        }

        var audiotracks = await context.TagService.GetAudiotracksWithTags(chosedTagIds);
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

