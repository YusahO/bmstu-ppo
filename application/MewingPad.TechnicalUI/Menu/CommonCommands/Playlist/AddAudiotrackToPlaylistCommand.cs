using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using MewingPad.TechnicalUI.CommonCommands.AudiotrackCommands;
using Serilog;

namespace MewingPad.TechnicalUI.CommonCommands.PlaylistCommands;

public class AddAudiotrackToPlaylistCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<AddAudiotrackToPlaylistCommand>();
    public override string? Description()
    {
        return "Добавить аудиотрек в плейлист";
    }

    public override async Task Execute(Context context)
    {
        context.UserObject = true;
        await new ViewUserPlaylistsCommand().Execute(context);
        var playlists = (List<Playlist>)context.UserObject!;
        if (playlists.Count == 0)
        {
            return;
        }

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
        await new ViewAllAudiotracksCommand().Execute(context);
        var audios = (List<Audiotrack>)context.UserObject!;
        if (audios.Count != 0)
        {
            Console.Write("Введите номер аудиотрека: ");

            inpCheck = int.TryParse(Console.ReadLine(), out choice);
            _logger.Information($"User input audiotrack number \"{choice}\"");

            if (!inpCheck)
            {
                _logger.Error("User input is invalid");
                Console.WriteLine("[!] Введенное значение имеет некорректный формат");
                return;
            }
            if (0 >= choice || choice > audios.Count)
            {
                _logger.Error($"User input is out of range [1, {audios.Count}]");
                Console.WriteLine($"[!] Аудиотрека с номером {choice} не существует");
                return;
            }

            await context.PlaylistService.AddAudiotrackToPlaylist(playlistId, audios[choice - 1].Id);
            Console.WriteLine("Аудиотрек добавлен в плейлист");
        }
    }
}