using System;

namespace VaultSave.AutoSave
{
    [Flags]
    public enum VSAutoSaveTypes
    {
        None = 0,
        Awake = 1,
        OnEnable = 2,
        Start = 4,
        OnAppPause = 8,
        OnAppQuit = 16,
    }
}