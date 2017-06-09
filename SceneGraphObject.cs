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

        // this function is called if the object has no parent, so there's no parentMatrix involved in the calculations
        public void Render(Shader shader, Camera camera)
        {
            if (visible)
            {
                /// render the object's mesh
                RenderMesh(shader, camera, transform.World);
                /// render this object's children
                foreach (SceneGraphObject o in _children) o.Render(shader, camera, transform.World);
            }
        }

        // this function is called by the object's parent and has an additional parentMatrix compared to the other Render method
        private void Render(Shader shader, Camera camera, Matrix4 parentMatrix)
        {
            if (visible)
            {
                /// multiply this object's world matrix with its parent's one
                Matrix4 recursiveMatrix = transform.World * parentMatrix;
                /// render this object's mesh
                RenderMesh(shader, camera, recursiveMatrix);
                /// render this object's children
                foreach (SceneGraphObject o in _children) o.Render(shader, camera, recursiveMatrix);
            }
        }

        // render the mesh
        private void RenderMesh(Shader shader, Camera camera, Matrix4 world)
        {
            _mesh.Render(shader, world * camera.transform.World * camera.perspective);
        }

        public virtual void OnRenderFrame(float elapsedTime)
        {

        }
    }
}
