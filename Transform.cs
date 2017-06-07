using OpenTK;

namespace Template_P3
{
    class Transform
    {
        private Matrix4 _model, _toWorld, _toScreen;
        private Vector3 _worldPos;

        public Matrix4 Model
        {
            get { return _model; }
        }

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
            _model = Matrix4.Identity;
            _toWorld = Matrix4.Identity;
            _toScreen = Matrix4.Identity;
            _worldPos = Vector3.Zero;
        }

        public void Clear()
        {
            _toWorld = Matrix4.Identity;
        }

        // translate in model space
        public void TranslateModel(Vector3 translation)
        {
            _model *= Matrix4.CreateTranslation(translation);
        }
    
        // rotate in model space
        public void RotateModel(Vector3 axes, float angle)
        {
            _model *= Matrix4.CreateFromAxisAngle(axes, angle);
        }

        // translate in world space
        public void TranslateWorld(Vector3 translation)
        {
            _toWorld *= Matrix4.CreateTranslation(translation);
        }

        // rotate in world space
        public void RotateWorld(Vector3 axes, float angle)
        {
            _toWorld *= Matrix4.CreateFromAxisAngle(axes, angle);
        }

        // rotate around a point in world space
        public void RotateAround(Vector3 point, Vector3 offset, Vector3 axes, float angle)
        {
            Clear();
            TranslateWorld(offset);
            RotateWorld(axes, angle);
            TranslateWorld(point);
        }
    }
}