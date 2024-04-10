using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.CommonCommands.PlaylistCommands;

public class ViewUserPlaylistAudiotracksCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<ViewUserPlaylistAudiotracksCommand>();
    public override string? Description()
    {
        return "Просмотреть аудиотреки в плейлисте";
    }

    public override async Task Execute(Context context)
    {
        Guid playlistId;
        if (context.UserObject is Guid)
        {
            playlistId = (Guid)context.UserObject!;
        }
        else
        {
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
            playlistId = playlists[choice - 1].Id;
        }

        var audios = await context.PlaylistService.GetAllAudiotracksFromPlaylist(playlistId);
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
        context.UserObject = audios;
    }
}