using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.GuestMenu.AuthActions;

public class RegisterUserCommand : Command
{
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
            if (username is null || username.Length < 3)
            {
                isIncorrect = true;
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
            if (password is null || password.Length < 3)
            {
                isIncorrect = true;
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

