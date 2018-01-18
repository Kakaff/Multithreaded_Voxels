using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets
{
    public class MeshData
    {
        List<Vector3> vertices;
        List<Vector2> uvs;
        List<int> triangles;

        public int[] Triangles
        {
            get
            {
                return triangles.ToArray();
            }
        }

        public Vector3[] Vertices
        {
            get
            {
                return vertices.ToArray();
            }
        }

        public Vector2[] UVs
        {
            get
            {
                return uvs.ToArray();
            }
        }

        public MeshData()
        {
            vertices = new List<Vector3>();
            uvs = new List<Vector2>();
            triangles = new List<int>();
        }

        public MeshData(Vector3[] verts, int[] triangs, Vector2[] uv)
        {
            vertices = new List<Vector3>(verts);
            triangles = new List<int>(triangs);
            uvs = new List<Vector2>(uv);
        }

        public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);

            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 1);
        }

        public void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            vertices.Add(v4);

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 1);
        }

        public void AddQuadUV(Vector2 uv1,Vector2 uv2, Vector2 uv3, Vector2 uv4)
        {
            uvs.Add(uv1);
            uvs.Add(uv2);
            uvs.Add(uv3);
            uvs.Add(uv4);
        }

        public void AddTriangleUV(Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {

        }

        public static MeshData Combine(params MeshData[] meshDatas)
        {

            MeshData m3 = new MeshData();

            int currCount = 0;
            foreach (MeshData md in meshDatas)
            {
                m3.vertices.AddRange(md.vertices);
                m3.uvs.AddRange(md.uvs);

                foreach (int i in md.triangles)
                {
                    m3.triangles.Add(i + currCount);
                }

                currCount = m3.vertices.Count;
                
            }

            return m3;
        }

        public void Dispose()
        {
            vertices.Clear();
            vertices = null;
            triangles.Clear();
            triangles = null;
            uvs.Clear();
            uvs = null;
        }
    }
}
