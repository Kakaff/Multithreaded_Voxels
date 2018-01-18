using Assets.Shapes;
using Assets.Shapes.Faces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Blocks
{
    public class Block_Air : FixedShapedBlock
    {

        public Block_Air() : base(0)
        {

        }

        public override MorphableEquivelent GetMorphableEquivelent(Direction dir)
        {
            return MorphableEquivelent.Air;
        }

        

        public override bool IsTransparent(Direction dir)
        {
            return true;
        }

    }
}
