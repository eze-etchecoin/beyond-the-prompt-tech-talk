namespace Minesweeper.Loop.Core;

/// <summary>
/// A single game of Minesweeper. Holds the board, applies the rules and tracks
/// the win/lose state.
///
/// This type deliberately keeps all logic in one readable place: mine placement,
/// adjacency counting, revealing (with flood fill of empty regions) and flagging.
/// No logging, no dependency injection, no configuration - it is meant to be
/// explained live and driven either by the console front-end or by tests.
/// </summary>
public sealed class Game
{
    private readonly Cell[,] _cells;

    /// <summary>Number of rows on the board.</summary>
    public int Rows { get; }

    /// <summary>Number of columns on the board.</summary>
    public int Columns { get; }

    /// <summary>Total number of mines on the board.</summary>
    public int MineCount { get; }

    /// <summary>The current state of the game.</summary>
    public GameState State { get; private set; } = GameState.InProgress;

    /// <summary>
    /// Mines minus placed flags. A simple counter to help the player; it can go
    /// negative if the player over-flags, which mirrors the classic game.
    /// </summary>
    public int RemainingMines => MineCount - FlagCount;

    /// <summary>The number of cells currently flagged.</summary>
    public int FlagCount { get; private set; }

    /// <summary>
    /// The number of plays (reveals and flag toggles) made so far. Does not
    /// count attempts on off-board coordinates or on a finished game, since
    /// those are rejected before they reach the board.
    /// </summary>
    public int MoveCount { get; private set; }

    /// <summary>
    /// Time elapsed since the first play. Null until the first reveal or flag
    /// toggle; stops advancing once the game is won or lost.
    /// </summary>
    public TimeSpan? Elapsed => _startedAt is null ? null : (_endedAt ?? _clock()) - _startedAt.Value;

    private readonly Func<DateTimeOffset> _clock;
    private DateTimeOffset? _startedAt;
    private DateTimeOffset? _endedAt;

    private Game(BoardSpec spec, Func<DateTimeOffset>? clock)
    {
        spec.Validate();
        Rows = spec.Rows;
        Columns = spec.Columns;
        MineCount = spec.MineCount;
        _clock = clock ?? (() => DateTimeOffset.UtcNow);

        _cells = new Cell[Rows, Columns];
        for (var r = 0; r < Rows; r++)
        {
            for (var c = 0; c < Columns; c++)
            {
                _cells[r, c] = new Cell();
            }
        }
    }

    /// <summary>Creates a game for one of the three canonical difficulty levels.</summary>
    public static Game NewGame(Difficulty difficulty, Random? random = null, Func<DateTimeOffset>? clock = null) =>
        NewGame(BoardSpec.FromDifficulty(difficulty), random, clock);

    /// <summary>
    /// Creates a game for an arbitrary board spec, placing mines at random.
    /// Pass a seeded <see cref="Random"/> for a reproducible board (useful for
    /// live demos and tests). Pass a <paramref name="clock"/> to control the
    /// time source behind <see cref="Elapsed"/> (useful for tests).
    /// </summary>
    public static Game NewGame(BoardSpec spec, Random? random = null, Func<DateTimeOffset>? clock = null)
    {
        var game = new Game(spec, clock);
        game.PlaceMinesRandomly(random ?? new Random());
        game.ComputeAdjacency();
        return game;
    }

    /// <summary>
    /// Creates a game with mines at explicit coordinates. Deterministic and handy
    /// for tests and worked examples.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when a coordinate is off-board or duplicated, or when the count does not match the spec.</exception>
    public static Game CreateWithMines(
        BoardSpec spec, IEnumerable<(int Row, int Column)> mines, Func<DateTimeOffset>? clock = null)
    {
        ArgumentNullException.ThrowIfNull(mines);
        var game = new Game(spec, clock);

        var placed = 0;
        foreach (var (row, column) in mines)
        {
            if (!game.InBounds(row, column))
            {
                throw new ArgumentException($"Mine at ({row}, {column}) is outside the board.", nameof(mines));
            }

            if (game._cells[row, column].HasMine)
            {
                throw new ArgumentException($"Duplicate mine at ({row}, {column}).", nameof(mines));
            }

            game._cells[row, column].HasMine = true;
            placed++;
        }

        if (placed != spec.MineCount)
        {
            throw new ArgumentException(
                $"Expected {spec.MineCount} mines but {placed} were provided.", nameof(mines));
        }

        game.ComputeAdjacency();
        return game;
    }

    /// <summary>Gets the cell at the given coordinates.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the coordinates are off-board.</exception>
    public Cell GetCell(int row, int column)
    {
        if (!InBounds(row, column))
        {
            throw new ArgumentOutOfRangeException(
                nameof(row), (row, column), "The coordinates are outside the board.");
        }

        return _cells[row, column];
    }

