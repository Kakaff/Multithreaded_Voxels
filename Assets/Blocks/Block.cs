using Assets.Shapes;
using Assets.Shapes.Faces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets
{
    public enum MorphableEquivelent
    {
        Cube,
        Air
    }

    public enum Direction
    {
        Up,
        Down,
        North,
        South,
        East,
        West
    }

    public class Block
    {

        int id;

        Vector2 texIndex;

        public int ID
        {
            get
            {
                return id;
            }
        }

        protected Block(int blockID)
        {
            id = blockID;
        }

        public void Draw(int x, int y, int z, Shape OriginShape, MeshData md, Chunk c)
        {
            //Check block above
            Shape s = GetShape(x, y + 1, z,c);
            
            if (!OriginShape.IsHiddenBy(s,Direction.Down))
            {
                OriginShape.Draw(x, y, z, md, Direction.Up);
            }

            s = GetShape(x, y - 1, z, c);

            if (!OriginShape.IsHiddenBy(s, Direction.Up))
            {
                OriginShape.Draw(x, y, z, md, Direction.Down);
            }

            s = GetShape(x + 1, y, z,c);

            if (!OriginShape.IsHiddenBy(s, Direction.West))
            {
                OriginShape.Draw(x, y, z, md, Direction.East);
            }

            s = GetShape(x - 1, y, z, c);

            if (!OriginShape.IsHiddenBy(s, Direction.East))
            {
                OriginShape.Draw(x, y, z, md, Direction.West);
            }

            s = GetShape(x, y, z+1, c);

            if (!OriginShape.IsHiddenBy(s, Direction.South))
            {
                OriginShape.Draw(x, y, z, md, Direction.North);
            }

            s = GetShape(x, y, z - 1, c);

            if (!OriginShape.IsHiddenBy(s, Direction.North))
            {
                OriginShape.Draw(x, y, z, md, Direction.South);
            }
        }

        public virtual MorphableEquivelent GetMorphableEquivelent(Direction dir)
        {
            return MorphableEquivelent.Cube;
        }

        public virtual bool IsTransparent(Direction dir)
        {
            return false;
        }

        public bool IsSolid(Direction dir, Shape s)
        {
            return s.IsSolid(this, dir);
        }

        internal virtual int DetermineShape(int x, int y, int z, Chunk c)
        {
            return 0;
        }

        public Block GetBlock(int x, int y, int z, Chunk c)
        {
            return c.GetBlockUnsafe(x+1, y+1, z+1);
        }

        public Shape GetShape(int x, int y, int z, Chunk c)
        {
            return c.GetShapeUnsafe(x+1, y+1, z+1);
        }

        public Block GetBlock(Vector3 pos, Chunk c)
        {
            return GetBlock((int)pos.x,(int)pos.y,(int)pos.z,c);
        }

        public Shape GetShape(Vector3 pos, Chunk c)
        {
            return GetShape((int)pos.x, (int)pos.y, (int)pos.z, c);
        }
    }
}
