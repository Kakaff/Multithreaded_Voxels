using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Blocks
{
    public class FixedShapedBlock : Block
    {
        public FixedShapedBlock(int blockID) : base(blockID)
        {

        }

        public override MorphableEquivelent GetMorphableEquivelent(Direction dir)
        {
            return MorphableEquivelent.Air;
        }

        internal override int DetermineShape(int x, int y, int z, Chunk c)
        {
            return 0;
        }
    }
}
