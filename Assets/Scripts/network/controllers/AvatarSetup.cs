using Photon.Pun;
using player;
using UnityEngine;

namespace network.controllers
{
    public class AvatarSetup : MonoBehaviour
    {
        private PhotonView _view;
        public int playerSkin;
        public GameObject avatar;

        private void Start()
        {
            _view = GetComponent<PhotonView>();
            if (_view.IsMine)
            {
                _view.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.PInfo.selectedSkin);
            }
        }

        [PunRPC]
        void RPC_AddCharacter(int whichCharacter)
        {
            playerSkin = whichCharacter;
            Transform tr = transform;
            avatar = Instantiate(PlayerInfo.PInfo.allCharacters[whichCharacter], tr.position, tr.rotation, tr);
            ThirdPersonMovement mvt = avatar.GetComponentInParent<ThirdPersonMovement>();
            mvt.cam = Camera.main.transform;
            mvt.animator = avatar.GetComponentInChildren<Animator>();
        }
    }
}