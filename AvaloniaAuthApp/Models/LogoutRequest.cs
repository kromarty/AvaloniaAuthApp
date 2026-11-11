using System.Text.Json.Serialization;

namespace AvaloniaAuthApp.Models;

public class LogoutRequest
{
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;
}
