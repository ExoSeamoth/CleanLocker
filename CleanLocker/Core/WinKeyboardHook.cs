using System;
using System.Runtime.InteropServices;

namespace CleanLocker.Core;

public sealed partial class WinKeyboardHook : IKeyboardHook
{
    private const int WhKeyboardLl = 13;
    
    private HookProc? _hookProcDelegate;
    private IntPtr _hookId = IntPtr.Zero;

    [LibraryImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowsHookExA")]
    private static partial IntPtr SetWindowsHookExA(int idHook, HookProc lpfn, IntPtr hmod, uint dwThreadId);

    [LibraryImport("user32.dll", SetLastError = true, EntryPoint = "CallNextHookEx")]
    private static partial IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [LibraryImport("user32.dll", SetLastError = true, EntryPoint = "UnhookWindowsHookEx")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool UnhookWindowsHookEx(IntPtr hhk);

    private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
    
    public bool IsLocked { get; private set; }

    public void Lock()
    {
        if (IsLocked) return;
        _hookProcDelegate = HookCallback;
        IntPtr hMod = Marshal.GetHINSTANCE(typeof(WinKeyboardHook).Module);
        
        _hookId = SetWindowsHookExA(WhKeyboardLl, _hookProcDelegate, hMod, 0);

        if (_hookId == IntPtr.Zero)
        {
            _hookProcDelegate = null;
            return;
        }
        
        IsLocked = true;
    }

    public void Unlock()
    {
        if (!IsLocked) return;
        
        UnhookWindowsHookEx(_hookId);
        
        _hookId = IntPtr.Zero;
        _hookProcDelegate = null;
        IsLocked = false;
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        // nCode < 0 означает, что мы обязаны передать управление следующему хуку
        if (nCode < 0)
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);

        // Если клавиатура заблокирована (IsLocked == true), мы "проглатываем" нажатие.
        // В Win32 API возврат значения (nint)1 (или IntPtr(1)) означает, что сообщение обработано 
        // и его НЕ нужно передавать дальше в систему.
        if (IsLocked)
            return new IntPtr(1);
        
        // Иначе передаем нажатие дальше, как ни в чем не бывало
        return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
    }
}