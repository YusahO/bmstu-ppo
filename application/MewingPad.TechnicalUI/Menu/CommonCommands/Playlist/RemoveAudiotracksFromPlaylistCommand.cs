using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.CommonCommands.PlaylistCommands;

public class RemoveAudiotracksFromPlaylistCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<RemoveAudiotracksFromPlaylistCommand>();
    public override string? Description()
    {
        return "Удалить аудиотрек(и) из плейлиста";
    }

    public override async Task Execute(Context context)
    {
        context.UserObject = true;
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

        var playlistId = playlists[choice - 1].Id;
        context.UserObject = playlistId;
        await new ViewUserPlaylistAudiotracksCommand().Execute(context);

        var audios = (List<Playlist>)context.UserObject!;
        if (audios.Count != 0)
        {
            Console.Write("Введите номер(а) аудиотрека(ов): ");
            List<Guid> choiceIds = [];
            while (int.TryParse(Console.ReadLine(), out choice) &&
                   0 < choice && choice <= audios.Count)
            {
                choiceIds.Add(audios[choice - 1].Id);
            }
            _logger.Information("User input audiotracks to remove Ids {@Ids}", choiceIds);

            await context.PlaylistService.RemoveAudiotracksFromPlaylist(playlistId, choiceIds);
            Console.WriteLine("Аудиотрек(и) удален(ы) из плейлиста");
        }
    }
}