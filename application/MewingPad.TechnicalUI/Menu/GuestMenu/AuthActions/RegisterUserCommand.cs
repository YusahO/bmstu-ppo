using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.GuestMenu.AuthActions;

public class RegisterUserCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<SignInUserCommand>();
    public override string? Description()
    {
        return "Зарегистрироваться";
    }

    public override async Task Execute(Context context)
    {
        Console.WriteLine("\n========== Регистрация ==========");

        string? username, password, passwordVerify, email;
        bool isIncorrect;

        do
        {
            Console.Write("Введите имя пользователя: ");

            username = Console.ReadLine();
            _logger.Information($"User input username \"{username}\"");

            if (username is null || username.Length < 3)
            {
                isIncorrect = true;
                _logger.Warning("User input is invalid");
                Console.WriteLine("[!] Имя пользователя должно содержать более 2 символов");
            }
            else
            {
                isIncorrect = false;
            }
        } while (isIncorrect);

        do
        {
            Console.Write("Введите пароль: ");

            password = Console.ReadLine();
            _logger.Information($"User input password");

            if (password is null || password.Length < 3)
            {
                isIncorrect = true;
                _logger.Warning("User input is invalid");
                Console.WriteLine("[!] Пароль должен содержать 8 символов и более");
            }
            else
            {
                isIncorrect = false;
            }
        } while (isIncorrect);

        do
        {
            Console.Write("-> Подтвердите пароль: ");
            passwordVerify = Console.ReadLine();
        } while (password != passwordVerify);
        _logger.Information($"User successfully verified password");

        do
        {
            Console.Write("Введите адрес электронной почты: ");
            email = Console.ReadLine();
            if (email is null || !email.Contains('@') || !email.Contains('.'))
            {
                isIncorrect = true;
                Console.WriteLine("[!] Введенный адрес имеет некорректный формат");
            }
            else
            {
                isIncorrect = false;
            }
        } while (isIncorrect);

        bool makeAdmin = context.CurrentUser is not null && context.CurrentUser.IsAdmin;
        _logger.Information($"User is administrator");

        try
        {
            var userId = Guid.NewGuid();
            var user = new User(userId,
                                Guid.NewGuid(),
                                username!,
                                email!,
                                password!,
                                makeAdmin);
            await context.OAuthService.RegisterUser(user);
            context.CurrentUser = user;
            Console.WriteLine("Регистрация прошла успешно");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[!] {ex.Message}\n");
            return;
        }
    }
}

