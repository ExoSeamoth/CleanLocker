using System;
using Avalonia.Controls;
using CleanLocker.Core;
using CleanLocker.ViewModels;

namespace CleanLocker.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        if (Design.IsDesignMode)
        {
            // This can be before or after InitializeComponent.
            Design.SetDataContext(this, new MainViewModel(new MockKeyboardHook()));
        }
    }
    
    protected override void OnClosed(EventArgs e)
    {
        // Получаем ViewModel и принудительно разблокируем клавиатуру
        if (DataContext is MainViewModel vm)
        {
            vm.ForceUnlock();
        }
        base.OnClosed(e);
    }
}