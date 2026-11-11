using System.Text.Json.Serialization;

namespace AvaloniaAuthApp.Models;

public class AuthResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;

    public User? User { get; set; }
}