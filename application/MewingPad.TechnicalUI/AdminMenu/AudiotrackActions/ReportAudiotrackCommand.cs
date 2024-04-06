
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.AudiotrackActions;

public class ReportAudiotrackCommand : Command
{
    public override string? Announce()
    {
        return "Пожаловаться";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}

