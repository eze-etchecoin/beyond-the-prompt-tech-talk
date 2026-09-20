using WordWrap.Core;

// Minimal, deterministic console front-end for the word-wrapping demo.
//
// Usage:
//   dotnet run --project src/ContextEngineering/WordWrap.Console -- "<text>" <maxLength>
//
// If no arguments are supplied, sensible defaults are used so the demo can be
// run live with a single command.

const string DefaultText =
    "Beyond the prompt: architectures for AI agents in the software development lifecycle.";
const int DefaultMaxLength = 30;

var text = args.Length >= 1 ? args[0] : DefaultText;

var maximumLineLength = DefaultMaxLength;
if (args.Length >= 2)
{
    if (!int.TryParse(args[1], out maximumLineLength))
    {
        Console.Error.WriteLine($"'{args[1]}' is not a valid integer for the maximum line length.");
        return 1;
    }
}

if (args.Length == 0)
{
    Console.WriteLine("(No arguments supplied - running with demo defaults.)");
    Console.WriteLine($"  text       : \"{DefaultText}\"");
    Console.WriteLine($"  max length : {DefaultMaxLength}");
    Console.WriteLine();
}

try
{
    var wrapper = new WordWrapper();
    var wrapped = wrapper.Wrap(text, maximumLineLength);

    var ruler = new string('-', maximumLineLength);
    Console.WriteLine($"Wrapped at {maximumLineLength} characters:");
    Console.WriteLine(ruler);
    Console.WriteLine(wrapped);
    Console.WriteLine(ruler);
    return 0;
}
catch (ArgumentException ex)
{
    Console.Error.WriteLine($"Invalid input: {ex.Message}");
    return 1;
}
