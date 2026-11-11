using AvaloniaAuthApp.Models;
using AvaloniaAuthApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace AvaloniaAuthApp.ViewModels;

public partial class AuthViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly AuthSession _session;
    private readonly IRegistrationDialogService _registrationDialogService;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    public User? CurrentUser => _session.User;
    public bool IsAuthenticated => _session.IsAuthenticated;

    public AuthViewModel(IAuthService authService, AuthSession session, IRegistrationDialogService registrationDialogService)
    {
        _authService = authService;
        _session = session;
        _registrationDialogService = registrationDialogService;
        _session.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName is nameof(AuthSession.User) or nameof(AuthSession.IsAuthenticated))
            {
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        };
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Введите email и пароль.";
            return;
        }

        try
        {
            IsBusy = true;
            var response = await _authService.LoginAsync(new LoginRequest { Email = Email.Trim(), Password = Password });
            StartSession(response);
            Password = string.Empty;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        ErrorMessage = string.Empty;
        try
        {
            await _authService.LogoutAsync(_session.RefreshToken);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Не удалось завершить сессию на сервере: {ex.Message}";
        }
        finally
        {
            _session.End();
        }
    }

    [RelayCommand]
    private async Task OpenRegisterDialogAsync()
    {
        ErrorMessage = string.Empty;
        try
        {
            IsBusy = true;
            var response = await _registrationDialogService.ShowAsync();
            if (response is not null)
            {
                StartSession(response);
                Password = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void StartSession(AuthResponse response)
    {
        if (string.IsNullOrWhiteSpace(response.AccessToken))
        {
            throw new InvalidOperationException("Сервер не вернул токен авторизации.");
        }

        _session.Start(response.AccessToken, response.RefreshToken, response.User);
    }
}