using MewingPad.Common.Entities;
using MewingPad.Services.OAuthService;
using MewingPad.Utils.PasswordHasher;

namespace MewingPad.TechnicalUI.Actions;

internal class AuthActions(OAuthService oauthService)
{
    private readonly OAuthService _oauthService = oauthService;

    public async Task<User?> RegisterUser(bool makeAdmin = false)
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

        try
        {
            var userId = Guid.NewGuid();
            var user = new User(userId,
                                Guid.NewGuid(),
                                username!,
                                email!,
                                PasswordHasher.HashPassword(password!),
                                makeAdmin);
            await _oauthService.RegisterUser(user);
            Console.WriteLine("Регистрация прошла успешно");
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[!] {ex.Message}\n");
            return null;
        }
    }

    public async Task<User?> SignInUser()
    {
        Console.WriteLine("\n========== Авторизация ==========");

        Console.Write("Введите адрес электронной почты: ");
        var email = Console.ReadLine();

        Console.Write("Введите пароль: ");
        var password = Console.ReadLine();

        if (email is null && password is null)
        {
            Console.WriteLine("[!] Неверный ввод\n");
            return null;
        }

        try
        {
            var user = await _oauthService.SignInUser(email!, password!);
            Console.WriteLine("Авторизация прошла успешно");
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n{ex.Message}\n");
            return null;
        }
    }

    public async Task AddAdmin()
    {
        await RegisterUser(true);
    }
}