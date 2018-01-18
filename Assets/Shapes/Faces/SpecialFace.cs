using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Shapes.Faces
{
    public class SpecialFace
    {
        int id;

        public int ID
        {
            get
            {
                return id;
            }
        }

        public SpecialFace(int id)
        {

        }

        public virtual MeshData DrawFace(int x, int y, int z, MeshData md)
        {
            return md;
        }

        public virtual bool IsHiddenBy(Face f)
        {
            return false;
        }
    }
}
