using System;
using System.Collections.Generic;
using network;
using network.controllers;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace menus
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        public int fallbackScene = 0;
        private PhotonView _view;
        public GameObject startGame;
        public Color readyColor;
        private readonly List<(Player, bool)> _readiness = new List<(Player, bool)>();
        public GameObject[] PlayerNametags;

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.RemoveCallbackTarget(this);
        }
        
        private void Start()
        {
            _view = GetComponent<PhotonView>();
            startGame.SetActive(false);
            foreach (var pair in PhotonNetwork.CurrentRoom.Players)
            {
                _readiness.Add((pair.Value, false));
            }
            Render();
        }

        public void StartGame()
        {
            PhotonRoom.CurrentRoom.StartGame();
        }
        
        public void Ready()
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            int i = FindPlayer(localPlayer.ActorNumber);
            if (i == -1) return;
            _view.RPC("RPC_UpdateReadyState", RpcTarget.AllBuffered,
                localPlayer.ActorNumber, !_readiness[i].Item2);
        }

        public void Leave()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LeaveRoom(false);
            SceneManager.LoadScene(fallbackScene);
        }

        [PunRPC]
        public void RPC_UpdateReadyState(int actor, bool ready)
        {
            int i = FindPlayer(actor);
            if (i != -1)
            {
                var pl = _readiness[i].Item1;
                _readiness[i] = (pl, ready);
            }

            Render();

            if (PhotonNetwork.IsMasterClient)
            {
                bool isEveryoneReady = true;
                foreach (var pair in _readiness)
                {
                    isEveryoneReady = isEveryoneReady && pair.Item2;
                }

                startGame.SetActive(isEveryoneReady);
            }
        }

        public void Render()
        {
            int currentActor = PhotonNetwork.LocalPlayer.ActorNumber;
            int i = 0;
            foreach (var pair in _readiness)
            {
                GameObject nametag = PlayerNametags[i];
                Image background = nametag.GetComponent<Image>();
                background.enabled = true;
                background.color = pair.Item2 ? readyColor : Color.white;
                nametag.GetComponentInChildren<TextMeshProUGUI>().text = 
                    (pair.Item1.ActorNumber == currentActor ? "(You)" : "") +pair.Item1.NickName;
                i++;
            }
        }

        private int FindPlayer(int actor)
        {
            int l = _readiness.Count, i = 0;
            while (i < l && _readiness[i].Item1.ActorNumber != actor) i++;

            if (i == l) return -1;
            return i;
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            int i = FindPlayer(otherPlayer.ActorNumber);
            if (i == -1) return;
            _readiness.RemoveAt(i);
            Render();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            _readiness.Add((newPlayer, false));
            Render();
        }
    }
}