using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Shapes.Faces
{
    public class Face_Quad : Face
    {

        public Face_Quad() : base(1,false)
        {

        }

        public override void DrawUp(int x, int y, int z, MeshData md)
        {
            Vector3 origin = new Vector3(x, y, z);

            md.AddQuad(origin + new Vector3(-0.5f, 0.5f, 0.5f),
                       origin + new Vector3(0.5f, 0.5f, 0.5f),
                       origin + new Vector3(0.5f, 0.5f, -0.5f),
                       origin + new Vector3(-0.5f, 0.5f, -0.5f));
            md.AddQuadUV(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));


        }

        public override void DrawDown(int x, int y, int z, MeshData md)
        {
            Vector3 origin = new Vector3(x, y, z);
            md.AddQuad(origin + new Vector3(-0.5f, -0.5f, -0.5f),
                       origin + new Vector3(0.5f, -0.5f, -0.5f),
                       origin + new Vector3(0.5f, -0.5f, 0.5f),
                       origin + new Vector3(-0.5f, -0.5f, 0.5f));
            md.AddQuadUV(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));
        }

        public override void DrawEast(int x, int y, int z, MeshData md)
        {
            Vector3 origin = new Vector3(x, y, z);

            md.AddQuad(origin + new Vector3(0.5f, 0.5f, -0.5f),
                       origin + new Vector3(0.5f, 0.5f, 0.5f),
                       origin + new Vector3(0.5f, -0.5f, 0.5f),
                       origin + new Vector3(0.5f, -0.5f, -0.5f));
            md.AddQuadUV(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));


        }

        public override void DrawWest(int x, int y, int z, MeshData md)
        {
            Vector3 origin = new Vector3(x, y, z);

            md.AddQuad(origin + new Vector3(-0.5f,0.5f,0.5f),
                       origin + new Vector3(-0.5f,0.5f,-0.5f),
                       origin + new Vector3(-0.5f,-0.5f,-0.5f),
                       origin + new Vector3(-0.5f,-0.5f,0.5f));
            md.AddQuadUV(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));


        }

        public override void DrawNorth(int x, int y, int z, MeshData md)
        {
            Vector3 origin = new Vector3(x, y, z);

            md.AddQuad(origin + new Vector3(0.5f,0.5f,0.5f),
                       origin + new Vector3(-0.5f,0.5f,0.5f),
                       origin + new Vector3(-0.5f,-0.5f,0.5f),
                       origin + new Vector3(0.5f,-0.5f,0.5f));
            md.AddQuadUV(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));
        }

        public override void DrawSouth(int x, int y, int z, MeshData md)
        {
            Vector3 origin = new Vector3(x, y, z);

            md.AddQuad(origin + new Vector3(-0.5f,0.5f,-0.5f),
                       origin + new Vector3(0.5f,0.5f,-0.5f),
                       origin + new Vector3(0.5f,-0.5f,-0.5f),
                       origin + new Vector3(-0.5f,-0.5f,-0.5f));
            md.AddQuadUV(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));
        }
    }
}
