using System.IO;
using Photon.Pun;
using UnityEngine;

namespace network.controllers
{
    public class PhotonPlayer : MonoBehaviour
    {
        private PhotonView _view;
        private GameObject _avatar;
    
        void Start()
        {
            _view = GetComponent<PhotonView>();
            if (_view.IsMine)
            {
                int spawnPointIdx = Random.Range(0, GameSetup.Instance.spawnPoints.Length);
                Transform spawnPoint = GameSetup.Instance.spawnPoints[spawnPointIdx];
                _avatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), 
                    spawnPoint.position, spawnPoint.rotation, 0);
            }
        }

        void Update()
        {
        
        }
    }
}
