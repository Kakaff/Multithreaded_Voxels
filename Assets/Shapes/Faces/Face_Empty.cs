using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Shapes.Faces
{
    public class Face_Empty : Face
    {
        public Face_Empty() : base(0,true)
        {
           
        }

        public override bool IsHiddenBy(Face f)
        {
            return true;
        }
    }
}
