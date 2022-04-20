using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace mikunis.spawners
{
    public class MikuniClusterSpawner : MikuniSpawner
    {
        public GameObject DummyModel;
        public GameObject MikuniModel;
        public float width = 3;
        public int spawnCount = 6;
        [Range(0, 1)]
        public float dummyProbability = 0.6f;
        public Vector3 rotationMask = Vector3.one;
        public bool stickToTerrain = true;

        private List<GameObject> SpanwedMikunis = new List<GameObject>();

        private void Start()
        {
            Init(new List<GameObject> { DummyModel, MikuniModel });
        }

        private void LateUpdate()
        {
            if (SpanwedMikunis.Count > 0) return; // TODO: Change me
            float size = width/2;
            for (int i = 0; i < spawnCount; i++)
            {
                bool dummy = Random.value <= dummyProbability;
                Quaternion rotation = RandomRot();

                int tries = 0;

                Vector3 position;
                Collider2D collision;
                do
                {
                    position = RandomPosition(size);
                    collision = Physics2D.OverlapCircle(position, 0.5f,
                        LayerMask.GetMask("Default"));
                    tries++;
                } while (collision && tries < 5);
                if(tries == 5) continue;
                GameObject spawned;
                if (dummy)
                {
                    spawned = Spawn(DummyModel, position, rotation);
                }
                else
                {
                    spawned = Spawn(MikuniModel, position, rotation);
                    spawned.GetComponent<Mikuni>().SetStateSilently(Mikuni.STATE_IDLE);
                }
                SpanwedMikunis.Add(spawned);
            }
        }

        private Vector3 RandomPosition(float size)
        {
            Vector3 position = transform.position;
            position.x += Random.Range(-size, size);
            position.z += Random.Range(-size, size);
            if (stickToTerrain)
            {
                if (Physics.Raycast(position, Vector3.down, out var hit, 10, 
                    LayerMask.GetMask("Ground")))
                {
                    position.y = hit.point.y;
                }
            }
            return position;
        }

        private Quaternion RandomRot()
        {
            return Quaternion.Euler(
                rotationMask.x * Random.Range(0.0f, 360.0f),
                rotationMask.y * Random.Range(0.0f, 360.0f),
                rotationMask.z * Random.Range(0.0f, 360.0f));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, new Vector3(width, 1, width));
        }
    }
}