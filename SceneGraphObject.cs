using OpenTK;
using System.Collections.Generic;

namespace Template_P3
{
    class SceneGraphObject
    {
        public readonly Transform transform;
        private Mesh _mesh;
        private List<SceneGraphObject> _children;
        protected bool visible;

        public Mesh Mesh
        {
            get { return _mesh; }
        }

        public List<SceneGraphObject> Children
        {
            get { return _children; }
        }

        public SceneGraphObject()
        {
            transform = new Transform();
            _children = new List<SceneGraphObject>();
            visible = true;
        }

        public void setMesh(string fileName)
        {
            _mesh = new Mesh(fileName);
        }

        public void Render(Shader shader, Matrix4 cameraMatrix)
        {
            if (visible)
            {
                Matrix4 recursive = transform.Model * transform.ToWorld;
                _mesh.Render(shader, recursive * cameraMatrix * Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 100));
                foreach (SceneGraphObject o in _children)
                {
                    o.Render(shader, recursive);
                }
            }
        }

        private void Render(Shader shader, Matrix4 cameraMatrix, Matrix4 wm)
        {
            if (visible)
            {
                Matrix4 recursiveMatrix = transform.ToWorld * wm;
                _mesh.Render(shader, recursiveMatrix * cameraMatrix * Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 100));
                foreach (SceneGraphObject o in _children)
                {
                    o.Render(shader, recursiveMatrix);
                }
            }
        }

        public virtual void OnRenderFrame(float elapsedTime)
        {

        }
    }
}
