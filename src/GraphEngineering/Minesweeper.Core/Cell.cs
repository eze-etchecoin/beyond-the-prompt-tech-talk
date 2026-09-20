namespace Minesweeper.Graph.Core;

/// <summary>
/// A single board cell. State is intentionally simple and observable so the
/// logic is easy to explain live and easy to assert against in tests.
/// </summary>
public sealed class Cell
{
    /// <summary>True when this cell contains a mine.</summary>
    public bool HasMine { get; internal set; }

    /// <summary>True once the player has revealed this cell.</summary>
    public bool IsRevealed { get; internal set; }

    /// <summary>True when the player has flagged this cell as a suspected mine.</summary>
    public bool IsFlagged { get; internal set; }

    /// <summary>
    /// The number of mines in the eight neighbouring cells. Meaningful only when
    /// <see cref="HasMine"/> is false.
    /// </summary>
    public int AdjacentMines { get; internal set; }
}
