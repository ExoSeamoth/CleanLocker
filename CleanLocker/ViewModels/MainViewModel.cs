using CleanLocker.Assets.Lang;
using CleanLocker.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CleanLocker.ViewModels;

public partial class MainViewModel(IKeyboardHook keyboardHook) : ObservableObject
{
    public string ButtonText => IsKeyboardLocked ? Locale.Unlock : Locale.Lock;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(ButtonText))]
    private bool _isKeyboardLocked;

    [RelayCommand]
    private void LockUnlock()
    {
        if (IsKeyboardLocked)
            keyboardHook.Unlock();
        else
            keyboardHook.Lock();

        IsKeyboardLocked = keyboardHook.IsLocked;
    }

    public void ForceUnlock()
    {
        keyboardHook.Unlock();
    }
}