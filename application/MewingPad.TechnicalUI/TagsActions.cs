using MewingPad.Common.Entities;
using MewingPad.Services.TagService;
using MewingPad.Services.UserService;

namespace MewingPad.TechnicalUI.Actions;

internal class TagActions(TagService tagService, UserService userService)
{
    private User? _currentUser;
    private readonly TagService _tagService = tagService;
    private readonly UserService _userService = userService;

    public async Task RunMenu(User currentUser)
    {
        _currentUser = currentUser;
        Console.WriteLine("\n========== Действия с тегами ==========");
        Console.WriteLine("1. Просмотреть все теги");
        Console.WriteLine("2. Создать");
        Console.WriteLine("3. Изменить");
        Console.WriteLine("4. Удалить");
        Console.Write("Ввод: ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine();
            switch (choice)
            {
                case 1:
                    await ViewAllTags();
                    break;
                case 2:
                    await CreateTag();
                    break;
                case 3:
                    await UpdateTag();
                    break;
                case 4:
                    await DeleteTag();
                    break;
                default:
                    Console.WriteLine($"[!] Нет пункта с номером {choice}");
                    break;
            }
        }
    }

    private async Task<List<Tag>> ViewAllTags()
    {
        var tags = await _tagService.GetAllTags();
        if (tags.Count == 0)
        {
            Console.WriteLine("Список тегов пуст");
        }
        else
        {
            for (int i = 0; i < tags.Count; ++i)
            {
                var u = await _userService.GetUserById(tags[i].AuthorId);
                Console.WriteLine($"{i + 1}. {tags[i].Name}");
                Console.WriteLine($"   Автор: {u.Username}");
            }
        }
        return tags;
    }

    private async Task<List<Tag>> ViewUserTags()
    {
        var tags = await _tagService.GetAllTags();
        tags = (from t in tags
                where t.AuthorId == _currentUser!.Id
                select t).ToList();
        if (tags.Count == 0)
        {
            Console.WriteLine("Список тегов пуст");
        }
        else
        {
            for (int i = 0; i < tags.Count; ++i)
            {
                Console.WriteLine($"{i + 1}. {tags[i].Name}");
            }
        }
        return tags;
    }

    private async Task CreateTag()
    {
        Console.Write("Введите название тега: ");
        var name = Console.ReadLine();
        if (name is null)
        {
            Console.WriteLine("[!] Название тега должно быть непустым");
            return;
        }

        var tag = new Tag(Guid.NewGuid(), _currentUser!.Id, name);
        await _tagService.CreateTag(tag);
        Console.WriteLine("Тег создан");
    }

    private async Task UpdateTag()
    {
        var tags = await ViewUserTags();
        if (tags.Count == 0)
        {
            return;
        }
        Console.Write("Введите номер тега: ");
        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > tags.Count)
        {
            Console.WriteLine($"[!] Аудиотрека с номером {choice} не существует");
            return;
        }

        Console.Write("Введите название тега: ");
        var name = Console.ReadLine();
        if (name is null)
        {
            Console.WriteLine("[!] Название тега должно быть непустым");
            return;
        }

        var tagId = tags[choice - 1].Id;
        await _tagService.UpdateTagName(tagId, name);
        Console.WriteLine("Тег обновлен");
    }

    private async Task DeleteTag()
    {
        var tags = await ViewUserTags();
        if (tags.Count == 0)
        {
            return;
        }
        Console.Write("Введите номер тега: ");
        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > tags.Count)
        {
            Console.WriteLine($"[!] Аудиотрека с номером {choice} не существует");
            return;
        }

        await _tagService.DeleteTag(tags[choice - 1].Id);
        Console.WriteLine("Тег удален");
    }
}