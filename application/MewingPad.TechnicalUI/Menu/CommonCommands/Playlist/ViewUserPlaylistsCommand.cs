using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.CommonCommands.PlaylistCommands;

public class ViewUserPlaylistsCommand : Command
{
    public override string? Description()
    {
        return "Просмотреть свои плейлисты";
    }

    public override async Task Execute(Context context)
    {
        Console.WriteLine($"\nСписок плейлистов {context.CurrentUser!.Username}");
        var playlists = await context.PlaylistService.GetUserPlaylists(context.CurrentUser!.Id);

        var printFavourites = context.UserObject is null or not bool
                              || (bool)context.UserObject!;
        int istart = printFavourites ? 0 : 1;

        for (int i = istart; i < playlists.Count; ++i)
        {
            Console.WriteLine($"{i}. {playlists[i].Title}");
        }
        context.UserObject = playlists;
    }
}