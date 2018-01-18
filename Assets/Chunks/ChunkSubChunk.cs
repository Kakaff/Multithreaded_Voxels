using Assets.Collections;
using Assets.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Chunks
{
    public class ChunkSubChunk
    {

        int[,,] blocks;
        int[,,] shapes;
        Vector3 pos;
        int size;
        bool isShapeChecked;
        bool needsToBeRedrawn;
        Chunk parent;
        MeshData md;

        public bool NeedsToBeDrawn
        {
            get
            {
                return needsToBeRedrawn;
            }
        }

        public MeshData MeshData
        {
            get
            {
                return md;
            }
        }

        public ChunkSubChunk(int size, Vector3 pos, Chunk parent)
        {
            this.parent = parent;
            this.size = size;
            this.pos = pos;
            blocks = new int[size, size, size];
            shapes = new int[size, size, size];
            needsToBeRedrawn = true;
        }

        public Vector3 Pos
        {
            get
            {
                return pos;
            }
        }

        public int GetBlockIDUnsafe(int x, int y, int z)
        {
            return blocks[x, y, z];
        }

        public int GetShapeIDUnsafe(int x, int y, int z)
        {
            return shapes[x, y, z];
        }

        public void SetBlockUnsafe(int x, int y, int z, int blockID)
        {
            blocks[x, y, z] = blockID;
        }

        public void SetShapeUnsafe(int x, int y, int z, int shapeID)
        {
            shapes[x, y, z] = shapeID;
        }

        public Block GetBlock(int x, int y, int z)
        {
            Block b;
            BlockCollection.TryGetBlock(GetBlockIDUnsafe(x, y, z), out b);

            return b;
        }

        public Shape GetShape(int x, int y, int z)
        {
            Shape s;
            ShapeCollection.TryGetShape(GetShapeIDUnsafe(x, y, z), out s);
            return s;
        }

        public MeshData Draw()
        {
            md = new MeshData();
            int minX = 1 - (int)pos.x, maxX = 9 - (int)pos.x, xPosMod = (int)(pos.x * 8) - minX;
            int minY = 1 - (int)pos.y, maxY = 9 - (int)pos.y, yPosMod = (int)(pos.y * 8) - minY;
            int minZ = 1 - (int)pos.z, maxZ = 9 - (int)pos.z, zPosMod = (int)(pos.z * 8) - minZ;

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    for (int z = minZ; z < maxZ; z++)
                    {
                        GetBlock(x, y, z).Draw(x + xPosMod, y + yPosMod, z + zPosMod, GetShape(x, y, z), md, parent);
                    }
                }
            }

            needsToBeRedrawn = false;
            return md;
        }

        public virtual void Dispose()
        {
            if (md != null)
            {
                md.Dispose();
                md = null;
            }

            parent = null;
            blocks = null;
            shapes = null;

        }
    }
}
