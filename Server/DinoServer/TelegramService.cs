using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Collections.Concurrent;
using System.Text;
using DinoServer.Users;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Polling;

namespace DinoServer;

public static class TelegramService
{
    private static string token = "8282986498:AAGSV11RSyUkl8uGWPTdh8oRelIJwEvdbSg";
    private static TelegramBotClient? _bot;
    private static readonly ConcurrentDictionary<long, bool> _chatIds = new(); // потокобезопасное хранение chatId
    private static CancellationTokenSource? _cts;
    private static IDbContextFactory<UserContext> _contextFactory;

    // Инициализация бота
    public static void Initialize(IDbContextFactory<UserContext> contextFactory)
    {
        _contextFactory = contextFactory;
        
        if (_bot != null) return; // уже инициализирован

        _bot = new TelegramBotClient(token);
        _cts = new CancellationTokenSource();

        _bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions { AllowedUpdates = Array.Empty<UpdateType>() },
            cancellationToken: _cts.Token
        );
    }

    private static async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken token)
    {
        if (update.Type == UpdateType.Message && update.Message != null)
        {
            var chatId = update.Message.Chat.Id;
            _chatIds.TryAdd(chatId, true); // сохраняем всех, кто писал боту

            var text = update.Message.Text?.Trim();

            if (text == "/top")
            {
                try
                {
                    // Формируем таблицу лидеров
                    var leaderboard = await BuildLeaderboardAsync();
                    await bot.SendTextMessageAsync(chatId, leaderboard);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(chatId, $"Ошибка при формировании таблицы лидеров: {ex.Message}");
                }
            }
        }
    }

    private static Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken token)
    {
        Console.WriteLine($"Telegram error: {exception.Message}");
        return Task.CompletedTask;
    }

    // Рассылка всем подписчикам
    public static async Task SendMessage(string message)
    {
        if (_bot == null)
        {
            Console.WriteLine("TelegramService not initialized!");
            return;
        }

        foreach (var chatId in _chatIds.Keys)
        {
            try
            {
                await _bot.SendTextMessageAsync(chatId, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send to {chatId}: {ex.Message}");
            }
        }
    }

    // Остановка бота
    public static void Stop()
    {
        _cts?.Cancel();
    }
    
    private static async Task<string> BuildLeaderboardAsync()
    {
        // Нужно получить сервис доступа к базе
        // Предполагаем, что у тебя есть DI, поэтому передадим фабрику контекста
        await using var db = _contextFactory.CreateDbContext(); // <-- замените на свою фабрику или сервис
        var users = await db.Users
            .OrderByDescending(u => u.Score)
            .Take(10) // топ-10
            .ToListAsync();

        if (!users.Any()) return "Лидеры пока отсутствуют.";

        // Формируем текст в формате "Имя | Счёт"
        var sb = new StringBuilder();
        sb.AppendLine("🏆 Таблица лидеров:");
        sb.AppendLine("Имя | Счёт");
        sb.AppendLine("-------------");
        foreach (var u in users)
        {
            sb.AppendLine($"{u.Name} | {u.Score}");
        }

        return sb.ToString();
    }
}
