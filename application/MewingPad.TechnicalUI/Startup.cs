using Microsoft.Extensions.Configuration;
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI;

internal class Startup(IConfiguration config,
               Context context,
               List<Menu> menus)
{
    private readonly IConfiguration _config = config;
    private Context _context = context;

    private readonly Menu _guestMenu = menus[0];
    private readonly Menu _authorizedMenu = menus[1];
    private readonly Menu _adminMenu = menus[2];

    public async Task Run()
    {
        int choice;
        do
        {
            Console.WriteLine();
            if (_context.CurrentUser is null)
            {
                Console.WriteLine("Статус пользователя: Гость");
                choice = await _guestMenu.Execute(_context);
            }
            else if (!_context.CurrentUser.IsAdmin)
            {
                Console.WriteLine($"Статус пользователя: Авторизованный ({_context.CurrentUser.Username})");
                choice = await _authorizedMenu.Execute(_context);
            }
            else
            {
                Console.WriteLine($"Статус пользователя: Администратор ({_context.CurrentUser.Username})");
                choice = await _adminMenu.Execute(_context);
            }
        } while (choice != 0);
    }
}