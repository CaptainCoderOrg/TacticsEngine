using Model;

Console.Clear();
Console.WriteLine("Welcome to Adder!");
int x = Prompt<int>("Enter an integer", int.TryParse);
int y = Prompt<int>("Enter an integer", int.TryParse);
int result = MathUtils.Add(x, y);
Console.WriteLine($"The result is: {result}");

static T Prompt<T>(string prompt, TryParse<T> tryParse)
{
    Console.WriteLine(prompt);
    Console.Write(" > ");
    string input = Console.ReadLine()!;
    if (tryParse.Invoke(input, out T result))
    {
        return result;
    }
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Invalid input");
    Console.ResetColor();
    return Prompt(prompt, tryParse);
}

delegate bool TryParse<T>(string toParse, out T result);