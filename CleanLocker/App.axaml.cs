using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CleanLocker.Core;
using CleanLocker.ViewModels;
using CleanLocker.Views;
using Pure.DI;

namespace CleanLocker;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow()
            {
                DataContext = new Composition().Root
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    [Conditional("DI")]
    private static void Setup() => DI.Setup(nameof(Composition))
        .Bind<IKeyboardHook>().As(Lifetime.Singleton).To<IKeyboardHook>(_ =>
        {
            if (OperatingSystem.IsWindows()) return new WinKeyboardHook();
            if (OperatingSystem.IsMacOS()) return new MacKeyboardHook();
            if (OperatingSystem.IsLinux()) return new UnixKeyboardHook();

            return new MockKeyboardHook();
        })
        .Bind().As(Lifetime.Singleton).To<MainViewModel>()
        .Root<MainViewModel>("Root");
}