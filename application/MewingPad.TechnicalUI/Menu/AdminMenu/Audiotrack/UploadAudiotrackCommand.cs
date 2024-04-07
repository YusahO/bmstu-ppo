using System.Globalization;
using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.AudiotrackCommands;

public class UploadAudiotrackCommand : Command
{
    private readonly NumberFormatInfo _nfi = new()
    {
        NumberDecimalSeparator = ","
    };

    public override string? Description()
    {
        return "Добавить новый";
    }

    public override async Task Execute(Context context)
    {
        string? title, filepath;

        Console.Write("Введите название аудиотрека: ");
        title = Console.ReadLine();
        if (title is null)
        {
            Console.WriteLine("[!] Введено некорректное значение");
            return;
        }

        Console.Write("Введите длительность: ");
        if (!float.TryParse(Console.ReadLine(), NumberStyles.Any, _nfi, out float duration))
        {
            Console.WriteLine("[!] Введено некорректное значение");
            return;
        }

        Console.Write("Введите путь к файлу аудиотрека: ");
        filepath = Console.ReadLine();
        if (filepath is null)
        {
            Console.WriteLine("[!] Введено некорректное значение");
            return;
        }

        var audio = new Audiotrack(
            Guid.NewGuid(),
            title!,
            (float)duration!,
            context.CurrentUser!.Id,
            Path.GetFileName(filepath!));

        try
        {
            await context.AudiotrackService.CreateAudiotrack(audio);
            Console.WriteLine("Аудиотрек загружен");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[!] {ex.Message}\n");
        }
    }
}

