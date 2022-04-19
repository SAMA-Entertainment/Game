using System;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace network
{
    public class PhotonRoom : MonoBehaviourPunCallbacks
    {
        public static PhotonRoom CurrentRoom;

        public int lobbySceneId = 1;
        public int multiplayerSceneId = 2;
        public int CurrentScene => _currentScene;

        private PhotonView _view;
        private bool _isGameLoaded;
        private int _currentScene;

        private Player[] _players;

        private void Awake()
        {
            if (CurrentRoom != null && CurrentRoom != this)
            {
                Destroy(CurrentRoom.gameObject);
            }
            CurrentRoom = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            _view = GetComponent<PhotonView>();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
            SceneManager.sceneLoaded += OnSceneFinishedLoading;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.RemoveCallbackTarget(this);
            SceneManager.sceneLoaded -= OnSceneFinishedLoading;
        }

        private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            _currentScene = scene.buildIndex;
            if (_currentScene == multiplayerSceneId)
            {
                CreatePlayer();
            }
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log("Succesfully connected to Room<" + PhotonNetwork.CurrentRoom.Name + ">");
            _players = PhotonNetwork.PlayerList;
            PhotonNetwork.NickName = "Player" + _players.Length;
            if(PlayerInfo.PInfo != null) PlayerInfo.PInfo.SetSelectedSkin(0); // TODO: Change me
            LoadScene(lobbySceneId);
        }

        public void StartGame()
        {
            LoadScene(multiplayerSceneId);
        }

        private void LoadScene(int scene)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            PhotonNetwork.LoadLevel(scene);
        }
        
        private void CreatePlayer()
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position,
                Quaternion.identity, 0);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log(otherPlayer.NickName + " left the game");
        }
    }
}