    /// <summary>
    /// Reveals the cell at the given coordinates.
    ///
    /// Rules:
    /// <list type="bullet">
    ///   <item>Revealing a flagged or already-revealed cell does nothing.</item>
    ///   <item>Revealing a mine ends the game as <see cref="GameState.Lost"/>.</item>
    ///   <item>Revealing a cell with zero adjacent mines flood-fills its empty neighbours.</item>
    ///   <item>Revealing the last safe cell ends the game as <see cref="GameState.Won"/>.</item>
    /// </list>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the coordinates are off-board.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the game is already over.</exception>
    public void Reveal(int row, int column)
    {
        EnsureInProgress();

        if (!InBounds(row, column))
        {
            throw new ArgumentOutOfRangeException(
                nameof(row), (row, column), "The coordinates are outside the board.");
        }

        RecordMove();

        var cell = _cells[row, column];
        if (cell.IsRevealed || cell.IsFlagged)
        {
            return;
        }

        if (cell.HasMine)
        {
            cell.IsRevealed = true;
            State = GameState.Lost;
            StopClockIfGameOver();
            return;
        }

        FloodReveal(row, column);

        if (AllSafeCellsRevealed())
        {
            State = GameState.Won;
        }

        StopClockIfGameOver();
    }

    /// <summary>
    /// Toggles a flag on the cell at the given coordinates. Revealed cells cannot
    /// be flagged.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the coordinates are off-board.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the game is already over.</exception>
    public void ToggleFlag(int row, int column)
    {
        EnsureInProgress();

        if (!InBounds(row, column))
        {
            throw new ArgumentOutOfRangeException(
                nameof(row), (row, column), "The coordinates are outside the board.");
        }

        RecordMove();

        var cell = _cells[row, column];
        if (cell.IsRevealed)
        {
            return;
        }

        cell.IsFlagged = !cell.IsFlagged;
        FlagCount += cell.IsFlagged ? 1 : -1;
    }

    private void RecordMove()
    {
        MoveCount++;
        _startedAt ??= _clock();
    }

    private void StopClockIfGameOver()
    {
        if (State != GameState.InProgress)
        {
            _endedAt ??= _clock();
        }
    }

    private void EnsureInProgress()
    {
        if (State != GameState.InProgress)
        {
            throw new InvalidOperationException("The game is already over.");
        }
    }

    private bool InBounds(int row, int column) =>
        row >= 0 && row < Rows && column >= 0 && column < Columns;

    private void PlaceMinesRandomly(Random random)
    {
        // Reservoir-free approach: shuffle the first MineCount picks over all cells.
        var placed = 0;
        while (placed < MineCount)
        {
            var row = random.Next(Rows);
            var column = random.Next(Columns);
            if (_cells[row, column].HasMine)
            {
                continue;
            }

            _cells[row, column].HasMine = true;
            placed++;
        }
    }

    private void ComputeAdjacency()
    {
        for (var r = 0; r < Rows; r++)
        {
            for (var c = 0; c < Columns; c++)
            {
                if (_cells[r, c].HasMine)
                {
                    continue;
                }

                _cells[r, c].AdjacentMines = CountAdjacentMines(r, c);
            }
        }
    }

    private int CountAdjacentMines(int row, int column)
    {
        var count = 0;
        foreach (var (r, c) in Neighbours(row, column))
        {
            if (_cells[r, c].HasMine)
            {
                count++;
            }
        }

        return count;
    }

    private void FloodReveal(int startRow, int startColumn)
    {
        // Iterative flood fill so a large empty region cannot overflow the stack.
        var queue = new Queue<(int Row, int Column)>();
        queue.Enqueue((startRow, startColumn));

        while (queue.Count > 0)
        {
            var (row, column) = queue.Dequeue();
            var cell = _cells[row, column];

            if (cell.IsRevealed || cell.IsFlagged || cell.HasMine)
            {
                continue;
            }

            cell.IsRevealed = true;

            // Only empty cells (no adjacent mines) cascade to their neighbours.
            if (cell.AdjacentMines == 0)
            {
                foreach (var neighbour in Neighbours(row, column))
                {
                    queue.Enqueue(neighbour);
                }
            }
        }
    }

    private bool AllSafeCellsRevealed()
    {
        for (var r = 0; r < Rows; r++)
        {
            for (var c = 0; c < Columns; c++)
            {
                var cell = _cells[r, c];
                if (!cell.HasMine && !cell.IsRevealed)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private IEnumerable<(int Row, int Column)> Neighbours(int row, int column)
    {
        for (var dr = -1; dr <= 1; dr++)
        {
            for (var dc = -1; dc <= 1; dc++)
            {
                if (dr == 0 && dc == 0)
                {
                    continue;
                }

                var r = row + dr;
                var c = column + dc;
                if (InBounds(r, c))
                {
                    yield return (r, c);
                }
            }
        }
    }
}
