namespace MewingPad.TechnicalUI.BaseMenu;

abstract public class Command
{
    abstract public Task Execute(Context context);
    abstract public string? Description();
}