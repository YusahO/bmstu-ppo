using System.Globalization;
using System.Numerics;
using MewingPad.Common.Entities;
using MewingPad.Common.Enums;
using MewingPad.Services.AudiotrackService;
using MewingPad.Services.CommentaryService;
using MewingPad.Services.OAuthService;
using MewingPad.Services.PlaylistService;
using MewingPad.Services.ReportService;
using MewingPad.Services.ScoreService;
using MewingPad.Services.TagService;
using MewingPad.Services.UserService;
using MewingPad.Utils.AudioManager;
using MewingPad.Utils.PasswordHasher;
using Microsoft.Extensions.Configuration;

namespace MewingPad.TechnicalUI;

public class Startup(IConfiguration config,
                     UserService userService,
                     OAuthService oauthService,
                     PlaylistService playlistService,
                     AudiotrackService audiotrackService,
                     TagService tagService,
                     ScoreService scoreService,
                     CommentaryService commentaryService,
                     ReportService reportService)
{
    private readonly IConfiguration _config = config;
    private User? _currentUser = null;
    private readonly NumberFormatInfo _nfi = new()
    {
        NumberDecimalSeparator = ","
    };

    private readonly UserService _userService = userService;
    private readonly OAuthService _oauthService = oauthService;
    private readonly PlaylistService _playlistService = playlistService;
    private readonly AudiotrackService _audiotrackService = audiotrackService;
    private readonly TagService _tagService = tagService;
    private readonly ScoreService _scoreService = scoreService;
    private readonly CommentaryService _commentaryService = commentaryService;
    private readonly ReportService _reportService = reportService;

    public async Task Run()
    {
        int choice = -1;
        do
        {
            if (_currentUser is null)
            {
                Console.WriteLine("\nСтатус пользователя: Гость");
                Console.WriteLine("[1] Зарегистрироваться");
                Console.WriteLine("[2] Авторизоваться");
                Console.WriteLine("[3] Просмотреть все аудиотреки");
                Console.WriteLine("[4] Поиск аудиотрека");
                Console.WriteLine(" ├─  По названию");
                Console.WriteLine(" └─  По тегу");
                Console.WriteLine("[5] Скачать аудиотрек");
                Console.WriteLine("[0] Выход");
                Console.Write("Ввод: ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 0:
                            Console.WriteLine("Выход");
                            break;
                        case 1:
                            await RegisterUser();
                            break;
                        case 2:
                            await SignInUser();
                            break;
                        case 3:
                            await ViewAllAudiotracks();
                            break;
                        case 4:
                            await Search();
                            break;
                        case 5:
                            await DownloadAudiotrack();
                            break;
                        default:
                            Console.WriteLine($"[!] Нет пункта с номером {choice}");
                            break;
                    }
                }
            }
            else if (!_currentUser.IsAdmin)
            {
                Console.WriteLine($"\nСтатус пользователя: Авторизован ({_currentUser.Username} {_currentUser.Email})");
                Console.WriteLine("[1] Поиск аудиотрека");
                Console.WriteLine(" ├─  По названию");
                Console.WriteLine(" └─  По тегу");
                Console.WriteLine("[2] Действия с аудиотреками");
                Console.WriteLine(" ├─  Просмотреть все аудиотреки");
                Console.WriteLine(" ├─  Скачать");
                Console.WriteLine(" ├─  Поставить оценку");
                Console.WriteLine(" ├─  Просмотреть комментарии");
                Console.WriteLine(" ├─  Написать комментарий");
                Console.WriteLine(" ├─  Изменить комментарий");
                Console.WriteLine(" ├─  Удалить комментарий");
                Console.WriteLine(" └─  Пожаловаться");
                Console.WriteLine("[3] Действия с плейлистами");
                Console.WriteLine(" ├─  Просмотреть свои плейлисты");
                Console.WriteLine(" ├─  Создать");
                Console.WriteLine(" ├─  Переименовать");
                Console.WriteLine(" ├─  Удалить");
                Console.WriteLine(" ├─  Просмотреть аудиотреки в плейлисте");
                Console.WriteLine(" ├─  Добавить аудиотрек в плейлист");
                Console.WriteLine(" └─  Удалить аудиотрек(и) из плейлиста");
                Console.WriteLine("[4] Выйти из аккаунта");
                Console.WriteLine("[0] Выход");
                Console.Write("Ввод: ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            await Search();
                            break;
                        case 2:
                            await AudiotracksActions();
                            break;
                        case 3:
                            await PlaylistActions();
                            break;
                        case 4:
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (_currentUser.IsAdmin)
            {
                Console.WriteLine($"\nСтатус пользователя: Администратор ({_currentUser.Username} {_currentUser.Email})");
                Console.WriteLine("[1] Поиск аудиотрека");
                Console.WriteLine(" ├─  По названию");
                Console.WriteLine(" └─  По тегу");
                Console.WriteLine("[2] Действия с аудиотреками");
                Console.WriteLine(" ├─  Просмотреть все аудиотреки");
                Console.WriteLine(" ├─  Скачать");
                Console.WriteLine(" ├─  Поставить оценку");
                Console.WriteLine(" ├─  Просмотреть комментарии");
                Console.WriteLine(" ├─  Написать комментарий");
                Console.WriteLine(" ├─  Изменить комментарий");
                Console.WriteLine(" ├─  Удалить комментарий");
                Console.WriteLine(" ├─  Пожаловаться");
                Console.WriteLine(" ├─  Добавить новый");
                Console.WriteLine(" ├─  Изменить");
                Console.WriteLine(" └─  Удалить");
                Console.WriteLine("[3] Действия с плейлистами");
                Console.WriteLine(" ├─  Просмотреть свои плейлисты");
                Console.WriteLine(" ├─  Создать");
                Console.WriteLine(" ├─  Переименовать");
                Console.WriteLine(" ├─  Удалить");
                Console.WriteLine(" ├─  Добавить аудиотрек в плейлист");
                Console.WriteLine(" └─  Удалить аудиотрек(и) из плейлиста");
                Console.WriteLine("[4] Действия с жалобами");
                Console.WriteLine(" ├─  Просмотреть все жалобы");
                Console.WriteLine(" └─  Изменить статус жалобы");
                Console.WriteLine("[5] Действия с тегами");
                Console.WriteLine(" ├─  Просмотреть все теги");
                Console.WriteLine(" ├─  Добавить");
                Console.WriteLine(" ├─  Изменить");
                Console.WriteLine(" └─  Удалить");
                Console.WriteLine("[6] Добавить администратора");
                Console.WriteLine("[7] Выйти из аккаунта");
                Console.WriteLine("[0] Выход");
                Console.Write("Ввод: ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            await Search();
                            break;
                        case 2:
                            await AudiotracksActions(true);
                            break;
                        case 3:
                            await PlaylistActions();
                            break;
                        case 4:
                            await ReportsActions();
                            break;
                        case 5:
                            // await TagActions();
                            break;
                        default:
                            break;
                    }
                }
            }
        } while (choice != 0);
    }

    private async Task RegisterUser()
    {
        Console.WriteLine("\n========== Регистрация ==========");

        string? username, password, passwordVerify, email;
        bool isIncorrect;

        do
        {
            Console.Write("Введите имя пользователя: ");
            username = Console.ReadLine();
            if (username is null || username.Length < 3)
            {
                isIncorrect = true;
                Console.WriteLine("[!] Имя пользователя должно содержать более 2 символов");
            }
            else
            {
                isIncorrect = false;
            }
        } while (isIncorrect);

        do
        {
            Console.Write("Введите пароль: ");
            password = Console.ReadLine();
            if (password is null || password.Length < 3)
            {
                isIncorrect = true;
                Console.WriteLine("[!] Пароль должен содержать 8 символов и более");
            }
            else
            {
                isIncorrect = false;
            }
        } while (isIncorrect);

        do
        {
            Console.Write("-> Подтвердите пароль: ");
            passwordVerify = Console.ReadLine();
        } while (password != passwordVerify);

        do
        {
            Console.Write("Введите адрес электронной почты: ");
            email = Console.ReadLine();
            if (email is null || !email.Contains('@') || !email.Contains('.'))
            {
                isIncorrect = true;
                Console.WriteLine("[!] Введенный адрес имеет некорректный формат");
            }
            else
            {
                isIncorrect = false;
            }
        } while (isIncorrect);

        try
        {
            var userId = Guid.NewGuid();
            var user = new User(userId,
                                Guid.NewGuid(),
                                username!,
                                email!,
                                PasswordHasher.HashPassword(password!));
            await _oauthService.RegisterUser(user);
            _currentUser = user;
            Console.WriteLine("Регистрация прошла успешно");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[!] {ex.Message}\n");
        }
    }

    private async Task SignInUser()
    {
        Console.WriteLine("\n========== Авторизация ==========");

        Console.Write("Введите адрес электронной почты: ");
        var email = Console.ReadLine();

        Console.Write("Введите пароль: ");
        var password = Console.ReadLine();

        if (email is null && password is null)
        {
            Console.WriteLine("[!] Неверный ввод\n");
            return;
        }

        try
        {
            _currentUser = await _oauthService.SignInUser(email!, password!);
            Console.WriteLine("Авторизация прошла успешно");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n{ex.Message}\n");
        }
    }

    private async Task Search()
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

    private async Task SearchByTag() // TODO: search by many tags
    {
        var indexedTags = (await _tagService.GetAllTags())
            .Select((item, i) => new { Index = i + 1, Value = item })
            .ToArray();

        Console.WriteLine("\nВыберите номер одного из предложенных тегов:");
        foreach (var it in indexedTags)
        {
            Console.WriteLine($"   {it.Index}. {it.Value.Name}");
        }

        Console.Write("Ввод: ");
        if (int.TryParse(Console.ReadLine(), out int chosenTag))
        {
            if (0 < chosenTag && chosenTag <= indexedTags.Length)
            {
                var audiotracks = await _tagService.GetAudiotracksWithTag(indexedTags[chosenTag - 1].Value.Id);
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

    private async Task AudiotracksActions(bool admin = false)
    {
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

    private async Task<List<Audiotrack>> ViewAllAudiotracks()
    {
        var audiotracks = await _audiotrackService.GetAllAudiotracks();
        if (audiotracks.Count == 0)
        {
            Console.WriteLine("Список аудиофайлов пуст");
        }
        else
        {
            Console.WriteLine();
            int iitem = 0;
            foreach (var a in audiotracks)
            {
                var scores = await _scoreService.GetAudiotrackScores(a.Id);
                float meanScore = 0.0f;
                if (scores.Count != 0)
                {
                    meanScore = (float)scores.Average(s => s.Value);
                }
                Console.WriteLine($"{++iitem}) {a.Title}");
                Console.WriteLine($"   {a.Duration} сек.");
                Console.WriteLine($"   {a.Filepath}");
                Console.WriteLine($"   {meanScore} ★");
            }
        }
        return audiotracks;
    }

    private async Task DownloadAudiotrack()
    {
        Console.WriteLine("========== Скачивание ==========");

        var audiotracks = await ViewAllAudiotracks();
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

        if (await AudioManager.GetFileAsync(audiotracks[choice - 1].Filepath, "/home/daria/Загрузки"))
        {
            Console.WriteLine($"[!] Не удалось скачать файл");
        }
    }

    private async Task SetAudiotrackScore()
    {
        var audiotracks = await ViewAllAudiotracks();
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
        var audio = new Audiotrack(Guid.NewGuid(), title!, (float)duration!, _currentUser!.Id, filepath!);
        await _audiotrackService.CreateAudiotrack(audio);
    }

    private async Task UpdateAudiotrack()
    {
        var audiotracks = await ViewAllAudiotracks();
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
    }

    private async Task DeleteAudiotrack()
    {
        var audiotracks = await ViewAllAudiotracks();
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

    private async Task PlaylistActions()
    {
        Console.WriteLine("\n========== Действия с плейлистами ==========");
        Console.WriteLine("1. Просмотреть свои плейлисты");
        Console.WriteLine("2. Создать");
        Console.WriteLine("3. Переименовать");
        Console.WriteLine("4. Удалить");
        Console.WriteLine("5. Просмотреть аудиотреки в плейлисте");
        Console.WriteLine("6. Добавить аудиотрек в плейлист");
        Console.WriteLine("7. Удалить аудиотрек(и) из плейлиста");
        Console.Write("Ввод: ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            switch (choice)
            {
                case 1:
                    await ViewUserPlaylists();
                    break;
                case 2:
                    await CreatePlaylist();
                    break;
                case 3:
                    await RenamePlaylist();
                    break;
                case 4:
                    await DeletePlaylist();
                    break;
                case 5:
                    await ViewUserPlaylistAudiotracks();
                    break;
                case 6:
                    await AddAudiotrackToPlaylist();
                    break;
                case 7:
                    await RemoveAudiotracksFromPlaylist();
                    break;
                default:
                    Console.WriteLine($"[!] Нет пункта с номером {choice}");
                    break;
            }
        }
    }

    private async Task<List<Playlist>> ViewUserPlaylists(bool printFavourites = true)
    {
        Console.WriteLine($"\nСписок плейлистов {_currentUser!.Username}");
        var playlists = await _playlistService.GetUserPlaylists(_currentUser!.Id);

        int iitem = 0;
        if (printFavourites)
        {
            foreach (var p in playlists)
            {
                Console.WriteLine($"{++iitem}. {p.Title}");
            }
        }
        else
        {
            foreach (var p in playlists)
            {
                if (p.Id != _currentUser!.FavouritesId)
                { Console.WriteLine($"{++iitem}. {p.Title}"); }
            }
        }
        return playlists;
    }

    private async Task CreatePlaylist()
    {
        var playlists = await ViewUserPlaylists();

        string? title;
        bool isInvalid;
        do
        {
            Console.Write("Введите название плейлиста: ");
            title = Console.ReadLine();
            isInvalid = title is null || playlists.Exists(p => p.Title == title);
            if (isInvalid)
            {
                Console.WriteLine("[!] Плейлист с таким названием уже существует");
            }
        } while (isInvalid);

        var playlist = new Playlist(Guid.NewGuid(), title!, _currentUser!.Id);
        await _playlistService.CreatePlaylist(playlist);
        Console.WriteLine("Плейлист создан");
    }

    private async Task RenamePlaylist()
    {
        var playlists = await ViewUserPlaylists(false);
        Console.Write("Введите номер плейлиста: ");
        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > playlists.Count)
        {
            Console.WriteLine($"[!] Плейлиста с номером {choice} не существует");
            return;
        }

        string? title;
        bool isInvalid;
        do
        {
            Console.Write("Введите название плейлиста: ");
            title = Console.ReadLine();
            isInvalid = title is null || playlists.Exists(p => p.Title == title);
            if (isInvalid)
            {
                Console.WriteLine("[!] Плейлист с таким названием уже существует");
            }
        } while (isInvalid);

        var playlistId = playlists[choice - 1].Id;
        await _playlistService.UpdateTitle(playlistId, title!);
        Console.WriteLine("Плейлист переименован");
    }

    private async Task DeletePlaylist()
    {
        var playlists = await ViewUserPlaylists(false);
        Console.Write("Введите номер плейлиста: ");
        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > playlists.Count)
        {
            Console.WriteLine($"[!] Плейлиста с номером {choice} не существует");
            return;
        }

        await _playlistService.DeletePlaylist(playlists[choice].Id);
        Console.WriteLine("Плейлист удален");
    }

    private async Task<List<Audiotrack>> ViewUserPlaylistAudiotracks(Guid playlistId = new())
    {
        if (playlistId == Guid.Empty)
        {
            var playlists = await ViewUserPlaylists();
            Console.Write("Введите номер плейлиста: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("[!] Введенное значение имеет некорректный формат");
                return [];
            }
            if (0 >= choice || choice > playlists.Count)
            {
                Console.WriteLine($"[!] Плейлиста с номером {choice} не существует");
                return [];
            }
            playlistId = playlists[choice - 1].Id;
        }

        var audios = await _playlistService.GetAllAudiotracksFromPlaylist(playlistId);
        if (audios.Count == 0)
        {
            Console.WriteLine("В плейлисте ничего нет");
        }
        else
        {
            int iitem = 0;
            foreach (var a in audios)
            {
                Console.WriteLine($"{++iitem}) {a.Title}");
                Console.WriteLine($"   {a.Duration} сек.");
            }
        }
        return audios;
    }

    private async Task AddAudiotrackToPlaylist()
    {
        var playlists = await ViewUserPlaylists();
        Console.Write("Введите номер плейлиста: ");
        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > playlists.Count)
        {
            Console.WriteLine($"[!] Плейлиста с номером {choice} не существует");
            return;
        }

        var playlistId = playlists[choice - 1].Id;
        var audios = await ViewAllAudiotracks();
        if (audios.Count != 0)
        {
            Console.Write("Введите номер аудиотрека: ");
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("[!] Введенное значение имеет некорректный формат");
                return;
            }
            if (0 >= choice || choice > audios.Count)
            {
                Console.WriteLine($"[!] Аудиотрека с номером {choice} не существует");
                return;
            }

            await _playlistService.AddAudiofileToPlaylist(playlistId, audios[choice - 1].Id);
            Console.WriteLine("Аудиотрек добавлен в плейлист");
        }
    }

    private async Task RemoveAudiotracksFromPlaylist()
    {
        var playlists = await ViewUserPlaylists();
        Console.Write("Введите номер плейлиста: ");
        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > playlists.Count)
        {
            Console.WriteLine($"[!] Плейлиста с номером {choice} не существует");
            return;
        }

        var playlistId = playlists[choice - 1].Id;
        var audios = await ViewUserPlaylistAudiotracks(playlistId);
        if (audios.Count != 0)
        {
            Console.Write("Введите номер(а) аудиотрека(ов): ");
            List<Guid> choiceIds = [];
            while (int.TryParse(Console.ReadLine(), out choice) &&
                   0 < choice && choice <= audios.Count)
            {
                choiceIds.Add(audios[choice - 1].Id);
            }

            await _playlistService.RemoveAudiofilesFromPlaylist(playlistId, choiceIds);
            Console.WriteLine("Аудиотрек(и) удален(ы) из плейлиста");
        }
    }

    private async Task ReportsActions()
    {
        Console.WriteLine("\n========== Действия с жалобами ==========");
        Console.WriteLine("1. Просмотреть все жалобы");
        Console.WriteLine("2. Изменить статус жалобы");
        Console.Write("Ввод: ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine();
            switch (choice)
            {
                case 1:
                    await ViewAllReports();
                    break;
                case 2:
                    await ChangeReportStatus();
                    break;
                default:
                    Console.WriteLine($"[!] Нет пункта с номером {choice}");
                    break;
            }
        }
    }

    private async Task<List<Report>> ViewAllReports()
    {
        var reports = await _reportService.GetAllReports();
        if (reports.Count == 0)
        {
            Console.WriteLine("Список жалоб пуст");
        }
        else
        {
            int iitem = 0;
            foreach (var r in reports)
            {
                var user = await _userService.GetUserById(r.AuthorId);
                var audio = await _audiotrackService.GetAudiotrackById(r.AudiotrackId);
                Console.WriteLine($"{++iitem}. Жалоба пользователя {user.Username} на аудиотрек \"{audio.Title}\"");
                Console.WriteLine($"   Причина: \"{r.Text}\"");
                Console.WriteLine($"   Статус: \"{r.Status}\"");
            }
        }
        return reports;
    }

    private async Task ChangeReportStatus()
    {
        var reports = await ViewAllReports();
        Console.Write("Введите номер жалобы: ");
        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > reports.Count)
        {
            Console.WriteLine($"[!] Жалобы с номером {choice} не существует");
            return;
        }

        var report = reports[choice - 1];
        string? selection;
        ReportStatus newStatus;

        Console.Write("Отметить непрочитанным? [y/n] ");
        selection = Console.ReadLine();
        if (selection == "y")
        {
            newStatus = ReportStatus.NotViewed;
        }
        else
        {
            Console.Write("Принять жалобу? [y/n/[empty]] ");
            selection = Console.ReadLine();
            if (selection == "y")
            {
                newStatus = ReportStatus.Accepted;
            }
            else if (selection == "n")
            {
                newStatus = ReportStatus.Declined;
            }
            else if (selection == "")
            {
                newStatus = ReportStatus.Viewed;
            }
            else
            {
                Console.WriteLine("[!] Введенно некорректное значение");
                return;
            }
        }

        await _reportService.UpdateReportStatus(report.Id, newStatus);
        Console.WriteLine("Статус жалобы изменен");
    }
}