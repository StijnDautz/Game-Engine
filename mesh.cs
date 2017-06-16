﻿using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace Template_P3
{
    // mesh and loader based on work by JTalton; http://www.opentk.com/node/642

    public class Mesh
    {
        // data members
        public ObjVertex[] vertices;                            // vertex positions, model space
        public ObjTriangle[] triangles;                         // triangles (3 vertex indices)
        public ObjQuad[] quads;                                 // quads (4 vertex indices)
        int vertexBufferId, triangleBufferId, quadBufferId;     // vertex buffer, triangle buffer, quad buffer
        public Material material;

        // constructors
        //TODO improve setting normal map routine
        public Mesh(string fileName)
        {
            var loader = new MeshLoader();
            loader.Load(this, "../../" + fileName);
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
        public void Render(Shader shader, Matrix4 toWorld, Matrix4 toScreen, Vector3 cameraPos, List<Light> lights)
        {
            // on first run, prepare buffers
            Prepare(shader);

            // setup light arrays to pass
            // TODO pick the closest lights instead of the first ones in the list      
            /// initialize variables
            int size = Math.Min(4, lights.Count);
            int vsize = size * 3;
            float[] lightPositions = new float[vsize];
            float[] lightColors = new float[vsize];
            float[] lightIntensity = new float[size];
            /// fill the arrays
            int c = 0;
            for (int i = 0; i < vsize; i += 3, c++)
            {
                lightColors[i    ] = lights[c].color.X;
                lightColors[i + 1] = lights[c].color.Y;
                lightColors[i + 2] = lights[c].color.Z;
                lightPositions[i    ] = lights[c].transform.worldPos.X;
                lightPositions[i + 1] = lights[c].transform.worldPos.Y;
                lightPositions[i + 2] = lights[c].transform.worldPos.Z;
                lightIntensity[c] = lights[c].intensity;
            }

            // pass variables to shader
            GL.UseProgram(shader.programID);

            /// pass the arrays with light related variables
            GL.Uniform1(shader.uniform_lightcnt, size);
            GL.Uniform1(shader.uniform_Alightcol, vsize, lightColors);
            GL.Uniform1(shader.uniform_Alightpos, vsize, lightPositions);
            GL.Uniform1(shader.uniform_Alightintensity, size, lightIntensity);

            /// pass world variables to shader
            GL.Uniform3(shader.uniform_camerapos, cameraPos);
            GL.UniformMatrix4(shader.uniform_toWorld, false, ref toWorld);
            GL.UniformMatrix4(shader.uniform_toScreen, false, ref toScreen);

            /// pass materials variabels to shader
            material.PassVarsToShader(shader);

            int vertexsize = Marshal.SizeOf(typeof(ObjVertex));

            // bind interleaved vertex data
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
            GL.InterleavedArrays(InterleavedArrayFormat.T2fN3fV3f, vertexsize, IntPtr.Zero);

            // link vertex attributes to shader parameters 
            GL.VertexAttribPointer(shader.attribute_vuvs, 2, VertexAttribPointerType.Float, false, vertexsize, 0);
            GL.VertexAttribPointer(shader.attribute_vnrm, 3, VertexAttribPointerType.Float, true, vertexsize, 2 * 4);
            GL.VertexAttribPointer(shader.attribute_vpos, 3, VertexAttribPointerType.Float, false, vertexsize, 5 * 4);
            GL.VertexAttribPointer(shader.attribute_tang, 3, VertexAttribPointerType.Float, true, vertexsize, 8 * 4);

            // enable position, normal and uv attributes
            GL.EnableVertexAttribArray(shader.attribute_vpos);
            GL.EnableVertexAttribArray(shader.attribute_vnrm);
            GL.EnableVertexAttribArray(shader.attribute_vuvs);
            GL.EnableVertexAttribArray(shader.attribute_tang);

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
            public Vector3 Tangent;
            public Vector3 BiTangent;
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
