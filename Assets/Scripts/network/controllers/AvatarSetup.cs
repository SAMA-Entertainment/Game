using Photon.Pun;
using player;
using UnityEngine;
using ustensils;

namespace network.controllers
{
    public class AvatarSetup : MonoBehaviour
    {
        private PhotonView _view;
        public int playerSkin;
        public int playerUstencil;
        [HideInInspector] 
        public GameObject avatar;
        [HideInInspector] 
        public GameObject ustencil;

        private void Start()
        {
            _view = GetComponent<PhotonView>();
            if (_view.IsMine)
            {
                _view.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, 
                    PlayerInfo.PInfo.selectedSkin, PlayerInfo.PInfo.selectedUstencil);
            }
        }

        [PunRPC]
        void RPC_AddCharacter(int characterIndex, int ustencilIndex)
        {
            playerSkin = characterIndex;
            playerUstencil = ustencilIndex;
            Transform tr = transform;
            avatar = Instantiate(PlayerInfo.PInfo.allCharacters[characterIndex], tr.position, tr.rotation, tr);
            ustencil = Instantiate(PlayerInfo.PInfo.allUstencils[characterIndex], tr.position, tr.rotation, tr);
            PlayerController mvt = avatar.GetComponentInParent<PlayerController>();
            mvt.cam = Camera.main.transform;
            mvt.animator = avatar.GetComponentInChildren<Animator>();
            mvt._ustencil = ustencil.GetComponent<Utensil>();
            mvt.SetupUstencil();
        }
    }
}