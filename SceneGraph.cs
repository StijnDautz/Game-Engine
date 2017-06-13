using System.Collections.Generic;

namespace Template_P3
{
    class SceneGraph
    {
        List<SceneGraphObject> _children;       // array of child ScenegraphObjects
        List<Camera> _cameras;                  // array of cameras
        List<Light> _ligthts;                   // array of lights
        Camera _camera;                         // the camera that is viewed through

        public SceneGraph()
        {
            _children = new List<SceneGraphObject>();
            _cameras = new List<Camera>();
            _ligthts = new List<Light>();
        }

        public void Render(Shader shader)
        {
            foreach(SceneGraphObject o in _children)
            {
                o.Render(shader, _camera.toScreen, _ligthts);
            }
        }

        public void Add(SceneGraphObject o)
        {
            // check for types and add o to the correct list
            if(o is Camera)
            {
                _cameras.Add(o as Camera);
                /// if o is Camera add it to _cameras and check it should be used as the main camera
                if (_camera == null) _camera = o as Camera;
            }
            else if(o is Light) _ligthts.Add(o as Light);
            /// always add o to _children to display o in the scenegraph
            _children.Add(o);
        }

        public void OnRenderFrame(float elapsedTime)
        {
            // loop through all child SceneGraphObjects and render them
            foreach(SceneGraphObject o in _children)
            {
                o.OnRenderFrame(elapsedTime);
            }
        }
    }
}