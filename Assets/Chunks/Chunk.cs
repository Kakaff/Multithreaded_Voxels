using Assets.Chunks;
using Assets.Chunks.Threading;
using Assets.Collections;
using Assets.Shapes;
using Assets.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets
{
    public class Chunk
    {

        public bool IsGenerated
        {
            get
            {
                return isGenerated;
            }
        }

        public const int CHUNKSIZE = 16;

        //protected int[,,] blocks;
        //protected int[,,] shapes;
        protected Vector3 _chunkPos;
        protected ChunkSubChunk[,,] subChunks;

        protected bool hasSurroundingBlocks;
        protected bool isDrawn;
        protected bool isShapeChecked;
        protected bool isGenerated;
        protected bool[] ChunkEdgeSolidity;
        bool isDisposed = false;

        public bool IsDrawn
        {
            get
            {
                return isDrawn;
            } set
            {
                isDrawn = value;
            }
        }

        public bool IsShapeChecked
        {
            get
            {
                return isShapeChecked;
            }
        }

        public bool HasSurroundingBlocks
        {
            get
            {
                return hasSurroundingBlocks;
            }
            set
            {
                hasSurroundingBlocks = value;
            }
        }

        public Vector3 ChunkPos
        {
            get
            {
                return _chunkPos;
            }
        }

        public Chunk(Vector3 chunkPos)
        {
            hasSurroundingBlocks = false;
            isDrawn = false;
            isShapeChecked = false;
            _chunkPos = chunkPos;
            ChunkEdgeSolidity = new bool[6];
            CreateSubChunks(9);
        }

        public Chunk(int x, int y, int z)
        {
            hasSurroundingBlocks = false;
            isDrawn = false;
            isShapeChecked = false;
            _chunkPos = new Vector3(x,y,z);
            ChunkEdgeSolidity = new bool[6];
            CreateSubChunks(9);
        }

        void CreateSubChunks(int size)
        {
            subChunks = new ChunkSubChunk[2, 2, 2];

            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int z = 0; z < 2; z++)
                    {
                        subChunks[x, y, z] = new ChunkSubChunk(9, new Vector3(x, y, z),this);
                    }
                }
            }
        }

        internal ThreadedSubChunkDrawingWork[] GetSubChunksToDraw()
        {
            List<ThreadedSubChunkDrawingWork> wList = new List<ThreadedSubChunkDrawingWork>();

            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int z = 0; z < 2; z++)
                    {
                        if (subChunks[x, y, z].NeedsToBeDrawn)
                            wList.Add(new ThreadedSubChunkDrawingWork(subChunks[x, y, z], ChunkPos));
                    }
                }
            }

            return wList.ToArray();
        }

        void SetBlockInSubChunk(int x, int y, int z,int BlockID)
        {
            int tX = x / 9;
            int tY = y / 9;
            int tZ = z / 9;

            subChunks[tX, tY, tZ].SetBlockUnsafe(x - (tX * 9), y - (tY * 9), z - (tZ * 9), BlockID);
        }

        int GetBlockIDInSubChunk(int x, int y, int z)
        {
            int tX = x / 9;
            int tY = y / 9;
            int tZ = z / 9;

            return subChunks[tX, tY, tZ].GetBlockIDUnsafe(x - (tX * 9), y - (tY * 9), z - (tZ * 9));
        }

        void SetShapeInSubChunk(int x, int y, int z, int ShapeID)
        {
            int tX = x / 9;
            int tY = y / 9;
            int tZ = z / 9;
            subChunks[tX, tY, tZ].SetShapeUnsafe(x - (tX * 9), y - (tY * 9), z - (tZ * 9), ShapeID);
        }

        int GetShapeIDInSubChunk(int x, int y, int z)
        {
            int tX = x / 9;
            int tY = y / 9;
            int tZ = z / 9;

            return subChunks[tX, tY, tZ].GetShapeIDUnsafe(x - (tX * 9), y - (tY * 9), z - (tZ * 9));
        }
        
        public Block GetBlock(Vector3 pos)
        {
            return GetBlock((int)pos.x,(int)pos.y,(int)pos.z);
        }
        public Shape GetShape(Vector3 pos)
        {
            return GetShape((int)pos.x, (int)pos.y, (int)pos.z);
        }

        public static Vector3 GetChunkPos(int x, int y, int z)
        {
            return new Vector3(Mathf.FloorToInt((float)x / (float)CHUNKSIZE), 
                               Mathf.FloorToInt((float)y / (float)CHUNKSIZE), 
                               Mathf.FloorToInt((float)z / (float)CHUNKSIZE));
        }

        public static Vector3 GetChunkPos(Vector3 v)
        {
            return GetChunkPos((int)v.x, (int)v.y, (int)v.z);
        }

        public static Vector3 GetBlockPos(int x, int y, int z)
        {
            Vector3 chunkPos = GetChunkPos(x, y, z);
            return new Vector3(x - (chunkPos.x * CHUNKSIZE),
                               y - (chunkPos.y * CHUNKSIZE),
                               z - (chunkPos.z * CHUNKSIZE));
        }

        public static Vector3 GetBlockPos(Vector3 v)
        {
            return GetBlockPos((int)v.x, (int)v.y, (int)v.z);
        }

        public virtual Block GetBlock(int x, int y, int z)
        {
            if (InRange(x,y,z, 0, CHUNKSIZE - 1))
            {
                return GetBlockUnsafe(x+1,y+1,z+1);
            } else
            {
                return ChunkHandler.GetBlock(_chunkPos,x, y, z);
            }
        }

        internal Block GetBlockUnsafe(int x, int y, int z)
        {
            Block b;
            BlockCollection.TryGetBlock(GetBlockIDUnsafe(x,y,z), out b);

            return b;
        }

        internal int GetBlockIDUnsafe(int x, int y, int z)
        {
            return GetBlockIDInSubChunk(x, y, z);
        }

        public MeshData GetMesh()
        {
            return MeshData.Combine(subChunks[0, 0, 0].MeshData, subChunks[0, 0, 1].MeshData, subChunks[0, 1, 0].MeshData, subChunks[1, 0, 0].MeshData,
                                           subChunks[0, 1, 1].MeshData, subChunks[1, 0, 1].MeshData, subChunks[1, 1, 0].MeshData, subChunks[1, 1, 1].MeshData);
        }

        public ChunkSubChunk GetSubChunk(int x, int y, int z)
        {
            return subChunks[x, y, z];
        }

        public ChunkSubChunk GetSubChunk(Vector3 v)
        {
            return GetSubChunk((int)v.x, (int)v.y, (int)v.z);
        }

        public virtual void SetBlock(int x, int y, int z, int blockID)
        {
            if (InRange(x, y, z, 0, CHUNKSIZE - 1))
            {
                SetBlockUnsafe(x+1, y+1, z+1,blockID);

                //Check if at edge of chunk.
                Vector3 bPos,cPos;

                if (OutOfRange(x, y, z, 1, CHUNKSIZE - 2,out bPos, out cPos))
                {
                    Chunk c;
                    ChunkHandler.TryGetChunk(_chunkPos + cPos, out c);
                    c.SetBlockUnsafe((int)bPos.x, (int)bPos.y,(int)bPos.z, blockID);
                }
            }
            else
            {
                ChunkHandler.SetBlock(_chunkPos, x, y, z, blockID);
            }
        }

        internal void SetShape(int x, int y, int z, int shapeID)
        {
            if (InRange(x,y,z,0,CHUNKSIZE - 1))
            {
                SetShapeUnsafe(x+1, y+1, z+1,shapeID);

                //Check if at edge of chunk.
                Vector3 bPos,cPos;
                if (OutOfRange(x, y, z, 1, CHUNKSIZE - 2, out bPos, out cPos))
                {
                    Chunk c;
                    ChunkHandler.TryGetChunk(_chunkPos + cPos, out c);
                    c.SetShapeUnsafe((int)bPos.x, (int)bPos.y, (int)bPos.z, shapeID);
                }
            }
            else
            {
                ChunkHandler.SetShape(_chunkPos, x, y, z, shapeID);
            }
        }

        internal void SetShape(Vector3 pos, int shapeID)
        {
            SetShape((int)pos.x, (int)pos.y, (int)pos.z, shapeID);
        }

        internal void SetShapeUnsafe(int x, int y, int z, int shapeID)
        {
            SetShapeInSubChunk(x, y, z, shapeID);
            //shapes[x, y, z] = shapeID;
        }

        internal void SetBlockUnsafe(int x, int y, int z, int BlockID)
        {
            isShapeChecked = false;
            //blocks[x, y, z] = BlockID;
            SetBlockInSubChunk(x, y, z, BlockID);
        }

        internal void SetBlockUnsafeAndCalculateShapes(int x, int y, int z, int BlockID)
        {
            throw new Exception("Method not yet implemented");
        }

        public virtual Shape GetShape(int x, int y , int z)
        {
            if (InRange(x,y,z,0,CHUNKSIZE-1))
            {
                return GetShapeUnsafe(x+1,y+1,z+1);
            }

            return ChunkHandler.GetShape(_chunkPos, x, y, z);
        }

        internal Shape GetShapeUnsafe(int x, int y, int z)
        {
            Shape s;
            ShapeCollection.TryGetShape(GetShapeIDUnsafe(x,y,z), out s);

            return s;
        }

        internal int GetShapeIDUnsafe(int x, int y, int z)
        {
            return GetShapeIDInSubChunk(x, y, z);
        }

        public MeshData Draw()
        {
            MeshData md = new MeshData();
            

            return md;
        }

        public bool IsSolid(Direction dir)
        {
            return ChunkEdgeSolidity[(int)dir];
        }

        public void CalculateShapes()
        {
            for (int x = 1; x < CHUNKSIZE + 1; x++)
            {
                for (int y = 1; y < CHUNKSIZE + 1; y++)
                {
                    for (int z = 1; z < CHUNKSIZE + 1; z++)
                    {
                        SetShapeInSubChunk(x,y,z,GetBlockUnsafe(x, y, z).DetermineShape(x-1,y-1,z-1,this));
                    }
                }
            }

            isShapeChecked = true;
        }

        /// <summary>
        /// Flags this chunk as being generated.
        /// </summary>
        public void SetGenerated()
        {
            isGenerated = true;
        }

        public void GetNeighboringBlocksAndShapes()
        {

        }

        public void CalculateEdgeSolidity()
        {
            bool EastSolidity = true, WestSolidity = true, UpSolidity = true, DownSolidity = true, NorthSolidity = true, SouthSolidity = true;

            //Should be moved to be in subchunks, so that subchunks can be culled instead of the whole chunk.

            //Check east
            for (int x = 1; x < Chunk.CHUNKSIZE + 1; x++)
            {
                for (int z = 1; z < Chunk.CHUNKSIZE +1; z++)
                {
                    if (EastSolidity && !GetBlockUnsafe(Chunk.CHUNKSIZE, x, z).IsSolid(Direction.East,GetShapeUnsafe(Chunk.CHUNKSIZE,x,z)))
                        EastSolidity = false;

                    if (WestSolidity && !GetBlockUnsafe(1, x, z).IsSolid(Direction.West, GetShapeUnsafe(1, x, z)))
                        WestSolidity = false;

                    if (UpSolidity && !GetBlockUnsafe(x, Chunk.CHUNKSIZE, z).IsSolid(Direction.Up, GetShapeUnsafe(x, Chunk.CHUNKSIZE, z)))
                        UpSolidity = false;

                    if (DownSolidity && !GetBlockUnsafe(x, 1, z).IsSolid(Direction.Down, GetShapeUnsafe(x, 1, z)))
                        DownSolidity = false;

                    if (NorthSolidity && !GetBlockUnsafe(x, z, Chunk.CHUNKSIZE).IsSolid(Direction.North, GetShapeUnsafe(x, z, Chunk.CHUNKSIZE)))
                        NorthSolidity = false;

                    if (SouthSolidity && !GetBlockUnsafe(x, z, 1).IsSolid(Direction.South, GetShapeUnsafe(x, z, 1)))
                        SouthSolidity = false;
                }
            }

            ChunkEdgeSolidity[(int)Direction.East] = EastSolidity;
            ChunkEdgeSolidity[(int)Direction.West] = WestSolidity;
            ChunkEdgeSolidity[(int)Direction.Up] = UpSolidity;
            ChunkEdgeSolidity[(int)Direction.Down] = DownSolidity;
            ChunkEdgeSolidity[(int)Direction.North] = UpSolidity;
            ChunkEdgeSolidity[(int)Direction.South] = DownSolidity;

        }

        public static bool InRange(int i, int min, int max)
        {
            if (i < min || i > max)
                return false;

            return true;
        }

        public static bool InRange(int i, int j, int min, int max)
        {
            if (i < min || i > max ||
                j < min || j > max)
                return false;

            return true;
        }

        public static bool InRange(int i, int j, int k, int min, int max)
        {
            if (i < min || i > max ||
                j < min || j > max ||
                k < min || k > max)
                return false;

            return true;
        }

        public static bool OutOfRange(int i, int j, int k, int min, int max)
        {
            return !InRange(i, j, k, min, max);
        }

        public static bool OutOfRange(int i, int j, int k, int min, int max, out Vector3 bPos)
        {
            bool res = !InRange(i,j,k,min,max);
            bPos = new Vector3(i,j,k);
            if (res)
            {
                if (i < 1)
                    bPos.x = CHUNKSIZE + i;
                else if (i > CHUNKSIZE - 1)
                    bPos.x = i - CHUNKSIZE;

                if (j < 1)
                    bPos.y = CHUNKSIZE + j;
                else if (j > CHUNKSIZE - 1)
                    bPos.y = j - CHUNKSIZE;

                if (k < 1)
                    bPos.z = CHUNKSIZE + k;
                else if (k > CHUNKSIZE - 1)
                    bPos.z = k - CHUNKSIZE;
            }

            return res;
        }

        public static void GetChunkAndBlockPos(int x, int y, int z, out Vector3 bPos, out Vector3 cPos)
        {
            bPos = new Vector3(x, y, z);
            cPos = new Vector3(0, 0, 0);

            if (x < 0)
            {
                bPos.x = CHUNKSIZE + x;
                cPos.x = -1;
            }
            else if (x > CHUNKSIZE - 1)
            {
                bPos.x = x - CHUNKSIZE;
                cPos.x = 1;
            }

            if (y < 0)
            {
                bPos.y = CHUNKSIZE + y;
                cPos.y = -1;
            }
            else if (y > CHUNKSIZE - 1)
            {
                bPos.y = y - CHUNKSIZE;
                cPos.y = 1;
            }

            if (z < 0)
            {
                bPos.z = CHUNKSIZE + z;
                cPos.z = -1;
            }
            else if (z > CHUNKSIZE - 1)
            {
                bPos.z = z - CHUNKSIZE;
                cPos.z = 1;
            }
        }

        public static bool OutOfRange(int i, int j, int k, int min, int max, out Vector3 bPos, out Vector3 cPos)
        {
            bool res = !InRange(i, j, k, min, max);
            bPos = new Vector3(i, j, k);
            cPos = new Vector3(0, 0, 0);

            if (res)
            {
                int signI = (int)Mathf.Sign(i-1);
                int signj = (int)Mathf.Sign(j-1);
                int signk = (int)Mathf.Sign(k-1);

                cPos.x = signI;
                cPos.y = signj;
                cPos.z = signk;

                if (signI == 1)
                    bPos.x = 0;
                else if (signI == -1)
                    bPos.x = CHUNKSIZE + 1;

                if (signj == 1)
                    bPos.y = 0;
                else if (signj == -1)
                    bPos.y = CHUNKSIZE + 1;

                if (signk == 1)
                    bPos.z = 0;
                else if (signk == -1)
                    bPos.z = CHUNKSIZE + 1;
            }

            return res;
        }

        public static void GetChunksToSet(int x, int y, int z, out Vector3[] chunkPositions, out Vector3[] blockPositions)
        {
            List<Vector3> cPositions = new List<Vector3>();
            List<Vector3> bPositions = new List<Vector3>();

            int signX = (int)Mathf.Sign(x - 1);
            int signY = (int)Mathf.Sign(y - 1);
            int signZ = (int)Mathf.Sign(z - 1);

            if (x != 0)
            {
                cPositions.Add(new Vector3(x, 0, 0));
                if (y != 0)
                {
                    cPositions.Add(new Vector3(0, y, 0));
                    cPositions.Add(new Vector3(x, y, 0));
                    if (z != 0)
                    {
                        cPositions.Add(new Vector3(x, y, z));
                        cPositions.Add(new Vector3(0, 0, z));
                        cPositions.Add(new Vector3(0, y, z));
                        
                    }
                } else if (z != 0)
                {
                    cPositions.Add(new Vector3(x, 0, z));
                    cPositions.Add(new Vector3(0, 0, z));
                }
            } else if (y != 0)
            {
                cPositions.Add(new Vector3(0, y, 0));
                if (z != 0)
                {
                    cPositions.Add(new Vector3(0, y, z));
                    cPositions.Add(new Vector3(0, 0, z));
                }
            } else if (z != 0)
            {
                cPositions.Add(new Vector3(0,0,z));
            }

            chunkPositions = cPositions.ToArray();
            blockPositions = bPositions.ToArray();
        }


        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                ChunkEdgeSolidity = null;
                subChunks[0, 0, 0].Dispose();
                subChunks[0, 0, 1].Dispose();
                subChunks[0, 1, 0].Dispose();
                subChunks[1, 0, 0].Dispose();

                subChunks[0, 1, 1].Dispose();
                subChunks[1, 1, 0].Dispose();
                subChunks[1, 0, 1].Dispose();
                subChunks[1, 1, 1].Dispose();

                subChunks = null;
            }
        }

        static int GetBlockPosForUnsafe(int i)
        {
            return (i == 0) ? i : (i < 0) ? CHUNKSIZE + 1 : 0;
        }
    }
}
