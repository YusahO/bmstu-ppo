using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.GuestMenu.AuthActions;

public class SignInUserCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<SignInUserCommand>();
    public override string? Description()
    {
        return "Авторизоваться";
    }

    public override async Task Execute(Context context)
    {
        Console.WriteLine("\n========== Авторизация ==========");

        Console.Write("Введите адрес электронной почты: ");
        var email = Console.ReadLine();
        _logger.Information($"User input email \"{email}\"");

        Console.Write("Введите пароль: ");
        var password = Console.ReadLine();
        _logger.Information("User input password");

        if (email is null && password is null)
        {
            _logger.Information("User input is invalid");
            Console.WriteLine("[!] Неверный ввод\n");
            return;
        }

        try
        {
            var user = await context.OAuthService.SignInUser(email!, password!);
            context.CurrentUser = user;
            Console.WriteLine("Авторизация прошла успешно");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[!] {ex.Message}\n");
        }
    }
}

