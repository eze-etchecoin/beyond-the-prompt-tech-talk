using Minesweeper.Loop.Console;
using Minesweeper.Loop.Core;
using Xunit;

namespace Minesweeper.Loop.Tests;

// Tests the glyph/color mapping in isolation from the actual console, so the
// classic color scheme can be asserted without capturing terminal output.
public class CellRendererTests
{
    private static Game OneMineCorner() =>
        Game.CreateWithMines(new BoardSpec(3, 3, 1), new[] { (0, 0) });

    [Fact]
    public void An_unrevealed_cell_is_a_dot_with_no_color()
    {
        var game = OneMineCorner();
        var cell = game.GetCell(1, 1);

        Assert.Equal('.', CellRenderer.GetGlyph(cell, revealMines: false));
        Assert.Null(CellRenderer.GetColor(cell, revealMines: false));
    }

    [Fact]
    public void A_flagged_cell_is_highlighted_yellow()
    {
        var game = OneMineCorner();
        game.ToggleFlag(1, 1);
        var cell = game.GetCell(1, 1);

        Assert.Equal('F', CellRenderer.GetGlyph(cell, revealMines: false));
        Assert.Equal(ConsoleColor.Yellow, CellRenderer.GetColor(cell, revealMines: false));
    }

    [Fact]
    public void A_revealed_mine_is_highlighted_red()
    {
        var game = OneMineCorner();
        game.Reveal(0, 0); // loses, mine gets revealed

        var cell = game.GetCell(0, 0);

        Assert.Equal('*', CellRenderer.GetGlyph(cell, revealMines: true));
        Assert.Equal(ConsoleColor.Red, CellRenderer.GetColor(cell, revealMines: true));
    }

    [Fact]
    public void Revealed_numbers_use_the_classic_colors()
    {
        var game = OneMineCorner();
        game.Reveal(0, 1); // AdjacentMines == 1

        var cell = game.GetCell(0, 1);

        Assert.Equal('1', CellRenderer.GetGlyph(cell, revealMines: false));
        Assert.Equal(ConsoleColor.Blue, CellRenderer.GetColor(cell, revealMines: false));
    }

    [Fact]
    public void A_revealed_empty_cell_is_blank_with_no_color()
    {
        var game = OneMineCorner();
        game.Reveal(2, 2); // floods, includes a zero-adjacency cell

        var cell = game.GetCell(2, 2);

        Assert.Equal(' ', CellRenderer.GetGlyph(cell, revealMines: false));
        Assert.Null(CellRenderer.GetColor(cell, revealMines: false));
    }
}
