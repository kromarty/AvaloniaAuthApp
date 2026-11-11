using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaAuthApp.Models;
using AvaloniaAuthApp.ViewModels;
using AvaloniaAuthApp.Views;
using System;
using System.Threading.Tasks;

namespace AvaloniaAuthApp.Services;

public sealed class RegistrationDialogService : IRegistrationDialogService
{
    private readonly IAuthService _authService;

    public RegistrationDialogService(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<AuthResponse?> ShowAsync()
    {
        var owner = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow
            ?? throw new InvalidOperationException("Не удалось определить главное окно приложения.");

        var window = new RegisterWindow
        {
            DataContext = new RegisterViewModel(_authService)
        };

        return window.ShowDialog<AuthResponse?>(owner);
    }
}
