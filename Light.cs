using OpenTK;

namespace Template_P3
{
    public class Light : SceneGraphObject
    {
        public Vector3 color;
        public float intensity;

        public Light()
        {
            visible = false;
        }
    }
}