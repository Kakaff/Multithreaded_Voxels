using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.World
{
    public static class ChunkGameObjectHandler
    {

        static Dictionary<Vector3, GameObject> ActiveGameObjects = new Dictionary<Vector3, GameObject>();
        static List<GameObject> gameObjectPool = new List<GameObject>();

        public static Vector3[] GetDrawnChunkPositions
        {
            get
            {
                return ActiveGameObjects.Keys.ToArray();
            }
        }

        public static GameObject TryGetChunkGameObject(Vector3 chunkPos)
        {
            GameObject go;
            ActiveGameObjects.TryGetValue(chunkPos, out go);

            return go;
        }

        public static GameObject GetChunkGameObject(Vector3 chunkPos, MeshData md)
        {
            GameObject go = GetGameObject(chunkPos);
            UpdateMesh(chunkPos, md);

            return go;
        }

        public static GameObject UpdateMesh(Vector3 chunkPos, MeshData md)
        {
            GameObject go;

            if (!ActiveGameObjects.TryGetValue(chunkPos, out go))
            {
                go = GetGameObject(chunkPos);
            }

            MeshFilter mf = go.GetComponent<MeshFilter>();

            Mesh m = new Mesh();

            m.vertices = md.Vertices;
            m.triangles = md.Triangles;
            m.uv = md.UVs;
            m.RecalculateNormals();
            mf.mesh = m;

            md.Dispose();
            return go;
        }

        static GameObject GetGameObject(Vector3 chunkPos)
        {
            if (gameObjectPool.Count == 0)
            {
                GameObject go = new GameObject();
                go.AddComponent<MeshFilter>();
                MeshRenderer mr = go.AddComponent<MeshRenderer>();
                mr.material = Resources.Load<Material>("TestMat");
                go.name = "Chunk " + chunkPos;
                ActiveGameObjects.Add(chunkPos, go);
                return go;
            } else
            {
                GameObject go = gameObjectPool.Last();
                gameObjectPool.RemoveAt(gameObjectPool.Count - 1);
                go.SetActive(true);
                go.name = "Chunk " + chunkPos;
                ActiveGameObjects.Add(chunkPos, go);
                return go;
            }
        }

        static void PoolGameObject(GameObject go)
        {
            gameObjectPool.Add(go);
            go.SetActive(false);
            go.GetComponent<MeshFilter>().sharedMesh.Clear(false);
            GameObject.Destroy(go.GetComponent<MeshFilter>().sharedMesh);
            //Remove the mesh
        }

        public static void PoolChunk(Vector3 chunkPos)
        {
            GameObject go;
            ActiveGameObjects.TryGetValue(chunkPos, out go);

            if (go != null)
            {
                PoolGameObject(go);
                ActiveGameObjects.Remove(chunkPos);
            }
        }

        public static void PoolChunks(params Vector3[] chunkPos)
        {
            foreach (Vector3 v in chunkPos)
            {
                PoolChunk(v);
            }
        }

        public static void UpdateChunkPositions(Vector3 playerPosition)
        {
            foreach (GameObject go in ActiveGameObjects.Values)
            {
                //Set the position depending on the players position.
            }
        }
    }
}
