using Minesweeper.Loop.Core;

namespace Minesweeper.Loop.Console;

/// <summary>
/// Maps a <see cref="Cell"/> to its console glyph and color. Kept separate from
/// <c>Program.cs</c> so the mapping can be unit tested without touching the
/// actual console (colors are plain data here; writing them out is the
/// front-end's job).
/// </summary>
public static class CellRenderer
{
    /// <summary>The character to print for a cell, exactly as before coloring was added.</summary>
    public static char GetGlyph(Cell cell, bool revealMines)
    {
        if (cell.IsFlagged && !cell.IsRevealed)
        {
            return 'F';
        }

        if (!cell.IsRevealed)
        {
            return revealMines && cell.HasMine ? '*' : '.';
        }

        if (cell.HasMine)
        {
            return '*';
        }

        return cell.AdjacentMines == 0 ? ' ' : (char)('0' + cell.AdjacentMines);
    }

    /// <summary>
    /// The color to print the glyph in, or null for the terminal's default
    /// foreground (used for blanks, unrevealed cells, and as the fallback when
    /// the terminal does not support color).
    /// </summary>
    public static ConsoleColor? GetColor(Cell cell, bool revealMines)
    {
        if (cell.IsFlagged && !cell.IsRevealed)
        {
            return ConsoleColor.Yellow;
        }

        if (!cell.IsRevealed)
        {
            return revealMines && cell.HasMine ? ConsoleColor.Red : null;
        }

        if (cell.HasMine)
        {
            return ConsoleColor.Red;
        }

        // Classic Minesweeper number colors.
        return cell.AdjacentMines switch
        {
            1 => ConsoleColor.Blue,
            2 => ConsoleColor.Green,
            3 => ConsoleColor.Red,
            4 => ConsoleColor.DarkBlue,
            5 => ConsoleColor.DarkRed,
            6 => ConsoleColor.Cyan,
            7 => ConsoleColor.Black,
            8 => ConsoleColor.Gray,
            _ => null
        };
    }
}
