using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.CommonCommands.PlaylistCommands;

public class DeletePlaylistCommand : Command
{
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

        await context.PlaylistService.DeletePlaylist(playlists[choice].Id);
        Console.WriteLine("Плейлист удален");
    }
}