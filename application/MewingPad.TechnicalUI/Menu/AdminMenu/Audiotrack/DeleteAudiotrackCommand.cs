using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
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

        var audio = audiotracks[choice - 1];
        try
        {
            await context.AudiotrackService.DeleteAudiotrack(audio.Id);
            Console.WriteLine("Аудиотрек удален");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[!] {ex.Message}\n");
        }
    }
}

