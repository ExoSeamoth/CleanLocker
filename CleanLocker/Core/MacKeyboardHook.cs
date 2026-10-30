namespace CleanLocker.Core;

public sealed partial class MacKeyboardHook : IKeyboardHook
{
    public bool IsLocked { get; private set; }

    public void Lock()
    {
        throw new System.NotImplementedException();
    }

    public void Unlock()
    {
        throw new System.NotImplementedException();
    }
}