using OpenTK;
using OpenTK.Input;

namespace Template_P3
{
    class Camera : SceneGraphObject
    {
        private int _sensitivity;
        public Matrix4 perspective;

        public Matrix4 toScreen
        {
            get { return transform.World * perspective; }
        }

        public Camera()
        {
            _sensitivity = 10;
            perspective = Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 100);
            visible = false;
        }

        public override void OnRenderFrame(float elapsedTime)
        {
            // Use input to move in the world
            InputManager input = Game.inputManager;
            /// lower and higher
            if(input.KeyDown(Key.W))
            { transform.TranslateModel(new Vector3(0, -_sensitivity * elapsedTime, 0)); }
            else if(input.KeyDown(Key.S))
            { transform.TranslateModel(new Vector3(0, _sensitivity * elapsedTime, 0)); }
            /// left and right
            if(input.KeyDown(Key.A))
            { transform.TranslateModel(new Vector3(_sensitivity * elapsedTime, 0, 0)); }
            else if(input.KeyDown(Key.D))
            { transform.TranslateModel(new Vector3(-_sensitivity * elapsedTime, 0, 0)); }
            /// back and forwards
            transform.TranslateModel(new Vector3(0, 0, 30 * input.ScrollWheel * elapsedTime));
            /// rotation using the mouse
            if (input.LeftMouseButtonDown)
            { transform.RotateModel(new Vector3(1, 0, 0), 0.001f * input.MousePos.Y); transform.RotateModel(new Vector3(0, 1, 0), 0.001f * input.MousePos.X); }
        }
    }
}