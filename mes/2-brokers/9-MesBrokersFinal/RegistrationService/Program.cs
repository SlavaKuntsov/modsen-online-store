using System.Text;
using Common;
using Common.Models;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();

// Объявляем очередь один раз
await channel.QueueDeclareAsync(
	"registration-events",
	false,
	false,
	false);

Console.WriteLine("Введите данные пользователя. Пустой email — выход.");

while (true)
{
	Console.Write("Email: ");
	var email = Console.ReadLine();

	if (string.IsNullOrWhiteSpace(email))
		break;

	Console.Write("Name: ");
	var name = Console.ReadLine()!;

	var now = DateTime.UtcNow;
	var correlationId = Guid.NewGuid().ToString();

	var evt = new UserRegisteredEvent
	{
		EventId = Guid.NewGuid(),
		Timestamp = now,
		CorrelationId = correlationId,
		Data = new Data
		{
			UserId = Guid.NewGuid(),
			Email = email,
			Name = name,
			RegistrationDate = now
		}
	};

	var json = EventSerializer.Serialize(evt);
	var body = Encoding.UTF8.GetBytes(json);

	await channel.BasicPublishAsync(
		"",
		"registration-events",
		body);

	Console.WriteLine($"Sent: {json}\n");
}

Console.WriteLine("RegistrationService завершён.");