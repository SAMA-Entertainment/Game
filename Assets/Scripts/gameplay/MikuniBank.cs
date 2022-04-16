using System;
using System.Collections.Generic;
using mikunis;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace gameplay
{
    public class MikuniBank : MonoBehaviour
    {
        private void Start()
        {
        }

        private void Update()
        {
        }

        public bool Put(Player player, List<Mikuni> mikunis)
        {
            foreach (Mikuni mikuni in mikunis)
            {
                mikuni.gameObject.SetActive(true);
                Transform tr = mikuni.transform;
                tr.parent = transform.parent.transform;
                tr.localPosition = Vector3.up * 5 + Random.Range(-1f, 1f) * Vector3.right 
                                                  + Random.Range(-1f, 1f) * Vector3.forward;
                Rigidbody rb = mikuni.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.detectCollisions = true;
            }

            return true;
        }
    }
}