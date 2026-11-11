using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaAuthApp.ViewModels;

public partial class View2ViewModel : ViewModelBase
{
    [ObservableProperty] private string _developerName = "Антон";
    [ObservableProperty] private string _developerDescription = "Разработчик AvaloniaAuthApp на .NET 10";
    [ObservableProperty] private string _contactInfo = "Контакт не указан";
}