namespace Minesweeper.Harness.Core;

/// <summary>The overall state of a game.</summary>
public enum GameState
{
    /// <summary>The game is still being played.</summary>
    InProgress,

    /// <summary>Every safe cell has been revealed. The player won.</summary>
    Won,

    /// <summary>A mine was revealed. The player lost.</summary>
    Lost
}
