using Assets.Shapes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Collections
{
    public static class ShapeCollection
    {
        static ConcurrentDictionary<int, Shape> shapeCollection = new ConcurrentDictionary<int, Shape>();


        public static void CreateCollection()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(Shape))).ToArray();

            foreach (Type t in types)
            {
                Shape s = (Shape)Activator.CreateInstance(t);

                shapeCollection.TryAdd(s.ID, s);
            }

        }

        public static bool TryGetShape(int id, out Shape s)
        {
            s = null;
            return shapeCollection.TryGetValue(id, out s);
        }
    }
}
