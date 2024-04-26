namespace SingularisTestTask;

class Program()
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        var host = builder.Build();
        await host.RunAsync();
    }
}