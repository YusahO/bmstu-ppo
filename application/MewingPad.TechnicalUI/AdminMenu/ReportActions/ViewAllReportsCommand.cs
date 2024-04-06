
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.ReportActions;

public class ViewAllReportsCommand : Command
{
    public override string? Announce()
    {
        return "Просмотреть все жалобы";
    }

    public override Task Execute(Context context)
    {
        throw new NotImplementedException();
    }
}

