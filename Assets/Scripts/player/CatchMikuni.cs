using System.Collections.Generic;
using menus;
using mikunis;
using Photon.Pun;
using UnityEngine;

namespace player
{
    public class CatchMikuni : MonoBehaviour
    {
        private PhotonView _view;
        private bool _isCatching;
        private List<Mikuni> _caughtMikunis;
    
        public int MikuniCatched => _caughtMikunis.Count;

        void Start()
        {
            _isCatching = false;
            _caughtMikunis = new List<Mikuni>();
            _view = GetComponentInParent<PhotonView>();
            this.enabled = _view.IsMine;
            if (_view.IsMine)
            {
                PlayerHUD.HUD.mikuniCatchController = this;
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Mikuni")) return;
            if (Input.GetKeyDown(KeyCode.P) && !_isCatching)
            {
                Mikuni target = other.gameObject.GetComponent<Mikuni>();
                _caughtMikunis.Add(target);
                //Destroy(other.gameObject);
                PhotonNetwork.Destroy(other.gameObject);
                _isCatching = true;
            }
            else
            {
                _isCatching = false;
            }
        
        }
    }
}
