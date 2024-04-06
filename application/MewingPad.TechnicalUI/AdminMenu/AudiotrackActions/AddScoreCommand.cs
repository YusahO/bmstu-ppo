
using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.AudiotrackActions;

public class AddScoreCommand : Command
{
    public override string? Announce()
    {
        return "Поставить оценку";
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

        Guid audiotrackId = audiotracks[choice - 1].Id;
        var score = (await context.ScoreService.GetAudiotrackScores(audiotrackId))
            .Find(s => s.AudiotrackId == audiotrackId);

        Console.Write("Введите оценку (0 - 5): ");
        if (int.TryParse(Console.ReadLine(), out int value))
        {
            if (score is null)
            {
                score = new Score(audiotracks[choice - 1].Id, context.CurrentUser!.Id, value);
                await context.ScoreService.CreateScore(score);
                Console.WriteLine("Оценка сохранена");
            }
            else
            {
                score.SetValue(value);
                await context.ScoreService.UpdateScore(score);
                Console.WriteLine("Оценка обновлена");
            }
        }
        else
        {
            Console.WriteLine("[!] Введено недопустимое значение оценки");
        }
    }
}

