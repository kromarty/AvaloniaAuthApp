using AvaloniaAuthApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaAuthApp.Services;

public partial class AuthSession : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAuthenticated))]
    private string _token = string.Empty;

    [ObservableProperty]
    private string _refreshToken = string.Empty;

    [ObservableProperty]
    private User? _user;

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(Token);

    public void Start(string token, string refreshToken, User? user)
    {
        User = user;
        RefreshToken = refreshToken;
        Token = token;
    }

    public void End()
    {
        Token = string.Empty;
        RefreshToken = string.Empty;
        User = null;
    }
}
