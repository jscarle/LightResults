using LightResults;

namespace Testing;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Running.");
        var result = await TestAsync();
        if (result.IsSuccess(out var value, out var error))
            Console.WriteLine($"Success: {value}");
        else
            Console.WriteLine($"Failed: {error.Message}");
    }

    private static async ResultValueValueTask<int> TestAsync()
    {
        Console.Write("Start waiting...");
        await Task.Delay(5000);
        Console.WriteLine(" done.");
        return Result.Ok(42);
    }
}
