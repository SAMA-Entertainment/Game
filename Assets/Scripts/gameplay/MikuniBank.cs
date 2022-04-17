using System;
using System.Collections.Generic;
using mikunis;
using network.controllers;
using Photon.Pun;
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

        public bool Put(PhotonPlayer player, List<Mikuni> mikunis)
        {
            if (mikunis.Count > 5) return false;
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
            player._view.RPC("RPC_PushScore", RpcTarget.MasterClient, 
                player.teamId, mikunis.Count);

            return true;
        }
    }
}