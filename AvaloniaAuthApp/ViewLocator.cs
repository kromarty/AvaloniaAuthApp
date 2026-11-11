using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaAuthApp.ViewModels;
using AvaloniaAuthApp.Views;

namespace AvaloniaAuthApp;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param) => param switch
    {
        View1ViewModel => new View1View(),
        View2ViewModel => new View2View(),
        _ => null
    };

    public bool Match(object? data) => data is View1ViewModel or View2ViewModel;
}
