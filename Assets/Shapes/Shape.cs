using Assets.Shapes.Faces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Shapes
{
    public class Shape
    {
        protected Face EastFace, WestFace, NorthFace, SouthFace, UpFace, DownFace;
        int id;

        public int ID
        {
            get
            {
                return id;
            }
        }

        protected Shape(int id)
        {
            this.id = id;
        }

        public virtual bool IsSolid(Block b, Direction dir)
        {

            switch (dir)
            {
                case Direction.East:
                    {
                        return EastFace.IsSeeThrough | b.IsTransparent(Direction.East);
                    }
                case Direction.West:
                    {
                        return WestFace.IsSeeThrough | b.IsTransparent(Direction.West);
                    }
                case Direction.Up:
                    {
                        return UpFace.IsSeeThrough | b.IsTransparent(Direction.Up);
                    }
                case Direction.Down:
                    {
                        return DownFace.IsSeeThrough | b.IsTransparent(Direction.Down);
                    }
                case Direction.North:
                    {
                        return NorthFace.IsSeeThrough | b.IsTransparent(Direction.North);
                    }
                case Direction.South:
                    {
                        return SouthFace.IsSeeThrough | b.IsTransparent(Direction.South);
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        public virtual bool IsHiddenBy(Shape s, Direction dir)
        {
            return false;
        }

        public Face GetFace(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    {
                        return UpFace;
                    }
                case Direction.Down:
                    {
                        return DownFace;
                    }
                case Direction.East:
                    {
                        return EastFace;
                    }
                case Direction.West:
                    {
                        return WestFace;
                    }
                case Direction.North:
                    {
                        return NorthFace;
                    }
                case Direction.South:
                    {
                        return SouthFace;
                    }
                default:
                    {
                        return new Face_Empty();
                    }
            }
        }

        public void Draw(int x, int y, int z, MeshData md, Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    {
                        UpFace.DrawUp(x,y,z,md);
                        break;
                    }
                case Direction.Down:
                    {
                        DownFace.DrawDown(x,y,z,md);
                        break;
                    }
                case Direction.East:
                    {
                        EastFace.DrawEast(x,y,z,md);
                        break;
                    }
                case Direction.West:
                    {
                        WestFace.DrawWest(x,y,z,md);
                        break;
                    }
                case Direction.North:
                    {
                        NorthFace.DrawNorth(x,y,z,md);
                        break;
                    }
                case Direction.South:
                    {
                        SouthFace.DrawSouth(x,y,z,md);
                        break;
                    }
            }
        }
    }
}
