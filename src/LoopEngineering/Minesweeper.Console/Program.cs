using Minesweeper.Loop.Core;

// Minimal, deterministic console front-end for the Minesweeper demo.
//
// Usage:
//   dotnet run --project src/Minesweeper/Minesweeper.Console -- [difficulty] [seed]
//     difficulty : b | i | e   (beginner | intermediate | expert). Default: b
//     seed       : optional integer for a reproducible board (great for demos)
//
// In-game commands:
//   r <row> <col>   reveal a cell
//   f <row> <col>   toggle a flag
//   q               quit
//
// Rows and columns are 0-based and shown as headers around the board.

var difficulty = ParseDifficulty(args.Length >= 1 ? args[0] : "b");

Random? random = null;
if (args.Length >= 2 && int.TryParse(args[1], out var seed))
{
    random = new Random(seed);
    Console.WriteLine($"(Using fixed seed {seed} for a reproducible board.)");
}

var game = Game.NewGame(difficulty, random);

Console.WriteLine($"Minesweeper - {difficulty} ({game.Rows}x{game.Columns}, {game.MineCount} mines)");
Console.WriteLine("Commands: 'r <row> <col>' reveal, 'f <row> <col>' flag, 'q' quit.");
Console.WriteLine();

while (game.State == GameState.InProgress)
{
    Render(game, revealMines: false);
    Console.Write($"Mines left: {game.RemainingMines}  > ");

    var line = Console.ReadLine();
    if (line is null)
    {
        // No more input (e.g. piped/non-interactive). Exit cleanly.
        Console.WriteLine();
        return 0;
    }

    var command = ParseCommand(line);
    if (command is null)
    {
        Console.WriteLine("Unrecognised command. Use 'r <row> <col>', 'f <row> <col>' or 'q'.");
        continue;
    }

    if (command.Value.Quit)
    {
        Console.WriteLine("Bye!");
        return 0;
    }

    try
    {
        if (command.Value.Flag)
        {
            game.ToggleFlag(command.Value.Row, command.Value.Column);
        }
        else
        {
            game.Reveal(command.Value.Row, command.Value.Column);
        }
    }
    catch (ArgumentOutOfRangeException)
    {
        Console.WriteLine("Those coordinates are off the board. Try again.");
    }
}

Console.WriteLine();
Render(game, revealMines: true);
Console.WriteLine(game.State == GameState.Won ? "You win! Every safe cell is clear." : "Boom! You hit a mine.");
return 0;

static Difficulty ParseDifficulty(string value) => value.Trim().ToLowerInvariant() switch
{
    "b" or "beginner" => Difficulty.Beginner,
    "i" or "intermediate" => Difficulty.Intermediate,
    "e" or "expert" => Difficulty.Expert,
    _ => Difficulty.Beginner
};

static (bool Quit, bool Flag, int Row, int Column)? ParseCommand(string line)
{
    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (parts.Length == 0)
    {
        return null;
    }

    var verb = parts[0].ToLowerInvariant();
    if (verb is "q" or "quit")
    {
        return (Quit: true, Flag: false, Row: 0, Column: 0);
    }

    if (parts.Length != 3 || (verb is not "r" and not "f"))
    {
        return null;
    }

    if (!int.TryParse(parts[1], out var row) || !int.TryParse(parts[2], out var column))
    {
        return null;
    }

    return (Quit: false, Flag: verb == "f", Row: row, Column: column);
}

static void Render(Game game, bool revealMines)
{
    // Column headers (single digit, wrapping every 10 columns).
    Console.Write("    ");
    for (var c = 0; c < game.Columns; c++)
    {
        Console.Write(c % 10);
    }

    Console.WriteLine();
    Console.WriteLine("   +" + new string('-', game.Columns) + "+");

    for (var r = 0; r < game.Rows; r++)
    {
        Console.Write($"{r,2} |");
        for (var c = 0; c < game.Columns; c++)
        {
            WriteGlyph(game.GetCell(r, c), revealMines);
        }

        Console.WriteLine("|");
    }

    Console.WriteLine("   +" + new string('-', game.Columns) + "+");
}

// Colours numbers the classic Minesweeper way and highlights flags/mines.
// Degrades to plain characters when output is redirected/piped (colour codes
// would otherwise corrupt non-interactive output).
static void WriteGlyph(Cell cell, bool revealMines)
{
    var (glyph, color) = Describe(cell, revealMines);
    if (color is null || Console.IsOutputRedirected)
    {
        Console.Write(glyph);
        return;
    }

    var original = Console.ForegroundColor;
    Console.ForegroundColor = color.Value;
    Console.Write(glyph);
    Console.ForegroundColor = original;
}

static (char Glyph, ConsoleColor? Color) Describe(Cell cell, bool revealMines)
{
    if (cell.IsFlagged && !cell.IsRevealed)
    {
        return ('F', ConsoleColor.Yellow);
    }

    if (!cell.IsRevealed)
    {
        return revealMines && cell.HasMine ? ('*', ConsoleColor.Red) : ('.', null);
    }

    if (cell.HasMine)
    {
        return ('*', ConsoleColor.Red);
    }

    if (cell.AdjacentMines == 0)
    {
        return (' ', null);
    }

    return ((char)('0' + cell.AdjacentMines), NumberColor(cell.AdjacentMines));
}

// Classic Minesweeper number colours (1=blue .. 8=dark gray).
static ConsoleColor NumberColor(int adjacentMines) => adjacentMines switch
{
    1 => ConsoleColor.Blue,
    2 => ConsoleColor.Green,
    3 => ConsoleColor.Red,
    4 => ConsoleColor.DarkBlue,
    5 => ConsoleColor.DarkRed,
    6 => ConsoleColor.Cyan,
    7 => ConsoleColor.Black,
    _ => ConsoleColor.DarkGray
};
