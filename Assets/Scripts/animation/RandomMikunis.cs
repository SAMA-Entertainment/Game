using System;
using System.Collections.Generic;
using mikunis;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace animation
{
    public class RandomMikunis : MonoBehaviour
    {
        public GameObject[] models;
        public int trimThreshold = 30;
        public int trimCount = 5;
        public int spawnDelay = 300;
        public int spawnInterval = 300;
        public int spawnRadius = 5;
        public int spawnHeight = 15;
        
        private readonly List<GameObject> _spawnedMikunis = new List<GameObject>();
        private int _timer;
    
        // Start is called before the first frame update
        private void Start()
        {
            _timer = spawnDelay;
        }

        private void Update()
        {
            if (_timer > 0)
            {
                _timer--;
                return;
            }
            if (_spawnedMikunis.Count > trimThreshold)
            {
                for (int i = 0; i < trimCount; i++)
                {
                    Destroy(_spawnedMikunis[i]);
                }
                _spawnedMikunis.RemoveRange(0, trimCount);
            }
            Vector2 pos = Random.insideUnitCircle * spawnRadius;
            Vector3 offset = new Vector3(pos.x, spawnHeight, pos.y);
            int idx = Random.Range(0, models.Length);
            GameObject obj = Instantiate(models[idx], transform.position + offset, Random.rotation);
            obj.GetComponent<Mikuni>().enabled = false;
            obj.GetComponent<NavMeshAgent>().enabled = false;
            _spawnedMikunis.Add(obj);
            _timer = spawnInterval;
        }
    }
}
