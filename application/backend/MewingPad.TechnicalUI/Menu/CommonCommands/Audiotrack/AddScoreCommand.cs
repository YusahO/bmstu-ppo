using MewingPad.Common.Entities;
using MewingPad.TechnicalUI.BaseMenu;
using Serilog;

namespace MewingPad.TechnicalUI.CommonCommands.AudiotrackCommands;

public class AddScoreCommand : Command
{
    private readonly ILogger _logger = Log.ForContext<AddScoreCommand>();
    public override string? Description()
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

        var inpCheck = int.TryParse(Console.ReadLine(), out int choice);
        _logger.Information($"User input audiotrack number \"{choice}\"");

        if (!inpCheck)
        {
            _logger.Error("User input is invalid");
            Console.WriteLine("[!] Введенное значение имеет некорректный формат");
            return;
        }
        if (0 >= choice || choice > audiotracks.Count)
        {
            _logger.Error($"User input is out of range [1, {audiotracks.Count}]");
            Console.WriteLine($"[!] Аудиотрека с номером {choice} не существует");
            return;
        }

        Guid audiotrackId = audiotracks[choice - 1].Id;
        var score = (await context.ScoreService.GetAudiotrackScores(audiotrackId))
            .Find(s => s.AudiotrackId == audiotrackId);

        Console.Write("Введите оценку (0 - 5): ");

        inpCheck = int.TryParse(Console.ReadLine(), out int value);
        _logger.Information($"User input score value \"{value}\"");

        if (!inpCheck)
        {
            _logger.Error("User input is invalid");
            Console.WriteLine("[!] Введено недопустимое значение оценки");
            return;
        }

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
}

