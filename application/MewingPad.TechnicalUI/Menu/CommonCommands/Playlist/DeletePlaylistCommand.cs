using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.CommonCommands.PlaylistCommands;

public class DeletePlaylistCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<DeletePlaylistCommand>();
    public override string? Description()
    {
        return "Удалить";
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

        await context.PlaylistService.DeletePlaylist(playlists[choice].Id);
        Console.WriteLine("Плейлист удален");
    }
}