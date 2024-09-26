using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using UtilityBot.Services;

namespace UtilityBot.Controllers;

// serving incoming text requests
internal class TextMessageController
{
    private ITelegramBotClient telegramBotClient;
    public TextMessageController (ITelegramBotClient telegramBotClient )
    {
        this.telegramBotClient = telegramBotClient;
    }
    
    /// <summary>
    /// Process chat's menu commands and user's messages
    /// </summary>
    /// <param name="update"></param>
    /// <param name="userChoice"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task ControlActionAsync(Update update, int userChoice, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Контроллером {GetType().Name} получено сообщение {update.Message.Text}");
        
        // start bot
        if (update.Message.Text == "/start")
        {
            // define buttons
            var buttons = new List<InlineKeyboardButton[]>();
            buttons.Add([ InlineKeyboardButton.WithCallbackData("Количество символов", $"1"),
                          InlineKeyboardButton.WithCallbackData("Сумма чисел", $"2") ]);
            
            // send buttons to bot
            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                         $"<b> Бот позволяет подсчитать количество символов в " +
                                                         $"строке или сумму чисел  в строке  </b> {Environment.NewLine}" +
                                                         $" Выберите требуемый вариант {Environment.NewLine}",
                                                         cancellationToken: cancellationToken,
                                                         parseMode: ParseMode.Html,                       // layout html
                                                         replyMarkup: new InlineKeyboardMarkup(buttons)); // drawing buttons in chat
        }
        
        // stop bot
        else if (update.Message.Text == "/stop")
        {
            Console.WriteLine("Сервис остановлен"); 
            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                         "Работа завершена",
                                                         cancellationToken: cancellationToken);
            Environment.Exit(0);
        }
        
        // others
        else
        {
            Calculations calculations = new Calculations();
            
            // calculate number of symbols
            if (userChoice == 1)
            {
                int numberOfSymbols = calculations.CalculateNumberOfSymbols(update.Message.Text);
                await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, 
                                                             $"Количество символов в строке {numberOfSymbols}",
                                                             cancellationToken: cancellationToken);
            }
            
            // calculate sum of digits
            else if (userChoice == 2)
            {
                int sum = calculations.CalculateSum(update.Message.Text);
                await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                             $"Сумма чисел в строке {sum}",
                                                             cancellationToken: cancellationToken);
            }

            // no choice from user
            else
            {
                await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                             "Выберите действие",
                                                             cancellationToken: cancellationToken);
            }
        }
    }                      
}