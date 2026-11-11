using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AvaloniaAuthApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase? _currentView;

    public AuthViewModel AuthViewModel { get; }
    public ObservableCollection<NavigationItem> NavigationItems { get; } = new();

    public MainViewModel(AuthViewModel authViewModel, View1ViewModel view1ViewModel, View2ViewModel view2ViewModel)
    {
        AuthViewModel = authViewModel;
        NavigationItems.Add(new NavigationItem("Главная", view1ViewModel));
        NavigationItems.Add(new NavigationItem("О разработчике", view2ViewModel));
        CurrentView = view1ViewModel;
    }

    [RelayCommand]
    private void SelectView(NavigationItem item)
    {
        CurrentView = item.ViewModel;
    }
}

public sealed record NavigationItem(string Title, ViewModelBase ViewModel);