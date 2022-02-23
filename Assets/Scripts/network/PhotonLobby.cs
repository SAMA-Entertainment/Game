using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace network
{
    public class PhotonLobby : MonoBehaviourPunCallbacks
    {
        public static PhotonLobby Lobby { get; private set; }
        public GameObject joinButton;

        private Button _joinButtonHandle;
        private System.Random _rd = new System.Random();
        private int _createRoomAttempts;
            
        private void Awake()
        {
            Lobby = this;
            _createRoomAttempts = 3;
        }

        void Start()
        {
            _joinButtonHandle = joinButton.GetComponent<Button>();
            _joinButtonHandle.interactable = false;
            PhotonNetwork.ConnectUsingSettings(); // Connects to the PUN master server
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Successfully connected to master");
            PhotonNetwork.AutomaticallySyncScene = true;
            _joinButtonHandle.interactable = true;
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to create room (" + returnCode + ": " + message + ")");
            if (_createRoomAttempts > 0)
            {
                _createRoomAttempts--;
                CreateRoom();
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("No random room found!");
            CreateRoom();
        }

        void CreateRoom()
        {
            byte[] roomId = new byte[8];
            _rd.NextBytes(roomId);
            string roomName = "mk:room:" + BitConverter.ToString(roomId).Replace("-", "").ToLower();
            RoomOptions opts = new RoomOptions() {IsVisible = true, IsOpen = true, MaxPlayers = 4};
            PhotonNetwork.CreateRoom(roomName, opts);
            Debug.Log("Room<" + roomName + "> created with settings={IsVisible = true, IsOpen = true, MaxPlayers = 4}");
        }

        public void OnJoinButtonClicked()
        {
            Debug.Log("Joining random room...");
            JoinRandomRoom();
        }

        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        void Update()
        {
            
        }
    }
}