using OpenTK;

namespace Template_P3
{
    class Transform
    {
        private Matrix4 _toWorld, _toScreen;
        private Vector3 _worldPos;

        public Matrix4 ToWorld
        {
            get { return _toWorld; }
        }

        public Matrix4 toScreen
        {
            get { return _toScreen; }
        }
        
        public Transform()
        {
            _toWorld = Matrix4.Zero;
            _toScreen = Matrix4.Zero;
            _worldPos = Vector3.Zero;
        }

        public void Clear()
        {
            _toWorld = Matrix4.Zero;
        }

        // translate in world space
        public void Translate(Vector3 translation)
        {
            if(_toWorld == Matrix4.Zero)
            {
                _toWorld = Matrix4.CreateTranslation(translation);
            }
            else
            {
                _toWorld *= Matrix4.CreateTranslation(translation);
            }
        }

        public void Rotate(Vector3 axes, float angle)
        {
            if(_toWorld == Matrix4.Zero)
            { _toWorld = Matrix4.CreateFromAxisAngle(axes, angle); }
            else
            {
                _toWorld *= Matrix4.CreateFromAxisAngle(axes, angle);
            }
        }
    }
}
