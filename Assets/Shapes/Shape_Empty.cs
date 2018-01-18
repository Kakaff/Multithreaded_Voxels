using Assets.Shapes.Faces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Shapes
{
    public class Shape_Empty : Shape
    {

        public Shape_Empty() : base(0)
        {
            WestFace = new Face_Empty();
            EastFace = new Face_Empty();
            NorthFace = new Face_Empty();
            SouthFace = new Face_Empty();
            UpFace = new Face_Empty();
            DownFace = new Face_Empty();
        }

        public override bool IsHiddenBy(Shape s, Direction dir)
        {
            return true;
        }

        public override bool IsSolid(Block b, Direction dir)
        {
            return false;
        }
    }
}
