using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using MewingPad.Utils.AudioManager;
using MewingPad.TechnicalUI.CommonCommands.AudiotrackCommands;

namespace MewingPad.TechnicalUI.AdminMenu.AudiotrackCommands;

public class DeleteAudiotrackCommand : Command
{
    public override string? Description()
    {
        return "Удалить";
    }

    public override async Task Execute(Context context)
    {
        await new ViewAllAudiotracksCommand().Execute(context);
        var audiotracks = (List<Common.Entities.Audiotrack>)context.UserObject!;
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

        var audio = audiotracks[choice - 1];
        if (!await AudioManager.DeleteFileAsync(audio.Filepath))
        {
            Console.WriteLine("[!] Не удалось удалить файл");
            return;
        }
        await context.AudiotrackService.DeleteAudiotrack(audio.Id);
        Console.WriteLine("Аудиотрек удален");
    }
}

