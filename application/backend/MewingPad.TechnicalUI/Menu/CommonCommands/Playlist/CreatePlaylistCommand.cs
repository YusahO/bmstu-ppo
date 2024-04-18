using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.CommonCommands.PlaylistCommands;

public class CreatePlaylistCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<CreatePlaylistCommand>();
    public override string? Description()
    {
        return "Создать";
    }

    public override async Task Execute(Context context)
    {
        context.UserObject = true;
        await new ViewUserPlaylistsCommand().Execute(context);
        var playlists = (List<Playlist>)context.UserObject!;

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

        var playlist = new Playlist(Guid.NewGuid(), title!, context.CurrentUser!.Id);
        await context.PlaylistService.CreatePlaylist(playlist);
        Console.WriteLine("Плейлист создан");
    }
}