using MewingPad.Common.Entities;
using Microsoft.Extensions.Configuration;
using MewingPad.TechnicalUI.Actions;
using MewingPad.TechnicalUI.AdminMenu;
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI;

internal class Startup
{
    private readonly IConfiguration _config;
    private User? _currentUser = null;


    private PlaylistActions _playlistActions;
    private AudiotrackActions _audiotrackActions;
    private ReportActions _reportActions;
    private SearchActions _searchActions;
    private AuthActions _authActions;
    private TagActions _tagActions;

    private List<Menu> _menus = [];

    public Startup(IConfiguration config,
                   PlaylistActions playlistActions,
                   AudiotrackActions audiotrackActions,
                   ReportActions reportActions,
                   SearchActions searchActions,
                   AuthActions authActions,
                   TagActions tagActions)
    {
        _config = config;

        _playlistActions = playlistActions;
        _audiotrackActions = audiotrackActions;
        _reportActions = reportActions;
        _searchActions = searchActions;
        _authActions = authActions;
        _tagActions = tagActions;

        var builder = new AdminMenuBuilder();
        _menus.Add(builder.BuildMenu(new BaseMenu.Context()));
    }

    public async Task Run()
    {
        _menus[0].Execute();
        // int choice = -1;
        // do
        // {
        //     if (_currentUser is null)
        //     {
        //         Console.WriteLine("\nСтатус пользователя: Гость");
        //         Console.WriteLine("[1] Зарегистрироваться");
        //         Console.WriteLine("[2] Авторизоваться");
        //         Console.WriteLine("[3] Просмотреть все аудиотреки");
        //         Console.WriteLine("[4] Поиск аудиотрека");
        //         Console.WriteLine(" ├─  По названию");
        //         Console.WriteLine(" └─  По тегу");
        //         Console.WriteLine("[5] Скачать аудиотрек");
        //         Console.WriteLine("[0] Выход");
        //         Console.Write("Ввод: ");

        //         if (int.TryParse(Console.ReadLine(), out choice))
        //         {
        //             switch (choice)
        //             {
        //                 case 0:
        //                     Console.WriteLine("Выход");
        //                     break;
        //                 case 1:
        //                     _currentUser = await _authActions.RegisterUser();
        //                     break;
        //                 case 2:
        //                     _currentUser = await _authActions.SignInUser();
        //                     break;
        //                 case 3:
        //                     await _audiotrackActions.ViewAllAudiotracks();
        //                     break;
        //                 case 4:
        //                     await _searchActions.RunMenu();
        //                     break;
        //                 case 5:
        //                     await _audiotrackActions.DownloadAudiotrack();
        //                     break;
        //                 default:
        //                     Console.WriteLine($"[!] Нет пункта с номером {choice}");
        //                     break;
        //             }
        //         }
        //     }
        //     else if (!_currentUser.IsAdmin)
        //     {
        //         Console.WriteLine($"\nСтатус пользователя: Авторизован ({_currentUser.Username} {_currentUser.Email})");
        //         Console.WriteLine("[1] Поиск аудиотрека");
        //         Console.WriteLine(" ├─  По названию");
        //         Console.WriteLine(" └─  По тегу");
        //         Console.WriteLine("[2] Действия с аудиотреками");
        //         Console.WriteLine(" ├─  Просмотреть все аудиотреки");
        //         Console.WriteLine(" ├─  Скачать");
        //         Console.WriteLine(" ├─  Поставить оценку");
        //         Console.WriteLine(" ├─  Просмотреть комментарии");
        //         Console.WriteLine(" ├─  Написать комментарий");
        //         Console.WriteLine(" ├─  Изменить комментарий");
        //         Console.WriteLine(" ├─  Удалить комментарий");
        //         Console.WriteLine(" └─  Пожаловаться");
        //         Console.WriteLine("[3] Действия с плейлистами");
        //         Console.WriteLine(" ├─  Просмотреть свои плейлисты");
        //         Console.WriteLine(" ├─  Создать");
        //         Console.WriteLine(" ├─  Переименовать");
        //         Console.WriteLine(" ├─  Удалить");
        //         Console.WriteLine(" ├─  Просмотреть аудиотреки в плейлисте");
        //         Console.WriteLine(" ├─  Добавить аудиотрек в плейлист");
        //         Console.WriteLine(" └─  Удалить аудиотрек(и) из плейлиста");
        //         Console.WriteLine("[4] Выйти из аккаунта");
        //         Console.WriteLine("[0] Выход");
        //         Console.Write("Ввод: ");

        //         if (int.TryParse(Console.ReadLine(), out choice))
        //         {
        //             switch (choice)
        //             {
        //                 case 1:
        //                     await _searchActions.RunMenu();
        //                     break;
        //                 case 2:
        //                     await _audiotrackActions.RunMenu(_currentUser);
        //                     break;
        //                 case 3:
        //                     await _playlistActions.RunMenu(_currentUser);
        //                     break;
        //                 case 4:
        //                     _currentUser = null;
        //                     break;
        //                 default:
        //                     break;
        //             }
        //         }
        //     }
        //     else if (_currentUser.IsAdmin)
        //     {
        //         Console.WriteLine($"\nСтатус пользователя: Администратор ({_currentUser.Username} {_currentUser.Email})");
        //         Console.WriteLine("[1] Поиск аудиотрека");
        //         Console.WriteLine(" ├─  По названию");
        //         Console.WriteLine(" └─  По тегу");
        //         Console.WriteLine("[2] Действия с аудиотреками");
        //         Console.WriteLine(" ├─  Просмотреть все аудиотреки");
        //         Console.WriteLine(" ├─  Скачать");
        //         Console.WriteLine(" ├─  Поставить оценку");
        //         Console.WriteLine(" ├─  Просмотреть комментарии");
        //         Console.WriteLine(" ├─  Написать комментарий");
        //         Console.WriteLine(" ├─  Изменить комментарий");
        //         Console.WriteLine(" ├─  Удалить комментарий");
        //         Console.WriteLine(" ├─  Пожаловаться");
        //         Console.WriteLine(" ├─  Добавить новый");
        //         Console.WriteLine(" ├─  Изменить");
        //         Console.WriteLine(" └─  Удалить");
        //         Console.WriteLine("[3] Действия с плейлистами");
        //         Console.WriteLine(" ├─  Просмотреть свои плейлисты");
        //         Console.WriteLine(" ├─  Создать");
        //         Console.WriteLine(" ├─  Переименовать");
        //         Console.WriteLine(" ├─  Удалить");
        //         Console.WriteLine(" ├─  Добавить аудиотрек в плейлист");
        //         Console.WriteLine(" └─  Удалить аудиотрек(и) из плейлиста");
        //         Console.WriteLine("[4] Действия с жалобами");
        //         Console.WriteLine(" ├─  Просмотреть все жалобы");
        //         Console.WriteLine(" └─  Изменить статус жалобы");
        //         Console.WriteLine("[5] Действия с тегами");
        //         Console.WriteLine(" ├─  Просмотреть все теги");
        //         Console.WriteLine(" ├─  Добавить");
        //         Console.WriteLine(" ├─  Изменить");
        //         Console.WriteLine(" └─  Удалить");
        //         Console.WriteLine("[6] Добавить администратора");
        //         Console.WriteLine("[7] Выйти из аккаунта");
        //         Console.WriteLine("[0] Выход");
        //         Console.Write("Ввод: ");

        //         if (int.TryParse(Console.ReadLine(), out choice))
        //         {
        //             switch (choice)
        //             {
        //                 case 1:
        //                     await _searchActions.RunMenu();
        //                     break;
        //                 case 2:
        //                     await _audiotrackActions.RunMenu(_currentUser, true);
        //                     break;
        //                 case 3:
        //                     await _playlistActions.RunMenu(_currentUser);
        //                     break;
        //                 case 4:
        //                     await _reportActions.RunMenu();
        //                     break;
        //                 case 5:
        //                     await _tagActions.RunMenu(_currentUser);
        //                     break;
        //                 case 6:
        //                     await _authActions.AddAdmin();
        //                     break;
        //                 case 7:
        //                     _currentUser = null;
        //                     break;
        //                 default:
        //                     break;
        //             }
        //         }
        //     }
        // } while (choice != 0);
    }
}