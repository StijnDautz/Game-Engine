using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Template_P3
{
    public class Material
    {
        public Texture diffuseTexture, normalTexture;           // textures
        public Vector3 diffuseColor;                            // diffuse color
        public float ambientModf, diffuseModf, specularModf;    // modifiers to change influence of different lightning types

        public Material()
        {

        }

        public void PassVarsToShader(Shader shader)
        {
            // pass material variables to shader
            /// pass textures
            if (diffuseTexture != null)
            {
                int diffuse = GL.GetUniformLocation(shader.programID, "pixels");
                int normal = GL.GetUniformLocation(shader.programID, "normalMap");
                GL.Uniform1(diffuse, 0);
                GL.Uniform1(normal, 1);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, diffuseTexture.id);
                GL.ActiveTexture(TextureUnit.Texture1);
                GL.BindTexture(TextureTarget.Texture2D, normalTexture.id);
            }

            /// pass diffuse color
            GL.Uniform3(shader.uniform_color, diffuseColor);
        }

        public static Texture GetTexture(string filename)
        {
            return new Texture("../../" + filename);
        }
    }
}