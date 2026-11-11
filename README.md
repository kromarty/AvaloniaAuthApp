# AvaloniaAuthApp

Приложение на Avalonia UI и .NET 10 с MVVM-архитектурой. Авторизация и регистрация используют REST API `https://fakeapifordevs.vercel.app`; меню переключает профиль и страницу с данными разработчика.

## Запуск

Откройте `AvaloniaAuthApp.slnx` в Visual Studio 2026 или запустите:

```powershell
dotnet run --project .\AvaloniaAuthApp\AvaloniaAuthApp.csproj
```

## NuGet-пакеты

- `Avalonia`, `Avalonia.Desktop`, `Avalonia.Themes.Fluent`, `Avalonia.Fonts.Inter` — настольный UI, Fluent-тема и шрифт.
- `CommunityToolkit.Mvvm` — генераторы свойств и команд MVVM.
- `Microsoft.Extensions.DependencyInjection` — регистрация и внедрение зависимостей приложения.

Сеть требуется для запросов аутентификации. При ошибках API приложение показывает сообщение и сохраняет состояние интерфейса согласованным.
