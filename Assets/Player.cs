using Assets.Chunks;
using Assets.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets
{
    public class Player : MonoBehaviour
    {
        Vector3 lastPos;
        bool blockChanged = false;

        public float speed = 10f;
        private void Start()
        {
            lastPos = new Vector3(0, 0, 0);
        }

        private void Update()
        {

            UpdatePlayerChunkPos();
            MovePlayer();

        }

        void MoveWorld()
        {
            Vector3 cp = Chunk.GetChunkPos((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);

            if (Math.Abs(cp.x) > 2 || Math.Abs(cp.y) > 2 || Math.Abs(cp.z) > 2)
            {
                ChunkGameObjectHandler.UpdateChunkPositions(cp);
            }
        }

        void UpdatePlayerChunkPos()
        {
            
            Vector3 cPos = Chunk.GetChunkPos(transform.position);
            cPos.y = 0;

            if (cPos != lastPos)
            {
                ChunkHandler.UpdatePlayerPosition(cPos);
                lastPos = cPos;
            }
        }

        void MovePlayer()
        {
            Vector3 moveVector = new Vector3(0, 0, 0);

            if (Input.GetKey(KeyCode.W))
                moveVector.z += 1;
            if (Input.GetKey(KeyCode.S))
                moveVector.z -= 1;
            if (Input.GetKey(KeyCode.D))
                moveVector.x += 1;
            if (Input.GetKey(KeyCode.A))
                moveVector.x -= 1;
            if (Input.GetKey(KeyCode.Space))
                moveVector.y += 1;
            if (Input.GetKey(KeyCode.LeftShift))
                moveVector.y -= 1;
            Vector3 forwardVector = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            Vector3 rightVector = new Vector3(transform.right.x, 0, transform.right.z).normalized;
            transform.position = transform.position + (forwardVector * moveVector.z * Time.deltaTime * speed);
            transform.position = transform.position + (rightVector * moveVector.x * 5f * Time.deltaTime);
            transform.position = transform.position + (Vector3.up * moveVector.y * 5f * Time.deltaTime);
        }
        //Shamelessly copied from answers.unity3d.com  Answer by IJM · Oct 11, 2010 at 01:28 AM 
    }
}
