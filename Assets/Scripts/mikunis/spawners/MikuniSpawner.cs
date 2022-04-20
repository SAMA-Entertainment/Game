using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;

namespace mikunis.spawners
{
    public abstract class MikuniSpawner : MonoBehaviour
    {
        protected void Init(List<GameObject> prefabs)
        {
            DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
            if (pool != null && prefabs != null)
            {
                foreach (GameObject prefab in prefabs)
                {
                    if(pool.ResourceCache.ContainsKey(prefab.name)) continue;
                    pool.ResourceCache.Add(prefab.name, prefab);
                }
            }

            if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient)
            {
                this.enabled = false;
            }
        }

        protected List<GameObject> ToGameObjects<T>(List<T> prefabs) where T : MonoBehaviour
        {
            List<GameObject> gameObjects = new List<GameObject>();
            foreach (T prefab in prefabs)
            {
                gameObjects.Add(prefab.gameObject);
            }
            return gameObjects;
        }

        protected GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom)
            {
                return PhotonNetwork.Instantiate(prefab.name, position, rotation);
            }
            return Instantiate(prefab, position, rotation);
        }
    }
}