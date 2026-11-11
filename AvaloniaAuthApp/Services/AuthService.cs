using AvaloniaAuthApp.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaAuthApp.Services;

public sealed class AuthService : IAuthService
{
    private readonly HttpClient _http;

    public AuthService(HttpClient http)
    {
        _http = http;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        using var response = await _http.PostAsJsonAsync("api/auth/login", request, ct);
        await EnsureSuccessAsync(response, ct);
        return await ReadRequiredAsync<AuthResponse>(response, ct);
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        using var response = await _http.PostAsJsonAsync("api/auth/register", request, ct);
        await EnsureSuccessAsync(response, ct);
        return await ReadRequiredAsync<RegisterResponse>(response, ct);
    }

    public async Task<User> GetCurrentUserAsync(string token, CancellationToken ct = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "api/auth/me");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var response = await _http.SendAsync(request, ct);
        await EnsureSuccessAsync(response, ct);
        return await ReadRequiredAsync<User>(response, ct);
    }

    public async Task LogoutAsync(string refreshToken, CancellationToken ct = default)
    {
        using var response = await _http.PostAsJsonAsync(
            "api/auth/logout",
            new LogoutRequest { RefreshToken = refreshToken },
            ct);
        await EnsureSuccessAsync(response, ct);
    }

    private static async Task<T> ReadRequiredAsync<T>(HttpResponseMessage response, CancellationToken ct)
    {
        try
        {
            var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken: ct);
            return result ?? throw new ApiException("Сервер вернул пустой ответ.", (int)response.StatusCode);
        }
        catch (JsonException ex)
        {
            throw new ApiException($"Не удалось прочитать ответ сервера: {ex.Message}", (int)response.StatusCode);
        }
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync(ct);
        var message = $"Ошибка API: {(int)response.StatusCode} {response.ReasonPhrase}";
        if (!string.IsNullOrWhiteSpace(body))
        {
            try
            {
                using var document = JsonDocument.Parse(body);
                if (document.RootElement.TryGetProperty("message", out var messageElement) && messageElement.ValueKind == JsonValueKind.String)
                {
                    message = messageElement.GetString() ?? message;
                }
                else if (document.RootElement.TryGetProperty("error", out var errorElement) && errorElement.ValueKind == JsonValueKind.String)
                {
                    message = errorElement.GetString() ?? message;
                }
            }
            catch (JsonException)
            {
                message = body;
            }
        }

        throw new ApiException(message, (int)response.StatusCode);
    }
}