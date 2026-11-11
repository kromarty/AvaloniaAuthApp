using Avalonia.Media.Imaging;
using AvaloniaAuthApp.Models;
using AvaloniaAuthApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AvaloniaAuthApp.ViewModels;

public partial class View1ViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly AuthSession _session;
    private readonly HttpClient _httpClient;

    [ObservableProperty]
    private User? _user;

    [ObservableProperty]
    private Bitmap? _avatarImage;

    [ObservableProperty]
    private bool _isAuthenticated;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public View1ViewModel(IAuthService authService, AuthSession session, HttpClient httpClient)
    {
        _authService = authService;
        _session = session;
        _httpClient = httpClient;
        _session.PropertyChanged += OnSessionChanged;
        UpdateSessionState();
    }

    private void OnSessionChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName is nameof(AuthSession.Token) or nameof(AuthSession.User))
        {
            UpdateSessionState();
        }
    }

    private void UpdateSessionState()
    {
        IsAuthenticated = _session.IsAuthenticated;
        User = _session.User;
        AvatarImage?.Dispose();
        AvatarImage = null;
        ErrorMessage = string.Empty;

        if (IsAuthenticated)
        {
            _ = LoadUserAsync();
        }
    }

    private async Task LoadAvatarAsync(string avatarUrl, string token)
    {
        if (string.IsNullOrWhiteSpace(avatarUrl))
        {
            return;
        }

        try
        {
            using var stream = await _httpClient.GetStreamAsync(avatarUrl);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var image = new Bitmap(memoryStream);
            if (_session.Token != token)
            {
                image.Dispose();
                return;
            }

            AvatarImage?.Dispose();
            AvatarImage = image;
        }
        catch (Exception ex) when (ex is HttpRequestException or IOException or ArgumentException)
        {
            if (_session.Token == token)
            {
                ErrorMessage = $"Не удалось загрузить аватар: {ex.Message}";
            }
        }
    }

    [RelayCommand]
    private async Task LoadUserAsync()
    {
        var token = _session.Token;
        if (string.IsNullOrWhiteSpace(token))
        {
            return;
        }

        try
        {
            IsBusy = true;
            var user = await _authService.GetCurrentUserAsync(token);
            if (_session.Token == token)
            {
                User = user;
                await LoadAvatarAsync(user.Avatar, token);
            }
        }
        catch (Exception ex)
        {
            if (_session.Token == token)
            {
                ErrorMessage = ex.Message;
            }
        }
        finally
        {
            if (_session.Token == token)
            {
                IsBusy = false;
            }
        }
    }
}