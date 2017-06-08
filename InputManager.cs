using OpenTK.Input;
using System.Drawing;

namespace Template_P3
{
    class InputManager
    {
        KeyboardState previousKBState, currentKBState;
        MouseState previousMState, currentMState;

        /*
         * KEYBOARD
         */
        public bool KeyDown(Key k)
        {
            return currentKBState.IsKeyDown(k);
        }

        //TODO is this function ever used?
        public bool KeyPressed(Key k)
        {
            return previousKBState.IsKeyUp(k) && currentKBState.IsKeyDown(k);
        }
        
        /*
         * MOUSE
         */
        public bool LeftMouseButtonDown
        {
            get { return currentMState.LeftButton == ButtonState.Pressed; }
        }

        public bool LeftMouseButtonPressed
        {
            get { return previousMState.LeftButton == ButtonState.Released && currentMState.LeftButton == ButtonState.Pressed; }
        }

        public float ScrollWheel
        {
            get { return currentMState.Scroll.Y - previousMState.Scroll.Y; }
        }

        public Point MousePos
        {
            get { return new Point(currentMState.X - previousMState.X, currentMState.Y - previousMState.Y); }
        }

        // TODO don't sync this with OnRenderFrame, but with OnFrameUpdate -> faster input time (not really neccesary for a rasterizer though)
        public void Update()
        {
            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();
            previousMState = currentMState;
            currentMState = Mouse.GetState();
        }
    }
}