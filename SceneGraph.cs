using System.Collections.Generic;

namespace Template_P3
{
    class SceneGraph
    {
        List<SceneGraphObject> _children;
        List<Camera> _cameras;
        Camera _camera;

        public SceneGraph()
        {
            _children = new List<SceneGraphObject>();
            _cameras = new List<Camera>();
        }

        public void Render(Shader shader)
        {
            foreach(SceneGraphObject o in _children)
            {
                o.Render(shader, _camera.toScreen);
            }
        }

        public void Add(SceneGraphObject o)
        {
            //if o is Camera add it to _cameras and check it should be used as the main camera
            if(o is Camera)
            {
                _cameras.Add(o as Camera);
                if (_camera == null) _camera = o as Camera;
            }
            _children.Add(o);
        }

        public void OnRenderFrame(float elapsedTime)
        {
            foreach(SceneGraphObject o in _children)
            {
                o.OnRenderFrame(elapsedTime);
            }
        }
    }
}