namespace Common.Models;

public class UserRegisteredEvent
{
	public Guid EventId { get; set; }
	public string EventType { get; set; } = "UserRegistered";
	public DateTime Timestamp { get; set; }
	public string Version { get; set; } = "1.0";
	public string CorrelationId { get; set; }
	public Data Data { get; set; }
}