using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace Template_P3
{
    public class Shader
    {
        // data members
        public int programID, vsID, fsID;
        public int uniform_color;
        public int attribute_vuvs;
        public int attribute_vpos;
        public int attribute_vnrm;
        public int attribute_tang;
        public int uniform_toWorld;
        public int uniform_toScreen;
        public int uniform_lightcnt;
        public int uniform_camerapos;
        public int uniform_Alightpos;
        public int uniform_Alightcol;
        public int uniform_Alightintensity;
        public int uniform_diffuseMap;
        public int uniform_normalMap;
        public int uniform_diffuseModf, uniform_specularModf;
        public int uniform_shininess;

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
            attribute_vuvs = GL.GetAttribLocation(programID, "vUV");
            uniform_color = GL.GetUniformLocation(programID, "color");
            attribute_vnrm = GL.GetAttribLocation(programID, "vNormal");
            attribute_tang = GL.GetAttribLocation(programID, "vTangent");
            attribute_vpos = GL.GetAttribLocation(programID, "vPosition");
            uniform_toWorld = GL.GetUniformLocation(programID, "toWorld");
            uniform_toScreen = GL.GetUniformLocation(programID, "toScreen");
            uniform_lightcnt = GL.GetUniformLocation(programID, "lightcount");
            uniform_camerapos = GL.GetUniformLocation(programID, "camerapos");
            uniform_Alightpos = GL.GetUniformLocation(programID, "Alightpos");
            uniform_Alightcol = GL.GetUniformLocation(programID, "Alightcol");
            uniform_Alightintensity = GL.GetUniformLocation(programID, "Alightintensity");
            uniform_diffuseMap = GL.GetUniformLocation(programID, "diffuseMap");
            uniform_normalMap = GL.GetUniformLocation(programID, "normalMap");
            uniform_diffuseModf = GL.GetUniformLocation(programID, "diffuseModf");
            uniform_specularModf = GL.GetUniformLocation(programID, "specularModf");
            uniform_shininess = GL.GetUniformLocation(programID, "shininess");
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