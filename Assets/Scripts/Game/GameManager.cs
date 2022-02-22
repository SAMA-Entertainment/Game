using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    void Start()
    {
        // TODO :
        // Vérifier que le joueur est connecté
        // Vérifier que playerPrefab != null
        PhotonNetwork.Instantiate
        (this.playerPrefab.name, 
        new Vector3((float)-2.24, (float)-0.04, (float)-32.95),
        Quaternion.identity, 0);
    }
}
