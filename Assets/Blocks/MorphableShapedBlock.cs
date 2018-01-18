using Assets.Shapes;
using Assets.Shapes.Faces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Blocks
{
    public class MorphableShapedBlock : Block
    {
        protected MorphableShapedBlock(int blockId) : base(blockId)
        {

        }

        public override MorphableEquivelent GetMorphableEquivelent(Direction dir)
        {
            return MorphableEquivelent.Cube;
        }

        internal override int DetermineShape(int x, int y, int z, Chunk c)
        {
            return 1;
        }
    }
}
