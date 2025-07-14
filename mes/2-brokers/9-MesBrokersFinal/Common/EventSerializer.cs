using System.Text.Json;

namespace Common;

public static class EventSerializer
{
	private static readonly JsonSerializerOptions Options = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase
	};

	public static string Serialize<T>(T @event)
	{
		return JsonSerializer.Serialize(@event, Options);
	}

	public static T Deserialize<T>(string json)
	{
		return JsonSerializer.Deserialize<T>(json, Options)!;
	}
}