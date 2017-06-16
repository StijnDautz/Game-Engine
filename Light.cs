using OpenTK;
using OpenTK.Input;

namespace Template_P3
{
    public class Light : SceneGraphObject
    {
        public Vector3 color;                                   // the color of the light
        public float intensity;                                 // the brightness of the light
        public float radius;                                    // the radius of the light
        public float attenuationC, attenuationL, attenuationQ;  // constant, linear and quadratic attenuation factor

        public Light()
        {
            // TODO add light rendering light component?
            // TODO add rendering without normal map, but with regular normals
            // TODO interpolate normals
            color = new Vector3(1, 1, 0.95f);
            visible = false;
        }

        public override void OnRenderFrame(float elapsedTime)
        {
            // Use input to move in the world
            InputManager input = Game.inputManager;
            /// lower and higher
            if (input.KeyDown(Key.I))
            { transform.TranslateModel(new Vector3(0, 0, -5 * elapsedTime)); }
            else if (input.KeyDown(Key.K))
            { transform.TranslateModel(new Vector3(0, 0, 5 * elapsedTime)); }
            /// left and right
            if (input.KeyDown(Key.J))
            { transform.TranslateModel(new Vector3(5 * elapsedTime, 0, 0)); }
            else if (input.KeyDown(Key.L))
            { transform.TranslateModel(new Vector3(-5 * elapsedTime, 0, 0)); }
        }
    }
}