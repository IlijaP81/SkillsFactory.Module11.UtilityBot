using Microsoft.Extensions.Hosting;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using UtilityBot;
using UtilityBot.Controllers;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;
        
        var host =              // this object is used for application lifecycle
            new HostBuilder()
                             .ConfigureServices((hostContext, services) // add services to the container
                           => ConfigureServices(services))              // define configuration
                             .UseConsoleLifetime()                      // application is active in console
                             .Build();
        Console.WriteLine("Сервис запущен");
        await host.RunAsync();                                          // start service
    }
        
    private static void ConfigureServices(IServiceCollection services)
    {
        // subscribe controllers
        services.AddTransient<TextMessageController>();
        services.AddTransient<InlineKeaboardController>();
        
        // registrate TelegramBotClient-object with connection token
        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(
            "7782920839:AAE9clGPcdqUKPGDc_P3cP5nJhi7fZ2X50M")); // special_utility_bot
        
        // registrate constantly active bot service
        services.AddHostedService<Bot>();
    }
}