using System.Globalization;
using MewingPad.Utils.AudioManager;
using MewingPad.TechnicalUI.BaseMenu;
using MewingPad.TechnicalUI.CommonCommands.AudiotrackCommands;
using MewingPad.Common.Entities;

namespace MewingPad.TechnicalUI.AdminMenu.AudiotrackCommands;

public class ChangeAudiotrackCommand : Command
{
    private readonly NumberFormatInfo _nfi = new()
    {
        NumberDecimalSeparator = ","
    };
    public override string? Description()
    {
        return "Изменить";
    }

    public override async Task Execute(Context context)
    {
        await new ViewAllAudiotracksCommand().Execute(context);
        var audiotracks = (List<Audiotrack>)context.UserObject!;
        if (audiotracks.Count == 0)
        {
            return;
        }
        Console.Write("Введите номер аудиотрека: ");
        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > audiotracks.Count)
        {
            Console.WriteLine($"[!] Аудиотрека с номером {choice} не существует");
            return;
        }
        var audio = audiotracks[choice - 1];

        Console.Write("Введите новое название (пустой ввод -- оставить таким же): ");
        var title = Console.ReadLine();

        Console.Write("Введите новую длительность (пустой ввод -- оставить такой же): ");
        float.TryParse(Console.ReadLine(), NumberStyles.Any, _nfi, out float duration);

        Console.Write("Введите новый путь к файлу (пустой ввод -- оставить таким же): ");
        var filepath = Console.ReadLine();

        audio.Title = title == "" ? audio.Title : title!;
        audio.Duration = duration == 0 ? audio.Duration : duration;
        if (filepath != "")
        {
            if (!await AudioManager.UpdateFileAsync(audio.Filepath, filepath!))
            {
                Console.WriteLine("\n[!] Не удалось обновить файл\n");
                return;
            }
            audio.Filepath = filepath!;
        }

        await context.AudiotrackService.UpdateAudiotrack(audio);
        await ChangeAudiotrackTags(audio.Id, context);
    }

    private async Task ChangeAudiotrackTags(Guid audiotrackId, Context context)
    {
        var tags = await context.TagService.GetAllTags();
        if (tags.Count == 0)
        {
            Console.WriteLine("Список тегов пуст");
            return;
        }
        else
        {
            for (int i = 0; i < tags.Count; ++i)
            {
                Console.WriteLine($"{i + 1}. {tags[i].Name}");
            }
        }

        Console.Write("Введите номера новых тегов: ");
        List<Guid> newTagIds = [];
        while (int.TryParse(Console.ReadLine(), out int choice))
        {
            if (0 >= choice || choice > tags.Count)
            {
                Console.WriteLine($"[!] Тега с номером {choice} не существует");
            }
            else
            {
                newTagIds.Add(tags[choice - 1].Id);
            }
        }
        var oldTagIds = (await context.TagService.GetAudiotrackTags(audiotrackId))
            .Select(t => t.Id)
            .ToHashSet();

        var toDelete = oldTagIds.Except(newTagIds.ToHashSet());
        var toAdd = newTagIds.Except(oldTagIds);

        foreach (var tid in toDelete)
        {
            await context.TagService.DeleteTagFromAudiotrack(audiotrackId, tid);
        }
        foreach (var tid in toAdd)
        {
            await context.TagService.AssignTagToAudiotrack(audiotrackId, tid);
        }
    }
}

