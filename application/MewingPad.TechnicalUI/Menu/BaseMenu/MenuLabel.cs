using Serilog;

namespace MewingPad.TechnicalUI.BaseMenu;

public class MenuLabel(string announce, List<Command> commands)
{
    private readonly ILogger _logger = Log.ForContext<MenuLabel>();
    readonly string _announce = announce;
    readonly List<Command> _commands = commands;

    public void Print()
    {
        Console.WriteLine(_announce);
        if (_commands.Count <= 1)
        {
            return;
        }
        for (int i = 0; i < _commands.Count; ++i)
        {
            if (i == _commands.Count - 1)
            {
                Console.WriteLine($" └─ {_commands[i].Description()}");
            }
            else
            {
                Console.WriteLine($" ├─ {_commands[i].Description()}");
            }
        }
    }

    public async Task Execute(Context context)
    {
        if (_commands.Count == 1)
        {
            await _commands.First().Execute(context);
            return;
        }

        Console.WriteLine($"============ {_announce} ============");
        int iitem = 1;
        foreach (var c in _commands)
        {
            Console.WriteLine($"{iitem++}. {c.Description()}");
        }
        
        Console.Write("Ввод: ");

        var inpCheck = int.TryParse(Console.ReadLine(), out int no);
        _logger.Information($"User input option \"{no}\"");

        if (!inpCheck)
        {
            _logger.Error("User input is invalid");
            Console.WriteLine("[!] Некорректный ввод");
            return;
        }
        if (0 > no || no > _commands.Count)
        {
            _logger.Error($"User input option is out of range [1, {_commands.Count}]");
            Console.WriteLine($"[!] Опции под номером {no} не существует");
            return;
        }

        await _commands[no - 1].Execute(context);
    }
}