using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Shapes;

namespace Assets.Blocks
{
    public class Block_Stone : MorphableShapedBlock
    {
        public Block_Stone() : base(1)
        {

        }

        public override bool IsTransparent(Direction dir)
        {
            return false;
        }

        
    }
}
