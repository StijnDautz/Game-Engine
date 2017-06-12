using OpenTK;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

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

        // this function is called if the object has no parent, so there's no parentMatrix involved in the calculations
        public void Render(Shader shader, Matrix4 cameraMatrix)
        {
            if (visible)
            {
                /// render the object's mesh
                RenderMesh(shader, cameraMatrix, transform.World);
                /// render this object's children
                foreach (SceneGraphObject o in _children) o.Render(shader, cameraMatrix, transform.World);
            }
        }

        // this function is called by the object's parent and has an additional parentMatrix compared to the other Render method
        private void Render(Shader shader, Matrix4 cameraMatrix, Matrix4 parentMatrix)
        {
            if (visible)
            {
                /// multiply this object's world matrix with its parent's one
                Matrix4 recursiveMatrix = transform.World * parentMatrix;
                /// render this object's mesh
                RenderMesh(shader, cameraMatrix, recursiveMatrix);
                /// render this object's children
                foreach (SceneGraphObject o in _children) o.Render(shader, cameraMatrix, recursiveMatrix);
            }
        }

        // render the mesh
        private void RenderMesh(Shader shader, Matrix4 cameraMatrix, Matrix4 world)
        {
            _mesh.Render(shader, world, cameraMatrix);
        }

        public virtual void OnRenderFrame(float elapsedTime)
        {

        }
    }
}
