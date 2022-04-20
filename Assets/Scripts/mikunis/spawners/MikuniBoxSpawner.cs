using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace mikunis.spawners
{
    public class MikuniBoxSpawner : MonoBehaviour
    {

        public int capacity = 30;
        public int width = 100;
        public List<Mikuni> mikuniInstances;
        public bool burstMode = true;
        public float spawnTimer = 5f;
        public int batchCount = 1;

        private bool _spawning;
        private float _spawnTimerState;

        public bool IsSpawning => _spawning;

        void Start()
        {
            DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
            if (pool != null && mikuniInstances != null)
            {
                foreach (Mikuni prefab in mikuniInstances)
                {
                    pool.ResourceCache.Add(prefab.name, prefab.gameObject);
                }
            }    
            if (!PhotonNetwork.IsMasterClient)
            {
                this.enabled = false;
                return;
            }
            _spawnTimerState = spawnTimer;
            _spawning = false;
            if (burstMode)
            {
                StartCoroutine(BurstSpawnMikunis());
            }
        }

        void Update()
        {
            if (burstMode || _spawning) return;
            _spawnTimerState -= Time.deltaTime;
            if (_spawnTimerState <= 0)
            {
                StartCoroutine(BurstSpawnMikunis());
                _spawnTimerState = spawnTimer;
            }
        }

        /**
         * Coroutine to spawn in burst `batchCount` mikunis
         * <example>StartCoroutine(BurstSpawnMikunis());</example>
         */
        public IEnumerator BurstSpawnMikunis()
        {
            if(_spawning) yield break;
            _spawning = true;
            Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, 
                new Vector3(width, 2, width), 
                Quaternion.identity, LayerMask.GetMask("Default"));
            // TODO: Replace this with an appropriate layer
            int nearbyMikunis = 0;
            foreach (Collider obj in hitColliders)
            {
                if (obj.gameObject.GetComponent<Mikuni>() != null)
                {
                    nearbyMikunis++;
                }
            }

            if (nearbyMikunis > capacity)
            {
                _spawning = false;
                Debug.LogWarning("Failed to spawn mikunis: Too much mikunis nearby (" + nearbyMikunis + ")");
                yield break;
            }

            int size = width/2;
            for (int i = 0; i < batchCount; i++)
            {
                int idx = Random.Range(0, mikuniInstances.Count);
                Vector3 position = transform.position;
                position.x += Random.Range(-size, size+1);
                position.z += Random.Range(-size, size+1);
                yield return new WaitForSeconds(0.1f);
                PhotonNetwork.Instantiate(mikuniInstances[idx].name, position,
                    quaternion.identity);
            }

            _spawning = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(width, 2, width));
        }
    }
}
