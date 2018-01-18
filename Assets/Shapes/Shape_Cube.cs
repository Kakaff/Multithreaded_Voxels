using Assets.Shapes.Faces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Shapes
{
    public class Shape_Cube : Shape
    {

        public Shape_Cube() : base(1) {

            UpFace = new Face_Quad();
            EastFace = new Face_Quad();
            WestFace = new Face_Quad();
            SouthFace = new Face_Quad();
            NorthFace = new Face_Quad();
            DownFace = new Face_Quad();
        }

        public override bool IsHiddenBy(Shape s, Direction dir)
        {
            switch (s.ID)
            {
                case 1:
                    {
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        public override bool IsSolid(Block b, Direction dir)
        {
            return b.IsTransparent(dir);
        }
    }
}
