using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets
{
    public class Face
    {
        int id;
        bool isSeeThrough;

        public bool IsSeeThrough
        {
            get
            {
                return isSeeThrough;
            }
        }

        public int ID
        {
            get
            {
                return id;
            }
        }

        protected Face(int id, bool isSeeThrough)
        {
            this.id = id;
            this.isSeeThrough = isSeeThrough;
        }

        public virtual void DrawUp(int x, int y, int z, MeshData md)
        {

        }

        public virtual void DrawDown(int x, int y, int z, MeshData md)
        {

        }

        public virtual void DrawEast(int x, int y, int z, MeshData md)
        {

        }

        public virtual void DrawWest(int x, int y, int z, MeshData md)
        {

        }

        public virtual void DrawNorth(int x, int y, int z, MeshData md)
        {

        }

        public virtual void DrawSouth(int x, int y, int z, MeshData md)
        {

        }

        public virtual bool IsHiddenBy(Face f)
        {
            return false;
        }

        
    }
}
