using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        
    }

    public void OnPlayerEnterRoom(Player other)
    {
        print(other.NickName + " a rejoint la partie");
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        print(other.NickName + " a quitté la partie");
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
