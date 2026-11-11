using AvaloniaAuthApp.Models;
using AvaloniaAuthApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace AvaloniaAuthApp.ViewModels;

public partial class RegisterViewModel : ViewModelBase
{
    private readonly IAuthService _authService;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    public event EventHandler<AuthResponse>? RegistrationCompleted;

    public RegisterViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        ErrorMessage = string.Empty;
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Заполните все поля.";
            return;
        }

        try
        {
            IsBusy = true;
            await _authService.RegisterAsync(new RegisterRequest
            {
                Email = Email.Trim(),
                Name = Name.Trim(),
                Password = Password
            });

            var response = await _authService.LoginAsync(new LoginRequest
            {
                Email = Email.Trim(),
                Password = Password
            });

            if (string.IsNullOrWhiteSpace(response.AccessToken))
            {
                throw new InvalidOperationException("Сервер не вернул токен авторизации после регистрации.");
            }

            RegistrationCompleted?.Invoke(this, response);
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
}