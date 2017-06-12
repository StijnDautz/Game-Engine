using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Template_P3
{
    // mesh and loader based on work by JTalton; http://www.opentk.com/node/642

    public class Mesh
    {
        // data members
        public ObjVertex[] vertices;                        // vertex positions, model space
        public ObjTriangle[] triangles;                     // triangles (3 vertex indices)
        public ObjQuad[] quads;                             // quads (4 vertex indices)
        int vertexBufferId, triangleBufferId, quadBufferId; // vertex buffer, triangle buffer, quad buffer
        private Texture _texture;
        private Vector3 color;

        public string Texture
        {
            set { _texture = new Texture("../../" + value); }
        }

        // constructors
        public Mesh(string fileName)
        {
            MeshLoader loader = new MeshLoader();
            loader.Load(this, "../../" + fileName);
            color = new Vector3(0, 0, 0);
        }

        // initialization; called during first render
        public void Prepare(Shader shader)
        {
            if (vertexBufferId != 0) return; // already taken care of

            // generate interleaved vertex data (uv/normal/position (total 8 floats) per vertex)
            GL.GenBuffers(1, out vertexBufferId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Marshal.SizeOf(typeof(ObjVertex))), vertices, BufferUsageHint.StaticDraw);

            // generate triangle index array
            GL.GenBuffers(1, out triangleBufferId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, triangleBufferId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(triangles.Length * Marshal.SizeOf(typeof(ObjTriangle))), triangles, BufferUsageHint.StaticDraw);

            // generate quad index array
            GL.GenBuffers(1, out quadBufferId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadBufferId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(quads.Length * Marshal.SizeOf(typeof(ObjQuad))), quads, BufferUsageHint.StaticDraw);
        }

        // render the mesh using the supplied shader and matrix
        public void Render(Shader shader, Matrix4 toWorld, Matrix4 cameraMatrix)
        {
            // on first run, prepare buffers
            Prepare(shader);

            // enable texture if it is not null
            if (_texture != null)
            {
                int texLoc = GL.GetUniformLocation(shader.programID, "pixels");
                GL.Uniform1(texLoc, 0);
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, _texture.id);
            }
            int lightCount = 2;
            float[] lightPositions = new float[3 * lightCount];
            lightPositions[0] = -7f;
            lightPositions[1] = -7f;
            lightPositions[2] = -7f;

            // enable shader, so variables can be passed to the shader
            GL.UseProgram(shader.programID);

            // pass uniform variabls to 
            //TODO retrieve this value from sceneGraph
            GL.Uniform3(shader.uniform_color, color);
            GL.UniformMatrix4(shader.uniform_toWorld, false, ref toWorld);
            GL.UniformMatrix4(shader.uniform_mcamera, false, ref cameraMatrix);
            
            lightPositions[3] = 5f;
            lightPositions[4] = 10f;
            lightPositions[5] = 10f;
            // pass lightpositions in a float[lightCount * 3], representing vector3's
            GL.Uniform1(shader.uniform_lights, 3 * lightCount, lightPositions);
            GL.Uniform1(shader.uniform_lightcount, lightCount);

            // bind interleaved vertex data
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
            GL.InterleavedArrays(InterleavedArrayFormat.T2fN3fV3f, Marshal.SizeOf(typeof(ObjVertex)), IntPtr.Zero);

            // link vertex attributes to shader parameters 
            GL.VertexAttribPointer(shader.attribute_vuvs, 2, VertexAttribPointerType.Float, false, 32, 0);
            GL.VertexAttribPointer(shader.attribute_vnrm, 3, VertexAttribPointerType.Float, true, 32, 2 * 4);
            GL.VertexAttribPointer(shader.attribute_vpos, 3, VertexAttribPointerType.Float, false, 32, 5 * 4);

            // enable position, normal and uv attributes
            GL.EnableVertexAttribArray(shader.attribute_vpos);
            GL.EnableVertexAttribArray(shader.attribute_vnrm);
            GL.EnableVertexAttribArray(shader.attribute_vuvs);

            // bind triangle index data and render
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, triangleBufferId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, triangles.Length * 3);

            // bind quad index data and render
            if (quads.Length > 0)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadBufferId);
                GL.DrawArrays(PrimitiveType.Quads, 0, quads.Length * 4);
            }

            // restore previous OpenGL state
            GL.UseProgram(0);
        }

        // layout of a single vertex
        [StructLayout(LayoutKind.Sequential)]
        public struct ObjVertex
        {
            public Vector2 TexCoord;
            public Vector3 Normal;
            public Vector3 Vertex;
            //public Vector3 Color;
        }

        // layout of a single triangle
        [StructLayout(LayoutKind.Sequential)]
        public struct ObjTriangle
        {
            public int Index0, Index1, Index2;
        }

        // layout of a single quad
        [StructLayout(LayoutKind.Sequential)]
        public struct ObjQuad
        {
            public int Index0, Index1, Index2, Index3;
        }
    }
}
