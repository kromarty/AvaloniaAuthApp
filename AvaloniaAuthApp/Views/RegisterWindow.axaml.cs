using Avalonia.Controls;
using AvaloniaAuthApp.ViewModels;
using System;

namespace AvaloniaAuthApp.Views;

public partial class RegisterWindow : Window
{
    public RegisterWindow()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is RegisterViewModel vm)
        {
            vm.RegistrationCompleted += (_, response) => Close(response);
        }
    }
}