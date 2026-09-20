namespace Minesweeper.Harness.Core;

/// <summary>
/// The three classic Minesweeper difficulty levels.
/// </summary>
public enum Difficulty
{
    Beginner,
    Intermediate,
    Expert
}

/// <summary>
/// Describes the shape of a board: its dimensions and mine count.
/// The three canonical presets match the classic Windows Minesweeper.
/// </summary>
/// <param name="Rows">Number of rows. Must be greater than zero.</param>
/// <param name="Columns">Number of columns. Must be greater than zero.</param>
/// <param name="MineCount">Number of mines. Must be at least one and leave at least one safe cell.</param>
public readonly record struct BoardSpec(int Rows, int Columns, int MineCount)
{
    /// <summary>Beginner: 9 x 9 with 10 mines.</summary>
    public static BoardSpec Beginner { get; } = new(9, 9, 10);

    /// <summary>Intermediate: 16 x 16 with 40 mines.</summary>
    public static BoardSpec Intermediate { get; } = new(16, 16, 40);

    /// <summary>Expert: 16 x 30 with 99 mines.</summary>
    public static BoardSpec Expert { get; } = new(16, 30, 99);

    /// <summary>Total number of cells on the board.</summary>
    public int CellCount => Rows * Columns;

    /// <summary>Resolves a <see cref="Difficulty"/> into its canonical <see cref="BoardSpec"/>.</summary>
    public static BoardSpec FromDifficulty(Difficulty difficulty) => difficulty switch
    {
        Difficulty.Beginner => Beginner,
        Difficulty.Intermediate => Intermediate,
        Difficulty.Expert => Expert,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, "Unknown difficulty.")
    };

    /// <summary>
    /// Validates that the spec describes a playable board. A board must have
    /// positive dimensions, at least one mine, and at least one safe cell.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any dimension or the mine count is invalid.</exception>
    public void Validate()
    {
        if (Rows <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(Rows), Rows, "Rows must be greater than zero.");
        }

        if (Columns <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(Columns), Columns, "Columns must be greater than zero.");
        }

        if (MineCount < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(MineCount), MineCount, "There must be at least one mine.");
        }

        if (MineCount >= CellCount)
        {
            throw new ArgumentOutOfRangeException(
                nameof(MineCount),
                MineCount,
                "The mine count must leave at least one safe cell.");
        }
    }
}
