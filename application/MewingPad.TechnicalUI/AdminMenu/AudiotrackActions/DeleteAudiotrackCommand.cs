
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.AudiotrackActions;

public class DeleteAudiotrackCommand : Command
{
    public override string? Announce()
    {
        return "Удалить";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}

