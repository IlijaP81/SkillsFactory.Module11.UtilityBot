using Telegram.Bot;

namespace UtilityBot.Controllers;

internal class InlineKeaboardController
{
    private ITelegramBotClient telegramBotClient;
    private (Task, string) outcomingParams;
    public InlineKeaboardController(ITelegramBotClient telegramBotClient )
    {
        this.telegramBotClient = telegramBotClient;
    }
    
    /// <summary>
    /// Process chat buttons clicking 
    /// </summary>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> ControlActionAsync(Telegram.Bot.Types.CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        int strLength = 0;
        Console.WriteLine($"Контроллером {GetType().Name} зафиксировано нажатие на кнопку {callbackQuery.Data}");
        if (callbackQuery == null) return strLength;
        
        switch (callbackQuery.Data)
        {
            // counting number of symbols in string
            case "1":
                {
                    telegramBotClient.SendTextMessageAsync(callbackQuery.From.Id,
                                                           $"Введите символы " +
                                                           $" для подсчета длины строки",
                                                           cancellationToken: cancellationToken);
                    return 1;
                }
            // counting sum of digits in string
            case "2":
                {
                    telegramBotClient.SendTextMessageAsync(callbackQuery.From.Id,
                                                           $"Введите числа " +
                                                           $" для подсчета их суммы" +
                                                           $"через пробел, например, 1 12 25",
                                                           cancellationToken: cancellationToken);
                    return 2;
                }
        }
        return 0;
    }
}
