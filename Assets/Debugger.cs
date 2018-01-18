using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Threading;
using Assets.Threading.Testing;
using Assets.Collections;
using Assets;
using Assets.Chunks;
using Assets.Chunks.Threading;

public class Debugger : MonoBehaviour {

    int threads = 2;
    int receivedResults = 0;

    int lastVal = 1;

    WorkBuffer[] buffers;

    GameObject go;
    Mesh m;
    MeshFilter mf;
    MeshRenderer mr;

    // Use this for initialization
    void Start()
    {
        ChunkHandler.ViewDistance = 15;

        ThreadHandler.InitThreads(threads);
        Debug.Log($"Initiated {threads} thread{((threads > 1) ? "s" : "")}");
        ThreadHandler.StartThreads();

        ThreadHandler.AllowWorkerThreadsToCollectWork = true;

        BlockCollection.CreateCollection();
        ShapeCollection.CreateCollection();

        ChunkHandler.DrawAllChunksWithinViewDistance();

        //TestEverything();
        //TestSettingAndGettingBlock(2, 2, 2, 1);
    }
	
	// Update is called once per frame
	void Update () {
        ThreadHandler.TryIssueWorkToThreads(1);
    }

    private void LateUpdate()
    {
        ThreadHandler.RetrieveCompletedWork();
        ThreadHandler.ReadAllCompletedWork();
    }


    public void TestChunkGeneration(Vector3 cPos)
    {
        ThreadedChunkGeneration tcg = new ThreadedChunkGeneration(cPos);
        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

        watch.Start();
        CompletedChunkGenerationWork ccgw = tcg.Work() as CompletedChunkGenerationWork;
        watch.Stop();

        Debug.Log("Completed generation of chunk " + cPos.ToString() + " in " + ((double)watch.Elapsed.Ticks / (double)System.TimeSpan.TicksPerMillisecond).ToString() + "ms");


        ChunkHandler.TryAddChunk(ccgw.Chunk);
        
    }

    void TestEverything()
    {
        
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                for (int z = -1; z < 2; z++)
                {
                    TestChunkGeneration(new Vector3(x, y, z));
                }
            }
        }
        
        Chunk c;
        ChunkHandler.TryGetChunk(0, 0, 0, out c);


        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

        ThreadedPendingDrawingWork tpdw = new ThreadedPendingDrawingWork(new Vector3(0, 0, 0));

        watch.Start();
        tpdw.Work();
        watch.Stop();
        Debug.Log("Completed chunk Drawing check in " + ((double)watch.Elapsed.Ticks / (double)System.TimeSpan.TicksPerMillisecond).ToString() + "ms");

        MeshData md;
        watch.Reset();
        watch.Start();
        new ThreadedSubChunkDrawingWork(c.GetSubChunk(0, 0, 0), c.ChunkPos).Work();
        new ThreadedSubChunkDrawingWork(c.GetSubChunk(0, 0, 1), c.ChunkPos).Work();
        new ThreadedSubChunkDrawingWork(c.GetSubChunk(0, 1, 0), c.ChunkPos).Work();
        new ThreadedSubChunkDrawingWork(c.GetSubChunk(1, 0, 0), c.ChunkPos).Work();

        new ThreadedSubChunkDrawingWork(c.GetSubChunk(1, 1, 0), c.ChunkPos).Work();
        new ThreadedSubChunkDrawingWork(c.GetSubChunk(0, 1, 1), c.ChunkPos).Work();
        new ThreadedSubChunkDrawingWork(c.GetSubChunk(1, 0, 1), c.ChunkPos).Work();
        new ThreadedSubChunkDrawingWork(c.GetSubChunk(1, 1, 1), c.ChunkPos).Work();

        watch.Stop();
        Debug.Log("Completed 1st chunk Drawing in " + ((double)watch.Elapsed.Ticks / (double)System.TimeSpan.TicksPerMillisecond).ToString() + "ms");

        GameObject go = new GameObject();
        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.material = Resources.Load<Material>("TestMat");
        Mesh m = new Mesh();
        md = c.GetMesh();
        m.vertices = md.Vertices;
        m.triangles = md.Triangles;
        m.uv = md.UVs;
        m.RecalculateNormals();
        mf.mesh = m;

        watch.Reset();
        watch.Start();
        new ThreadedChunkDrawing(c).Work();
        watch.Stop();
        Debug.Log("Completed 2nd chunk Drawing in " + ((double)watch.Elapsed.Ticks / (double)System.TimeSpan.TicksPerMillisecond).ToString() + "ms");
        
    }

    void TestViewDistanceLoader()
    {
        CompletedViewDistanceLoaderWork cvdlw = new ThreadedViewDistanceLoaderWork(15, new Vector3(0, 0, 0),ChunkHandler.PlayerHasMoved).Work() as CompletedViewDistanceLoaderWork;
        cvdlw = new ThreadedViewDistanceLoaderWork(15, new Vector3(0, 0, 0), ChunkHandler.PlayerHasMoved).Work() as CompletedViewDistanceLoaderWork;
    }

   

    void CalcLocalizedChunkAndBlockPos(int x, int y, int z, out Vector3 chunkPos, out Vector3 blockPos)
    {
        chunkPos = new Vector3(1, 1, 1);
        blockPos = new Vector3(x, y, z);

        if (x < 0)
        {
            chunkPos.x = 0;
            blockPos.x = (Chunk.CHUNKSIZE - 1) + x;
        }
        else if (x > Chunk.CHUNKSIZE - 1)
        {
            chunkPos.x = 1;
            blockPos.x = x - Chunk.CHUNKSIZE;
        }

        if (y < 0)
        {
            chunkPos.y = 0;
            blockPos.y = (Chunk.CHUNKSIZE - 1) + y;
        }
        else if (y > Chunk.CHUNKSIZE - 1)
        {
            chunkPos.y = 1;
            blockPos.y = y - Chunk.CHUNKSIZE;
        }

        if (z < 0)
        {
            chunkPos.z = 0;
            blockPos.z = (Chunk.CHUNKSIZE - 1) + z;
        }
        else if (z > Chunk.CHUNKSIZE - 1)
        {
            chunkPos.z = 1;
            blockPos.z = z - Chunk.CHUNKSIZE;
        }
    }
}
