using System.IO;
using System.Collections.Generic;
using OpenTK;

namespace Template_P3 {

    // mesh and loader based on work by JTalton; http://www.opentk.com/node/642

    public class MeshLoader
    {
        public bool Load(Mesh mesh, string fileName)
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(fileName))
                {
                    Load(mesh, streamReader);
                    streamReader.Close();
                    return true;
                }
            }
            catch { return false; }
        }

        char[] splitCharacters = new char[] { ' ' };

        List<Vector3> vertices;
        List<Vector3> normals;
        List<Vector2> texCoords;
        List<Mesh.ObjVertex> objVertices;
        List<Mesh.ObjTriangle> objTriangles;
        List<Mesh.ObjQuad> objQuads;

        void Load(Mesh mesh, TextReader textReader)
        {
            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            texCoords = new List<Vector2>();
            objVertices = new List<Mesh.ObjVertex>();
            objTriangles = new List<Mesh.ObjTriangle>();
            objQuads = new List<Mesh.ObjQuad>();
            string line;
            while ((line = textReader.ReadLine()) != null)
            {
                line = line.Trim(splitCharacters);
                line = line.Replace("  ", " ");
                string[] parameters = line.Split(splitCharacters);
                switch (parameters[0])
                {
                    case "p": // point
                        break;
                    case "v": // vertex
                        float x = float.Parse(parameters[1]);
                        float y = float.Parse(parameters[2]);
                        float z = float.Parse(parameters[3]);
                        vertices.Add(new Vector3(x, y, z));
                        break;
                    case "vt": // texCoord
                        float u = float.Parse(parameters[1]);
                        float v = float.Parse(parameters[2]);
                        texCoords.Add(new Vector2(u, v));
                        break;
                    case "vn": // normal
                        float nx = float.Parse(parameters[1]);
                        float ny = float.Parse(parameters[2]);
                        float nz = float.Parse(parameters[3]);
                        normals.Add(new Vector3(nx, ny, nz));
                        break;
                    case "f":
                        switch (parameters.Length)
                        {
                            case 4:
                                Mesh.ObjTriangle objTriangle = new Mesh.ObjTriangle();
                                objTriangle.Index0 = ParseFaceParameter(parameters[1]);
                                objTriangle.Index1 = ParseFaceParameter(parameters[2]);
                                objTriangle.Index2 = ParseFaceParameter(parameters[3]);
                                /// set the tangent vectors of the vertices of the triangle
                                setTriangleTangents(objTriangle);
                                objTriangles.Add(objTriangle);
                                break;
                            case 5:
                                Mesh.ObjQuad objQuad = new Mesh.ObjQuad();
                                objQuad.Index0 = ParseFaceParameter(parameters[1]);
                                objQuad.Index1 = ParseFaceParameter(parameters[2]);
                                objQuad.Index2 = ParseFaceParameter(parameters[3]);
                                objQuad.Index3 = ParseFaceParameter(parameters[4]);
                                /// set the tangent vectors of the vertices of the quad
                                setQuadTangents(objQuad);
                                objQuads.Add(objQuad);
                                break;
                        }
                        break;
                }
            }
            mesh.vertices = objVertices.ToArray();
            mesh.triangles = objTriangles.ToArray();
            mesh.quads = objQuads.ToArray();
            vertices = null;
            normals = null;
            texCoords = null;
            objVertices = null;
            objTriangles = null;
            objQuads = null;
        }

        char[] faceParamaterSplitter = new char[] { '/' };
        int ParseFaceParameter(string faceParameter)
        {
            Vector3 vertex = new Vector3();
            Vector2 texCoord = new Vector2();
            Vector3 normal = new Vector3();
            string[] parameters = faceParameter.Split(faceParamaterSplitter);
            int vertexIndex = int.Parse(parameters[0]);
            if (vertexIndex < 0) vertexIndex = vertices.Count + vertexIndex;
            else vertexIndex = vertexIndex - 1;
            vertex = vertices[vertexIndex];
            if (parameters.Length > 1) if (parameters[1] != "")
                {
                    int texCoordIndex = int.Parse(parameters[1]);
                    if (texCoordIndex < 0) texCoordIndex = texCoords.Count + texCoordIndex;
                    else texCoordIndex = texCoordIndex - 1;
                    texCoord = texCoords[texCoordIndex];
                }
            if (parameters.Length > 2)
            {
                int normalIndex = int.Parse(parameters[2]);
                if (normalIndex < 0) normalIndex = normals.Count + normalIndex;
                else normalIndex = normalIndex - 1;
                normal = normals[normalIndex];
            }
            return AddObjVertex(ref vertex, ref texCoord, ref normal);
        }

        int AddObjVertex(ref Vector3 vertex, ref Vector2 texCoord, ref Vector3 normal)
        {
            Mesh.ObjVertex newObjVertex = new Mesh.ObjVertex();
            newObjVertex.Vertex = vertex;
            newObjVertex.TexCoord = texCoord * 4;
            newObjVertex.Normal = normal;
            objVertices.Add(newObjVertex);
            return objVertices.Count - 1;
        }

        private void setTriangleTangents(Mesh.ObjTriangle triangle)
        {
            // create a triangle with vertices with a tangent vector
            /// get vertices
            var vertex0 = objVertices[triangle.Index0];
            var vertex1 = objVertices[triangle.Index1];
            var vertex2 = objVertices[triangle.Index2];
            /// set the tangent vector for each vertex
            objVertices[triangle.Index0] = ComputeVertexWithTangent(vertex0, vertex1, vertex2);
            objVertices[triangle.Index1] = ComputeVertexWithTangent(vertex1, vertex0, vertex2);
            objVertices[triangle.Index2] = ComputeVertexWithTangent(vertex2, vertex0, vertex1);
        }

        private void setQuadTangents(Mesh.ObjQuad quad)
        {
            // create a quad with vertices with a tangent vector
            /// get vertices
            var vertex0 = objVertices[quad.Index0];
            var vertex1 = objVertices[quad.Index1];
            var vertex2 = objVertices[quad.Index2];
            var vertex3 = objVertices[quad.Index3];
            /// set the tangent vector for each vertex
            objVertices[quad.Index0] = ComputeVertexWithTangent(vertex0, vertex1, vertex3);
            objVertices[quad.Index1] = ComputeVertexWithTangent(vertex1, vertex0, vertex2);
            objVertices[quad.Index2] = ComputeVertexWithTangent(vertex2, vertex1, vertex3);
            objVertices[quad.Index3] = ComputeVertexWithTangent(vertex3, vertex2, vertex0);
        }

        private Mesh.ObjVertex ComputeVertexWithTangent(Mesh.ObjVertex v0, Mesh.ObjVertex v1, Mesh.ObjVertex v2)
        {
            // compute the tangent of v0 using 2 other vertices which form a triangle
            var edge1 = v1.Vertex - v0.Vertex;
            var edge2 = v2.Vertex - v0.Vertex;
            var deltaUV1 = v1.TexCoord - v0.TexCoord;
            var deltaUV2 = v2.TexCoord - v0.TexCoord;
            float f = 1.0f / (deltaUV1.X * deltaUV2.Y - deltaUV2.X * deltaUV1.Y);
            v0.Tangent = Vector3.Normalize(f * (deltaUV2.Y * edge1 - deltaUV1.Y * edge2));
            /// return the new vertex with a computed tangent
            return v0;
        }
    }
} // namespace Template_P3
