using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.CommonCommands.PlaylistCommands;

public class RenamePlaylistCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<RenamePlaylistCommand>();
    public override string? Description()
    {
        return "Переименовать";
    }

    public override async Task Execute(Context context)
    {
        context.UserObject = false;
        await new ViewUserPlaylistsCommand().Execute(context);
        var playlists = (List<Playlist>)context.UserObject!;
        
        Console.Write("Введите номер плейлиста: ");

        var inpCheck = int.TryParse(Console.ReadLine(), out int choice);
        _logger.Information($"User input playlist number \"{choice}\"");

        if (!inpCheck)
        {
            _logger.Error("User input is invalid");
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > playlists.Count)
        {
            _logger.Error($"User input is out of range [1, {playlists.Count}]");
            Console.WriteLine($"[!] Плейлиста с номером {choice} не существует");
            return;
        }

        string? title;
        bool isInvalid;
        do
        {
            Console.Write("Введите название плейлиста: ");

            title = Console.ReadLine();
            _logger.Information($"User input playlist title \"{title}\"");

            isInvalid = title is null || playlists.Exists(p => p.Title == title);
            if (isInvalid)
            {
                _logger.Warning($"User already has playlist with name \"{title}\"");
                Console.WriteLine("[!] Плейлист с таким названием уже существует");
            }
        } while (isInvalid);

        var playlistId = playlists[choice].Id;
        await context.PlaylistService.UpdatePlaylistTitle(playlistId, title!);
        Console.WriteLine("Плейлист переименован");
    }
}