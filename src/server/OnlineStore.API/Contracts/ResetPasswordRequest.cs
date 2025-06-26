namespace OnlineStore.API.Contracts;

public sealed record ResetPasswordRequest(string Token, string NewPassword);