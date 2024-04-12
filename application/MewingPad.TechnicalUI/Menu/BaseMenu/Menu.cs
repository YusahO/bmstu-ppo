using Serilog;

namespace MewingPad.TechnicalUI.BaseMenu;

public class Menu
{
    private readonly ILogger _logger = Log.ForContext<Menu>();
    readonly List<MenuLabel> _labels = [];

    public void AddLabel(MenuLabel label)
    {
        _labels.Add(label);
    }

    public async Task<int> Execute(Context context)
    {
        int iitem = 1;
        foreach (var l in _labels)
        {
            Console.Write($"[{iitem++}] ");
            l.Print();
        }
        Console.WriteLine("[0] Выход");

        Console.Write("Ввод: ");

        var inpCheck = int.TryParse(Console.ReadLine(), out int no);
        _logger.Information($"User input option \"{no}\"");

        if (!inpCheck)
        {
            _logger.Error("User input is invalid");
            Console.WriteLine("[!] Некорректный ввод");
            return -1;
        }

        if (no == 0)
        {
            return 0;
        }
        if (0 > no || no > _labels.Count)
        {
            _logger.Error($"User input option is out of range [1, {_labels.Count}]");
            Console.WriteLine($"[!] Опции под номером {no} не существует");
            return -1;
        }
        await _labels[no - 1].Execute(context);
        return no;
    }
}