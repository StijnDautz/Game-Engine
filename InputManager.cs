using OpenTK.Input;

namespace template_P3
{
    class InputManager
    {
        KeyboardState previousKBState, currentKBState; 

        public bool KeyDown(Key k)
        {
            return !previousKBState.IsKeyDown(k) && currentKBState.IsKeyDown(k);
        }
    }
}
