// Key code documentation:
// http://msdn.microsoft.com/en-us/library/dd375731%28v=VS.85%29.aspx

internal enum KeyCode : int
{
    Space = 0x20,
    Backspace = 0x08
}

internal static class NativeKeyboard
{
    // A positional bit flag indicating the part of a key state denoting key pressed.
    private const int KeyPressed = 0x8000;

    // Returns a value indicating if a given key is pressed.
    public static bool IsKeyDown(KeyCode key)
    {
        return (GetKeyState((int)key) & KeyPressed) != 0;
    }

    // Gets the key state of a key.
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern short GetKeyState(int key);
}