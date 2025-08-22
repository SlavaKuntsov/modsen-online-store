namespace OnlineStore.API.Contracts.Auth;

public sealed record ResetPasswordRequest(string Token, string NewPassword);