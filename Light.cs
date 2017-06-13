using OpenTK;

namespace Template_P3
{
    public class Light : SceneGraphObject
    {
        public Vector3 color;

        public Light()
        {
            color = new Vector3(1, 1, 1);
            transform.TranslateModel(new Vector3(-10, 10, 0));
            visible = false;
        }
    }
}