using MewingPad.Common.Entities;
using MewingPad.Services.AudiotrackService;
using MewingPad.Services.CommentaryService;
using MewingPad.Services.ReportService;
using MewingPad.Services.ScoreService;
using MewingPad.Services.TagService;
using MewingPad.Services.UserService;
using MewingPad.Utils.AudioManager;

namespace MewingPad.TechnicalUI.Actions;

internal class AudiotrackActions(AudiotrackService audiotrackService,
                                 ScoreService scoreService,
                                 CommentaryService commentaryService,
                                 UserService userService,
                                 ReportService reportService,
                                 TagService tagService)
{
    private readonly NumberFormatInfo _nfi = new()
    {
        NumberDecimalSeparator = ","
    };

    private User? _currentUser;
    private readonly AudiotrackService _audiotrackService = audiotrackService;
    private readonly ScoreService _scoreService = scoreService;
    private readonly CommentaryService _commentaryService = commentaryService;
    private readonly UserService _userService = userService;
    private readonly ReportService _reportService = reportService;
    private readonly TagService _tagService = tagService;

    public async Task RunMenu(User currentUser, bool admin = false)
    {
        _currentUser = currentUser;
        Console.WriteLine("\n========== Действия с аудиотреками ==========");
        Console.WriteLine("1. Просмотреть все аудиотреки");
        Console.WriteLine("2. Скачать");
        Console.WriteLine("3. Поставить оценку");
        Console.WriteLine("4. Просмотреть комментарии");
        Console.WriteLine("5. Написать комментарий");
        Console.WriteLine("6. Изменить комментарий");
        Console.WriteLine("7. Удалить комментарий");
        Console.WriteLine("8. Пожаловаться");
        if (admin)
        {
            Console.WriteLine("9. Добавить новый");
            Console.WriteLine("10. Изменить");
            Console.WriteLine("11. Удалить");
        }
        Console.Write("Ввод: ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            if (choice == 1)
                await ViewAllAudiotracks();
            else if (choice == 2)
                await DownloadAudiotrack();
            else if (choice == 3)
                await SetAudiotrackScore();
            else if (choice == 4)
                await ViewAudiotrackCommentaries();
            else if (choice == 5)
                await CreateAudiotrackCommentary();
            else if (choice == 6)
                await UpdateAudiotrackCommentary();
            else if (choice == 7)
                await DeleteAudiotrackCommentary();
            else if (choice == 8)
                await ReportAudiotrack();
            else if (admin && choice == 9)
                await AddNewAudiotrack();
            else if (admin && choice == 10)
                await UpdateAudiotrack();
            else if (admin && choice == 11)
                await DeleteAudiotrack();
            else
                Console.WriteLine($"[!] Нет пункта с номером {choice}");
        }
    }

    public async Task DownloadAudiotrack()
    {
        Console.WriteLine("========== Скачивание ==========");

        var audiotracks = await ViewAllAudiotracks();
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

        if (!await AudioManager.GetFileAsync(audiotracks[choice - 1].Filepath, "/home/daria/Загрузки"))
        {
            Console.WriteLine($"[!] Не удалось скачать файл");
        }
    }

    public async Task<List<Audiotrack>> ViewAllAudiotracks()
    {
        var audiotracks = await _audiotrackService.GetAllAudiotracks();
        if (audiotracks.Count == 0)
        {
            Console.WriteLine("Список аудиофайлов пуст");
        }
        else
        {
            Console.WriteLine();
            int i = 0;
            foreach (var a in audiotracks)
            {
                var scores = await _scoreService.GetAudiotrackScores(a.Id);
                var tags = await _tagService.GetAudiotrackTags(a.Id);
                double meanScore = 0.0f;
                if (scores.Count != 0)
                {
                    meanScore = scores.Average(s => s.Value);
                    meanScore = (meanScore - Math.Floor(meanScore)
                                 < 0.5) ? Math.Floor(meanScore) : Math.Floor(meanScore) + 0.5;
                }
                Console.WriteLine($"{++i}) {a.Title}");
                Console.WriteLine($"   {a.Duration} сек.");
                Console.WriteLine($"   {a.Filepath}");
                Console.WriteLine($"   {meanScore} ★");
                Console.Write("   Теги: ");
                foreach (var t in tags)
                {
                    Console.Write($"{t.Name} ");
                }
                Console.WriteLine();
            }
        }
        return audiotracks;
    }

    private async Task SetAudiotrackScore()
    {
        var audiotracks = await ViewAllAudiotracks();
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

        Guid audiotrackId = audiotracks[choice - 1].Id;
        var score = (await _scoreService.GetAudiotrackScores(audiotrackId))
            .Find(s => s.AudiotrackId == audiotrackId);

        Console.Write("Введите оценку (0 - 5): ");
        if (int.TryParse(Console.ReadLine(), out int value))
        {
            if (score is null)
            {
                score = new Score(audiotracks[choice - 1].Id, _currentUser!.Id, value);
                await _scoreService.CreateScore(score);
                Console.WriteLine("Оценка сохранена");
            }
            else
            {
                score.SetValue(value);
                await _scoreService.UpdateScore(score);
                Console.WriteLine("Оценка обновлена");
            }
        }
        else
        {
            Console.WriteLine("[!] Введено недопустимое значение оценки");
        }
    }

    private async Task CreateAudiotrackCommentary()
    {
        var audiotracks = await ViewAllAudiotracks();
        Console.Write("Введите номер аудиотрека: ");
        if (audiotracks.Count == 0)
        {
            return;
        }
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

        Console.Write("Введите содержимое комментария: ");
        var commentaryText = Console.ReadLine();
        if (commentaryText is null)
        {
            Console.WriteLine("[!] Текст комментария должен быть непустым");
        }
        else
        {
            var commentary = new Commentary(Guid.NewGuid(), _currentUser!.Id, audiotracks[choice - 1].Id, commentaryText!);
            await _commentaryService.CreateCommentary(commentary);
            Console.WriteLine("Комментарий создан");
        }
    }

    private async Task ViewAudiotrackCommentaries()
    {
        var audiotracks = await ViewAllAudiotracks();
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

        var comms = await _commentaryService.GetAudiofileCommentaries(audiotracks[choice - 1].Id);
        if (comms.Count == 0)
        {
            Console.WriteLine("Список комментариев пуст");
            return;
        }
        foreach (var c in comms)
        {
            var username = (await _userService.GetUserById(c.AuthorId)).Username;
            Console.WriteLine($"-> {username}");
            Console.WriteLine($"   {c.Text}");
        }
    }

    private async Task UpdateAudiotrackCommentary()
    {
        var audiotracks = await ViewAllAudiotracks();
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

        var comms = (await _commentaryService.GetAudiofileCommentaries(audiotracks[choice - 1].Id))
            .FindAll(c => c.AuthorId == _currentUser!.Id);
        if (comms.Count == 0)
        {
            Console.WriteLine("Список комментариев пуст");
            return;
        }
        for (int i = 0; i < comms.Count; ++i)
        {
            var username = (await _userService.GetUserById(comms[i].AuthorId)).Username;
            Console.WriteLine($"{i}. {username}");
            Console.WriteLine($"   {comms[i].Text}");
        }

        Console.Write("Введите номер комментария: ");
        if (!int.TryParse(Console.ReadLine(), out choice))
        {
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > comms.Count)
        {
            Console.WriteLine($"[!] Комментария с номером {choice} не существует");
            return;
        }

        Console.Write("Введите содержимое комментария: ");
        var commentaryText = Console.ReadLine();
        if (commentaryText is null)
        {
            Console.WriteLine("[!] Текст комментария должен быть непустым");
        }
        else
        {
            comms[choice - 1].Text = commentaryText;
            await _commentaryService.UpdateCommentary(comms[choice - 1]);
            Console.WriteLine("Комментарий обновлен");
        }
    }

    private async Task DeleteAudiotrackCommentary()
    {
        var audiotracks = await ViewAllAudiotracks();
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

        var comms = (await _commentaryService.GetAudiofileCommentaries(audiotracks[choice - 1].Id))
            .FindAll(c => c.AuthorId == _currentUser!.Id);
        int iitem = 0;
        foreach (var c in comms)
        {
            var username = (await _userService.GetUserById(c.AuthorId)).Username;
            Console.WriteLine($"{++iitem}. {username}");
            Console.WriteLine($"   {c.Text}");
        }

        Console.Write("Введите номер комментария: ");
        if (!int.TryParse(Console.ReadLine(), out choice))
        {
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > comms.Count)
        {
            Console.WriteLine($"[!] Комментария с номером {choice} не существует");
            return;
        }

        await _commentaryService.DeleteCommentary(comms[choice - 1].Id);
        Console.WriteLine("Комментарий удален");
    }

    private async Task ReportAudiotrack()
    {
        var audiotracks = await ViewAllAudiotracks();
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

        Console.Write("Введите причину жалобы: ");
        var reportText = Console.ReadLine();
        if (reportText is null)
        {
            Console.WriteLine("[!] Текст должен быть непустым");
        }
        else
        {
            var report = new Report(Guid.NewGuid(), _currentUser!.Id, audiotracks[choice - 1].Id, reportText!);
            await _reportService.CreateReport(report);
            Console.WriteLine("Жалоба отправлена");
        }
    }

    private async Task AddNewAudiotrack()
    {
        string? title, filepath;

        Console.Write("Введите название аудиотрека: ");
        title = Console.ReadLine();
        if (title is null)
        {
            Console.WriteLine("[!] Введено некорректное значение");
            return;
        }

        Console.Write("Введите длительность: ");
        if (!float.TryParse(Console.ReadLine(), NumberStyles.Any, _nfi, out float duration))
        {
            Console.WriteLine("[!] Введено некорректное значение");
            return;
        }

        Console.Write("Введите путь к файлу аудиотрека: ");
        filepath = Console.ReadLine();
        if (filepath is null)
        {
            Console.WriteLine("[!] Введено некорректное значение");
            return;
        }

        if (!await AudioManager.CreateFileAsync(filepath!))
        {
            Console.WriteLine("[!] Не удалось загрузить файл");
            return;
        }
        var audio = new Audiotrack(Guid.NewGuid(), title!, (float)duration!, _currentUser!.Id, Path.GetFileName(filepath!));
        await _audiotrackService.CreateAudiotrack(audio);
    }

    private async Task ChangeAudiotrackTags(Guid audiotrackId)
    {
        var tags = await _tagService.GetAllTags();
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
        var oldTagIds = (await _tagService.GetAudiotrackTags(audiotrackId))
            .Select(t => t.Id)
            .ToHashSet();

        var toDelete = oldTagIds.Except(newTagIds.ToHashSet());
        var toAdd = newTagIds.Except(oldTagIds);

        foreach (var tid in toDelete)
        {
            await _tagService.DeleteTagFromAudiotrack(audiotrackId, tid);
        }
        foreach (var tid in toAdd)
        {
            await _tagService.AssignTagToAudiotrack(audiotrackId, tid);
        }
    }

    private async Task UpdateAudiotrack()
    {
        var audiotracks = await ViewAllAudiotracks();
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

        await _audiotrackService.UpdateAudiotrack(audio);
        await ChangeAudiotrackTags(audio.Id);
    }

    private async Task DeleteAudiotrack()
    {
        var audiotracks = await ViewAllAudiotracks();
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
        if (!await AudioManager.DeleteFileAsync(audio.Filepath))
        {
            Console.WriteLine("[!] Не удалось удалить файл");
            return;
        }
        await _audiotrackService.DeleteAudiotrack(audio.Id);
    }

}