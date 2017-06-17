using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Template_P3
{
    public class Material
    {
        public Texture diffuseTexture, normalTexture;           // textures
        public Vector3 diffuseColor;                            // diffuse color
        private float _diffuseModf, _specularModf;                 // modifiers to change influence of different lightning types
        public float shininess;

        public float DiffuseModf
        {
            set { _diffuseModf = MathHelper.Clamp(value, 0f, 1f); }
        }

        public float SpecularModf
        {
            set { _specularModf = MathHelper.Clamp(value, 0f, 1f); }
        }

        public Material()
        {
            _diffuseModf = 1f;
            _specularModf = 1;
            shininess = 10f;
        }

        public void PassVarsToShader(Shader shader)
        {
            // pass material variables to shader
            /// pass textures
            if (diffuseTexture != null)
            {
                PassTexture(0, diffuseTexture.id, shader.uniform_diffuseMap);
                PassTexture(1, normalTexture.id, shader.uniform_normalMap);
            }

            /// pass diffuse color
            GL.Uniform3(shader.uniform_color, diffuseColor);
            /// pass lighting modifiers
            GL.Uniform1(shader.uniform_diffuseModf, _diffuseModf);
            GL.Uniform1(shader.uniform_specularModf, _specularModf);
            GL.Uniform1(shader.uniform_shininess, shininess);
        }

        public static Texture GetTexture(string filename)
        {
            return new Texture("../../" + filename);
        }

        private void PassTexture(int num, int texid, int shaderid)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.ActiveTexture(TextureUnit.Texture0 + num);
            GL.BindTexture(TextureTarget.Texture2D, texid);
            GL.Uniform1(shaderid, num);
        }
    }
}