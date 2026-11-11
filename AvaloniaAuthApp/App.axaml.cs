using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaAuthApp.Services;
using AvaloniaAuthApp.ViewModels;
using AvaloniaAuthApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace AvaloniaAuthApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        services.AddSingleton(new HttpClient
        {
            BaseAddress = new Uri("https://fakeapifordevs.vercel.app/")
        });
        services.AddSingleton<AuthSession>();
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IRegistrationDialogService, RegistrationDialogService>();
        services.AddSingleton<AuthViewModel>();
        services.AddSingleton<View1ViewModel>();
        services.AddTransient<View2ViewModel>();
        services.AddSingleton<MainViewModel>();

        var serviceProvider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainVm = serviceProvider.GetRequiredService<MainViewModel>();
            desktop.MainWindow = new MainWindow { DataContext = mainVm };
            desktop.Exit += (_, _) => serviceProvider.Dispose();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
