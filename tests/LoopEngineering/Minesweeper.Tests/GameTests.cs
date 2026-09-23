using Minesweeper.Loop.Core;
using Xunit;

namespace Minesweeper.Loop.Tests;

// Tests describe observable game behaviour, driven through the public API. They
// use CreateWithMines for a deterministic board so assertions are exact.
public class GameTests
{
    // A 3x3 board with a single mine in the top-left corner.
    // Adjacency:  *  1  0
    //             1  1  0
    //             0  0  0
    private static Game OneMineCorner() =>
        Game.CreateWithMines(new BoardSpec(3, 3, 1), new[] { (0, 0) });

    [Fact]
    public void A_new_random_game_places_exactly_the_requested_number_of_mines()
    {
        var game = Game.NewGame(Difficulty.Intermediate, new Random(1234));

        var mines = 0;
        for (var r = 0; r < game.Rows; r++)
        {
            for (var c = 0; c < game.Columns; c++)
            {
                if (game.GetCell(r, c).HasMine)
                {
                    mines++;
                }
            }
        }

        Assert.Equal(game.MineCount, mines);
    }

    [Fact]
    public void Revealing_a_mine_loses_the_game()
    {
        var game = OneMineCorner();

        game.Reveal(0, 0);

        Assert.Equal(GameState.Lost, game.State);
        Assert.True(game.GetCell(0, 0).IsRevealed);
    }

    [Fact]
    public void Revealing_a_numbered_cell_reveals_only_that_cell()
    {
        var game = OneMineCorner();

        game.Reveal(0, 1); // adjacent to exactly one mine

        Assert.True(game.GetCell(0, 1).IsRevealed);
        Assert.Equal(1, game.GetCell(0, 1).AdjacentMines);
        Assert.False(game.GetCell(2, 2).IsRevealed); // no cascade from a numbered cell
        Assert.Equal(GameState.InProgress, game.State);
    }

    [Fact]
    public void Revealing_an_empty_cell_flood_fills_neighbouring_cells()
    {
        var game = OneMineCorner();

        game.Reveal(2, 2); // an empty cell far from the mine

        // Flood reaches the numbered border cells but never the mine.
        Assert.True(game.GetCell(0, 1).IsRevealed);
        Assert.True(game.GetCell(1, 0).IsRevealed);
        Assert.False(game.GetCell(0, 0).IsRevealed);
    }

    [Fact]
    public void Revealing_every_safe_cell_wins_the_game()
    {
        var game = OneMineCorner();

        game.Reveal(2, 2); // floods all eight safe cells

        Assert.Equal(GameState.Won, game.State);
    }

    [Fact]
    public void Flagging_updates_the_remaining_mine_counter_and_is_reversible()
    {
        var game = OneMineCorner();

        game.ToggleFlag(1, 1);
        Assert.Equal(1, game.FlagCount);
        Assert.Equal(game.MineCount - 1, game.RemainingMines);

        game.ToggleFlag(1, 1);
        Assert.Equal(0, game.FlagCount);
        Assert.Equal(game.MineCount, game.RemainingMines);
    }

    [Fact]
    public void A_flagged_cell_cannot_be_revealed()
    {
        var game = OneMineCorner();

        game.ToggleFlag(0, 0);
        game.Reveal(0, 0);

        Assert.False(game.GetCell(0, 0).IsRevealed);
        Assert.Equal(GameState.InProgress, game.State);
    }

    [Fact]
    public void Playing_after_the_game_is_over_is_not_allowed()
    {
        var game = OneMineCorner();
        game.Reveal(0, 0); // lose

        Assert.Throws<InvalidOperationException>(() => game.Reveal(1, 1));
        Assert.Throws<InvalidOperationException>(() => game.ToggleFlag(1, 1));
    }

    [Fact]
    public void Revealing_off_board_coordinates_is_rejected()
    {
        var game = OneMineCorner();

        Assert.Throws<ArgumentOutOfRangeException>(() => game.Reveal(-1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => game.Reveal(0, 3));
    }

    [Fact]
    public void CreateWithMines_rejects_a_wrong_mine_count()
    {
        Assert.Throws<ArgumentException>(
            () => Game.CreateWithMines(new BoardSpec(3, 3, 2), new[] { (0, 0) }));
    }

    [Fact]
    public void CreateWithMines_rejects_duplicate_mines()
    {
        Assert.Throws<ArgumentException>(
            () => Game.CreateWithMines(new BoardSpec(3, 3, 2), new[] { (0, 0), (0, 0) }));
    }

    [Fact]
    public void A_freshly_created_game_has_no_moves_and_no_elapsed_time()
    {
        var game = OneMineCorner();

        Assert.Equal(0, game.MoveCount);
        Assert.Null(game.Elapsed);
    }

    [Fact]
    public void MoveCount_reflects_the_sequence_of_reveals_and_flags()
    {
        var game = OneMineCorner();

        game.ToggleFlag(0, 0);
        Assert.Equal(1, game.MoveCount);

        game.ToggleFlag(0, 0);
        Assert.Equal(2, game.MoveCount);

        game.Reveal(0, 1);
        Assert.Equal(3, game.MoveCount);
    }

    [Fact]
    public void The_clock_starts_on_the_first_move_not_at_creation()
    {
        var now = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var game = Game.CreateWithMines(new BoardSpec(3, 3, 1), new[] { (0, 0) }, () => now);

        Assert.Null(game.Elapsed);

        game.ToggleFlag(1, 1);
        Assert.Equal(TimeSpan.Zero, game.Elapsed);

        now = now.AddSeconds(5);
        Assert.Equal(TimeSpan.FromSeconds(5), game.Elapsed);
    }

    [Fact]
    public void The_clock_stops_once_the_game_is_won_or_lost()
    {
        var now = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var game = Game.CreateWithMines(new BoardSpec(3, 3, 1), new[] { (0, 0) }, () => now);

        game.Reveal(2, 2); // floods all safe cells and wins
        Assert.Equal(GameState.Won, game.State);

        var elapsedAtWin = game.Elapsed;
        now = now.AddSeconds(30);

        Assert.Equal(elapsedAtWin, game.Elapsed);
    }
}
