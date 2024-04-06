using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.CommonCommands.PlaylistCommands;

public class RenamePlaylistCommand : Command
{
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
        await context.PlaylistService.UpdateTitle(playlistId, title!);
        Console.WriteLine("Плейлист переименован");
    }
}