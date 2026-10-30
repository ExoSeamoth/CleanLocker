namespace CleanLocker.Core;

public interface IKeyboardHook
{
    public bool IsLocked { get; }
    public void Lock();

    public void Unlock();
}