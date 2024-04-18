using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.GuestMenu.AuthActions;

public class SignOutUserCommand : Command
{
    public override string? Description()
    {
        return "Выйти из аккаунта";
    }

    public override async Task Execute(Context context)
    {
        context.CurrentUser = null;
        context.UserObject = null;
    }
}

