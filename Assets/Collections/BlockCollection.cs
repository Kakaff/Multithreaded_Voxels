using Assets.Blocks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace Assets.Collections
{
    public static class BlockCollection
    {
        static ConcurrentDictionary<int, Block> blockCollection = new ConcurrentDictionary<int, Block>();


        public static void CreateCollection()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(MorphableShapedBlock)) || t.IsSubclassOf(typeof(FixedShapedBlock))).ToArray();

            foreach (Type t in types)
            {
                Block b = (Block)Activator.CreateInstance(t);

                blockCollection.TryAdd(b.ID, b);
            }
            
        }

        public static bool TryGetBlock(int id, out Block b)
        {
            b = null;
            return blockCollection.TryGetValue(id, out b);
        }
    }
}
