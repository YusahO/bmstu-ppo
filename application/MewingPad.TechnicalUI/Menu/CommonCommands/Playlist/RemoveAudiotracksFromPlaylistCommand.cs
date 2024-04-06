using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using MewingPad.TechnicalUI.CommonCommands.AudiotrackCommands;

namespace MewingPad.TechnicalUI.CommonCommands.PlaylistCommands;

public class RemoveAudiotracksFromPlaylistCommand : Command
{
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

            await context.PlaylistService.RemoveAudiofilesFromPlaylist(playlistId, choiceIds);
            Console.WriteLine("Аудиотрек(и) удален(ы) из плейлиста");
        }
    }
}