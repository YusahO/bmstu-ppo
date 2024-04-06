using MewingPad.TechnicalUI.BaseMenu;
using MewingPad.TechnicalUI.CommonCommands.AudiotrackCommands;
using MewingPad.TechnicalUI.CommonCommands.Search;
using MewingPad.TechnicalUI.GuestMenu.AuthActions;

namespace MewingPad.TechnicalUI.GuestMenu;

public class GuestMenuBuilder : MenuBuilder
{
    public override Menu BuildMenu()
    {
        Menu menu = new();
        menu.AddLabel(new("Зарегистрироваться",
        [
            new RegisterUserCommand()
        ]));
        menu.AddLabel(new("Авторизоваться",
        [
            new SignInUserCommand()
        ]));
        menu.AddLabel(new("Поиск аудиотреков",
        [
            new SearchByTitleCommand(),
            new SearchByTagsCommand()
        ]));
        menu.AddLabel(new("Скачать аудиотрек",
        [
            new DownloadAudiotrackCommand()
        ]));
        return menu;
    }
}