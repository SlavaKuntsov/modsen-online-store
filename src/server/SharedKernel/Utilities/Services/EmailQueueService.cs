using System.Text.Json;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Utilities.Services;

public class EmailQueueService(IConnectionMultiplexer redis, ILogger<EmailQueueService> logger) : IEmailQueueService
{
    private readonly IDatabase _db = redis.GetDatabase();
    private const string QueueKey = "email_queue";

    public async Task EnqueueEmailAsync(string recipient, string subject, string body)
    {
        var message = new EmailMessage(recipient, subject, body);
        var json = JsonSerializer.Serialize(message);
        await _db.ListRightPushAsync(QueueKey, json);
        logger.LogInformation("Email queued for {Recipient}", recipient);
    }
}
