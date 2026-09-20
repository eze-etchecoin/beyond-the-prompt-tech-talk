using WordWrap.Core;
using Xunit;

namespace WordWrap.Tests;

// Tests describe observable behaviour of the word-wrapping algorithm, not its
// internal implementation. They double as living documentation for the demo.
public class WordWrapperTests
{
    private readonly WordWrapper _wrapper = new();

    [Fact]
    public void Text_shorter_than_the_maximum_is_returned_on_a_single_line()
    {
        var result = _wrapper.Wrap("hello world", 20);

        Assert.Equal("hello world", result);
    }

    [Fact]
    public void Text_longer_than_the_maximum_is_split_across_several_lines()
    {
        var result = _wrapper.Wrap("the quick brown fox jumps", 10);

        var lines = SplitLines(result);
        Assert.All(lines, line => Assert.True(line.Length <= 10));
        Assert.Equal(new[] { "the quick", "brown fox", "jumps" }, lines);
    }

    [Fact]
    public void Repeated_whitespace_collapses_and_lines_are_trimmed()
    {
        var result = _wrapper.Wrap("  the   quick    brown  ", 20);

        Assert.Equal("the quick brown", result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void An_invalid_maximum_line_length_is_rejected(int invalidLength)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => _wrapper.Wrap("anything", invalidLength));
    }

    [Fact]
    public void Empty_input_produces_empty_output()
    {
        var result = _wrapper.Wrap(string.Empty, 10);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Whitespace_only_input_produces_empty_output()
    {
        var result = _wrapper.Wrap("     ", 10);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void A_word_longer_than_the_maximum_is_hard_split_into_chunks()
    {
        // "abcdefghij" is 10 chars; with a max of 4 it cannot fit on one line,
        // so it is broken into chunks of at most 4 characters.
        var result = _wrapper.Wrap("abcdefghij", 4);

        var lines = SplitLines(result);
        Assert.All(lines, line => Assert.True(line.Length <= 4));
        Assert.Equal(new[] { "abcd", "efgh", "ij" }, lines);
    }

    [Fact]
    public void An_over_long_word_wraps_together_with_surrounding_words()
    {
        var result = _wrapper.Wrap("hi supercalifragilistic bye", 8);

        var lines = SplitLines(result);
        Assert.All(lines, line => Assert.True(line.Length <= 8));
    }

    private static string[] SplitLines(string text) =>
        text.Split(Environment.NewLine);
}
