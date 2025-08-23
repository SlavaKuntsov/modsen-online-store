using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Utilities.Services;

public class EmailQueueBackgroundService(
    IConnectionMultiplexer redis,
    IEmailService emailService,
    ILogger<EmailQueueBackgroundService> logger) : BackgroundService
{
    private readonly IDatabase _db = redis.GetDatabase();
    private const string QueueKey = "email_queue";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var value = await _db.ListLeftPopAsync(QueueKey);
                if (value.HasValue)
                {
                    var message = JsonSerializer.Deserialize<EmailMessage>(value!);
                    if (message is not null)
                    {
                        await emailService.SendEmailAsync(message.Recipient, message.Subject, message.Body);
                    }
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing email queue");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
