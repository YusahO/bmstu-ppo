using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.CommonCommands.PlaylistCommands;

public class ViewUserPlaylistAudiotracksCommand : Command
{
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
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("[!] Введенное значение имеет некорректный формат");
                context.UserObject = new List<Audiotrack>();
            }
            if (0 >= choice || choice > playlists.Count)
            {
                Console.WriteLine($"[!] Плейлиста с номером {choice} не существует");
                context.UserObject = new List<Audiotrack>();
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