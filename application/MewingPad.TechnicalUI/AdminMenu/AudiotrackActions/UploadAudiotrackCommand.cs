
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.AudiotrackActions;

public class UploadAudiotrackCommand : Command
{
    public override string? Announce()
    {
        return "Добавить новый";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}

