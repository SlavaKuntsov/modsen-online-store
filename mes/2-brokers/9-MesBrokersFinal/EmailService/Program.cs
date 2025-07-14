using System.Text;
using Common;
using Common.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();
await channel.QueueDeclareAsync("registration-events", false, false, false, null);

var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += async (model, ea) =>
{
	var json = Encoding.UTF8.GetString(ea.Body.ToArray());
	var evt = EventSerializer.Deserialize<UserRegisteredEvent>(json);

	Console.WriteLine(
		$"EmailService -> Sending to {evt.Data.Name} <{evt.Data.Email}>");

	await channel.BasicAckAsync(ea.DeliveryTag, false);
};

await channel.BasicConsumeAsync("registration-events", false, consumer);
Console.WriteLine("Listening for registration events...");
Console.ReadLine();