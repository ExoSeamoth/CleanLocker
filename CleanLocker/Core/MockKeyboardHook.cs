namespace CleanLocker.Core;

public sealed class MockKeyboardHook: IKeyboardHook
{
    public bool IsLocked { get; private set; }
    public bool IsMouseLocked { get; private set; }

    public void Lock()
    {
        throw new System.NotImplementedException();
    }

    public void Unlock()
    {
        throw new System.NotImplementedException();
    }
}