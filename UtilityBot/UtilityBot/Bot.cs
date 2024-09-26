using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Polling;
using UtilityBot.Controllers;

namespace UtilityBot;

internal class Bot : BackgroundService // this class is a part of NuGet Microsoft.Extensions.Hosting
                                       // use to create constantly-running service
                                       // see ExecuteAsync method
{
    // object TelegramBotClient is used to connect to Tg Bot API:
    // supports sending messages, files, getting updates
    private ITelegramBotClient telegramBotClient;
    private TextMessageController textMessageController;
    private InlineKeaboardController inlineKeaboardController;

    int userChoice = 0;
    public Bot(ITelegramBotClient telegramBotClient, 
               TextMessageController textMessageController,
               InlineKeaboardController inlineKeaboardController)
    {
        this.telegramBotClient = telegramBotClient;
        this.textMessageController = textMessageController;
        this.inlineKeaboardController = inlineKeaboardController;
    }

    /// <summary>
    /// method ExecuteAsync inherited from BackgroundService class
    /// By executing this method bot is allways active
    /// </summary>
    /// <param name="stopToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stopToken)
    {
        telegramBotClient.StartReceiving(ControlUpdatesAsync, ControlError,
                                         new ReceiverOptions() { AllowedUpdates = { } },
                                         cancellationToken: stopToken);
    }

    /// <summary>
    /// Handles different type of updates: Build-in chart Button clicks & user's message
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="update"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    async Task ControlUpdatesAsync(ITelegramBotClient telegramBotClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
    {
        // button click processing. Callback buttons are located inside the chart
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
        {
            //Console.WriteLine("Зарегистрировано нажатие кнопки");
            userChoice = await inlineKeaboardController.ControlActionAsync(update.CallbackQuery, cancellationToken);
        }

        // user's message processing
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            if (update.Message.Type == MessageType.Text)
            {  
                textMessageController.ControlActionAsync(update, userChoice, cancellationToken: cancellationToken);
            }
            else
            {
                Console.WriteLine("Получена не текстовая информация");
                await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                             "Необходимо ввести строку символов",
                                                             cancellationToken: cancellationToken);
            }
        }
    }

    /// <summary>
    /// Provides different errorMessage: 
    /// if exception == null then use ApiRequestException object
    /// else use exception.ToString()
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="exception"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ControlError(ITelegramBotClient telegramBotClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
            $"Telegram API Error \n{apiRequestException.ErrorCode} \n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        Console.WriteLine(errorMessage);
        Thread.Sleep(10000);
        return Task.CompletedTask;
    }
}

