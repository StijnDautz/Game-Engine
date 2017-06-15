using OpenTK;

namespace Template_P3
{
    public class Light : SceneGraphObject
    {
        public Vector3 color;
        public float intensity;

        public Light()
        {
            color = new Vector3(1);
            visible = false;
        }
    }
}