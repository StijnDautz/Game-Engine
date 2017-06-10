using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace Template_P3
{
    public class Shader
    {
        // data members
        public int programID, vsID, fsID;
        public int attribute_vpos;
        public int attribute_vnrm;
        public int attribute_vuvs;
        public int uniform_mview;

        // constructor
        public Shader(String vertexShader, String fragmentShader)
        {
            // compile shaders
            /// create a program that will contain the shaders
            programID = GL.CreateProgram();
            /// load the shaders        
            Load(vertexShader, ShaderType.VertexShader, programID, out vsID);
            Load(fragmentShader, ShaderType.FragmentShader, programID, out fsID);
            // link the shaders to each other
            GL.LinkProgram(programID);
            Console.WriteLine(GL.GetProgramInfoLog(programID));

            // get locations of shader parameters
            attribute_vpos = GL.GetAttribLocation(programID, "vPosition");
            attribute_vnrm = GL.GetAttribLocation(programID, "vNormal");
            attribute_vuvs = GL.GetAttribLocation(programID, "vUV");
            uniform_mview = GL.GetUniformLocation(programID, "transform");
        }

        // loading shaders
        void Load(String filename, ShaderType type, int program, out int ID)
        {
            // source: http://neokabuto.blogspot.nl/2013/03/opentk-tutorial-2-drawing-triangle.html
            // setup the shader
            ID = GL.CreateShader(type);
            /// load the shader source
            using (StreamReader sr = new StreamReader(filename)) GL.ShaderSource(ID, sr.ReadToEnd());
            /// compile the shader source
            GL.CompileShader(ID);
            /// attach the shader to the program
            GL.AttachShader(program, ID);
            Console.WriteLine(GL.GetShaderInfoLog(ID));
        }
    }
} // namespace Template_P3