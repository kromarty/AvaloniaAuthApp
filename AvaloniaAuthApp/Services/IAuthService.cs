using AvaloniaAuthApp.Models;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaAuthApp.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<User> GetCurrentUserAsync(string token, CancellationToken ct = default);
    Task LogoutAsync(string refreshToken, CancellationToken ct = default);
}