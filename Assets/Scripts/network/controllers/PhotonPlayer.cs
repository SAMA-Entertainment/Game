using System.IO;
using Photon.Pun;
using player;
using UnityEngine;

namespace network.controllers
{
    public class PhotonPlayer : MonoBehaviour
    {
        public PhotonView _view;
        private GameObject _avatar;
        public int teamId = -1;
    
        void Start()
        {
            _view = GetComponent<PhotonView>();
            if (_view.IsMine)
            {
                _view.RPC("RPC_GetTeam", RpcTarget.MasterClient);
            }
        }

        void Update()
        {
            if (!_view.IsMine || _avatar != null || teamId == -1) return;
            Transform[] spawnPoints =
                teamId == 0 ? GameManager.Instance.blueSpawnPoints : GameManager.Instance.redSpawnPoints;
            int spawnPointIdx = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnPointIdx];
            _avatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
                spawnPoint.position, spawnPoint.rotation, 0);
            _avatar.GetComponent<PlayerController>().Player = this;
        }

        [PunRPC]
        void RPC_GetTeam()
        {
            teamId = GameManager.Instance.nextPlayersTeam;
            GameManager.Instance.UpdateTeam();
            _view.RPC("RPC_UpdateTeam", RpcTarget.OthersBuffered, teamId);
        }

        [PunRPC]
        void RPC_UpdateTeam(int which)
        {
            teamId = which;
        }
        
        [PunRPC]
        void RPC_PushScore(int team, int score)
        {
            if (score < 0) return;
            GameManager.Instance.scores[team] += (uint)score;
            _view.RPC("RPC_UpdateScore", RpcTarget.OthersBuffered, 
                team, (int)GameManager.Instance.scores[team]);
        }

        [PunRPC]
        void RPC_UpdateScore(int team, int newScore)
        {
            if (newScore < 0) return;
            GameManager.Instance.scores[team] = (uint)newScore;
        }
    }
}
