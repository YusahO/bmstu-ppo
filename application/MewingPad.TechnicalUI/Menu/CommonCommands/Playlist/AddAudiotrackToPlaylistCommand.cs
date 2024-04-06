using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using MewingPad.TechnicalUI.CommonCommands.AudiotrackCommands;

namespace MewingPad.TechnicalUI.CommonCommands.PlaylistCommands;

public class AddAudiotrackToPlaylistCommand : Command
{
    public override string? Description()
    {
        return "Добавить аудиотрек в плейлист";
    }

    public override async Task Execute(Context context)
    {
        context.UserObject = true;
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

        var playlistId = playlists[choice - 1].Id;
        await new ViewAllAudiotracksCommand().Execute(context);
        var audios = (List<Audiotrack>)context.UserObject!;
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

            await context.PlaylistService.AddAudiofileToPlaylist(playlistId, audios[choice - 1].Id);
            Console.WriteLine("Аудиотрек добавлен в плейлист");
        }
    }
}