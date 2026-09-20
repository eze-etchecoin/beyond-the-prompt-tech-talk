namespace WordWrap.Core;

/// <summary>
/// Provides a small, deterministic word-wrapping algorithm.
///
/// This type is deliberately simple: no logging, no dependency injection,
/// no configuration and no hidden state. It exists to be read and explained
/// live during the talk, and to serve as the "current state" that a future
/// agentic demo will reason about (see prompts/02-context-engineering).
/// </summary>
public sealed class WordWrapper
{
    /// <summary>
    /// Wraps <paramref name="text"/> so that no resulting line is longer than
    /// <paramref name="maximumLineLength"/> characters.
    ///
    /// Rules:
    /// <list type="bullet">
    ///   <item>Words are kept together whenever possible; breaks happen at spaces.</item>
    ///   <item>Runs of whitespace collapse into a single space; no line starts or
    ///   ends with an unnecessary space.</item>
    ///   <item>If a single word is longer than <paramref name="maximumLineLength"/>,
    ///   it cannot fit on one line, so it is hard-split into chunks of at most
    ///   <paramref name="maximumLineLength"/> characters. This is the one case
    ///   where a "word" is broken.</item>
    /// </list>
    /// </summary>
    /// <param name="text">The text to wrap. An empty string yields an empty string.</param>
    /// <param name="maximumLineLength">The maximum number of characters per line. Must be greater than zero.</param>
    /// <returns>The wrapped text, with lines separated by <see cref="Environment.NewLine"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maximumLineLength"/> is less than or equal to zero.</exception>
    public string Wrap(string text, int maximumLineLength)
    {
        ArgumentNullException.ThrowIfNull(text);

        if (maximumLineLength <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maximumLineLength),
                maximumLineLength,
                "The maximum line length must be greater than zero.");
        }

        // Collapse any whitespace run into single spaces and drop empty tokens.
        var words = text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        if (words.Length == 0)
        {
            return string.Empty;
        }

        var lines = new List<string>();
        var currentLine = new System.Text.StringBuilder();

        foreach (var word in words)
        {
            foreach (var piece in SplitLongWord(word, maximumLineLength))
            {
                if (currentLine.Length == 0)
                {
                    currentLine.Append(piece);
                }
                else if (currentLine.Length + 1 + piece.Length <= maximumLineLength)
                {
                    currentLine.Append(' ').Append(piece);
                }
                else
                {
                    lines.Add(currentLine.ToString());
                    currentLine.Clear();
                    currentLine.Append(piece);
                }
            }
        }

        if (currentLine.Length > 0)
        {
            lines.Add(currentLine.ToString());
        }

        return string.Join(Environment.NewLine, lines);
    }

    /// <summary>
    /// Splits a word that is longer than the maximum line length into
    /// consecutive chunks that each fit on a line. A word that already fits
    /// is returned unchanged as a single piece.
    /// </summary>
    private static IEnumerable<string> SplitLongWord(string word, int maximumLineLength)
    {
        if (word.Length <= maximumLineLength)
        {
            yield return word;
            yield break;
        }

        for (var start = 0; start < word.Length; start += maximumLineLength)
        {
            var length = Math.Min(maximumLineLength, word.Length - start);
            yield return word.Substring(start, length);
        }
    }
}
