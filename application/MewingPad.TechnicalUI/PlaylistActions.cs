using MewingPad.Common.Entities;
using MewingPad.Services.AudiotrackService;
using MewingPad.Services.PlaylistService;

namespace MewingPad.TechnicalUI.Actions;

internal class PlaylistActions(PlaylistService playlistService,
                               AudiotrackService audiotrackService)
{
    private User? _currentUser;
    private readonly PlaylistService _playlistService = playlistService;
    private readonly AudiotrackService _audiotrackService = audiotrackService;

    public async Task RunMenu(User currentUser)
    {
        _currentUser = currentUser;
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
                Console.WriteLine($"{++iitem}) {a.Title}");
                Console.WriteLine($"   {a.Duration} сек.");
                Console.WriteLine($"   {a.Filepath}");
            }
        }
        return audiotracks;
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

        var playlistId = playlists[choice].Id;
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

}