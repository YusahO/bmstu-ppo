
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.AudiotrackActions;

public class ViewAllAudiotracksCommand : Command
{
    public override string? Announce()
    {
        return "Просмотреть все аудиотреки";
    }

    public override async Task Execute(Context context)
    {
        var audiotracks = await context.AudiotrackService.GetAllAudiotracks();
        if (audiotracks.Count == 0)
        {
            Console.WriteLine("Список аудиофайлов пуст");
        }
        else
        {
            Console.WriteLine();
            int i = 0;
            foreach (var a in audiotracks)
            {
                var scores = await context.ScoreService.GetAudiotrackScores(a.Id);
                var tags = await context.TagService.GetAudiotrackTags(a.Id);
                double meanScore = 0.0f;
                if (scores.Count != 0)
                {
                    meanScore = scores.Average(s => s.Value);
                    meanScore = (meanScore - Math.Floor(meanScore)
                                 < 0.5) ? Math.Floor(meanScore) : Math.Floor(meanScore) + 0.5;
                }
                Console.WriteLine($"{++i}) {a.Title}");
                Console.WriteLine($"   {a.Duration} сек.");
                Console.WriteLine($"   {a.Filepath}");
                Console.WriteLine($"   {meanScore} ★");
                Console.Write("   Теги: ");
                foreach (var t in tags)
                {
                    Console.Write($"{t.Name} ");
                }
                Console.WriteLine();
            }
        }
        context.UserObject = audiotracks;
    }
}

