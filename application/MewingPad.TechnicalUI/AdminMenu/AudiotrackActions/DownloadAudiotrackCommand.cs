
using MewingPad.Common.Entities;
using MewingPad.Utils.AudioManager;
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.AudiotrackActions;

public class DownloadAudiotrackCommand : Command
{
    public override string? Announce()
    {
        return "Скачать";
    }

    public override async Task Execute(Context context)
    {
        await new ViewAllAudiotracksCommand().Execute(context);
        var audiotracks = (List<Audiotrack>)context.UserObject!;
        if (audiotracks.Count == 0)
        {
            return;
        }
        Console.Write("Введите номер аудиотрека: ");
        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > audiotracks.Count)
        {
            Console.WriteLine($"[!] Аудиотрека с номером {choice} не существует");
            return;
        }

        if (!await AudioManager.GetFileAsync(audiotracks[choice - 1].Filepath, "/home/daria/Загрузки"))
        {
            Console.WriteLine($"[!] Не удалось скачать файл");
        }
    }
}

