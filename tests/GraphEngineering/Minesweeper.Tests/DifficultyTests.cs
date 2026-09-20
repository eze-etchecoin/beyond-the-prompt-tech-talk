using Minesweeper.Graph.Core;
using Xunit;

namespace Minesweeper.Graph.Tests;

public class DifficultyTests
{
    [Theory]
    [InlineData(Difficulty.Beginner, 9, 9, 10)]
    [InlineData(Difficulty.Intermediate, 16, 16, 40)]
    [InlineData(Difficulty.Expert, 16, 30, 99)]
    public void The_three_levels_use_the_classic_dimensions(
        Difficulty difficulty, int rows, int columns, int mines)
    {
        var spec = BoardSpec.FromDifficulty(difficulty);

        Assert.Equal(rows, spec.Rows);
        Assert.Equal(columns, spec.Columns);
        Assert.Equal(mines, spec.MineCount);
    }

    [Fact]
    public void A_board_with_no_safe_cell_is_rejected()
    {
        var spec = new BoardSpec(2, 2, 4);

        Assert.Throws<ArgumentOutOfRangeException>(spec.Validate);
    }

    [Theory]
    [InlineData(0, 5, 1)]
    [InlineData(5, 0, 1)]
    [InlineData(5, 5, 0)]
    public void Invalid_specs_are_rejected(int rows, int columns, int mines)
    {
        var spec = new BoardSpec(rows, columns, mines);

        Assert.Throws<ArgumentOutOfRangeException>(spec.Validate);
    }
}
