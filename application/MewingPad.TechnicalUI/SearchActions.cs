using MewingPad.Services.AudiotrackService;
using MewingPad.Services.TagService;

namespace MewingPad.TechnicalUI.Actions;

internal class SearchActions(TagService tagService,
                             AudiotrackService audiotrackService)
{

    private readonly TagService _tagService = tagService;
    private readonly AudiotrackService _audiotrackService = audiotrackService;

    public async Task RunMenu()
    {
        Console.WriteLine("\n========== Поиск ==========");
        Console.WriteLine("1. По названию");
        Console.WriteLine("2. По тегу");
        Console.Write("Ввод: ");
        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            switch (choice)
            {
                case 1:
                    await SearchByTitle();
                    break;
                case 2:
                    await SearchByTag();
                    break;
                default:
                    Console.WriteLine($"[!] Нет пункта с номером {choice}");
                    break;
            }
        }
    }

    private async Task SearchByTag()
    {
        var tags = await _tagService.GetAllTags();

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

        var audiotracks = await _tagService.GetAudiotracksWithTags(chosedTagIds);
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

    private async Task SearchByTitle()
    {
        Console.Write("\nВведите название: ");
        var title = Console.ReadLine();
        if (title is not null)
        {
            var audiotracks = await _audiotrackService.GetAudiotracksByTitle(title!);
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