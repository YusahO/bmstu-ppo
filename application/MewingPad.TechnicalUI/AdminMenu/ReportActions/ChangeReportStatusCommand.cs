
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.ReportActions;

public class ChangeReportStatusCommand : Command
{
    public override string? Announce()
    {
        return "Изменить статус жалобы";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}

