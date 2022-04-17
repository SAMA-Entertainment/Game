using System;
using System.Collections.Generic;
using gameplay;
using menus;
using mikunis;
using Photon.Pun;
using UnityEngine;

namespace player
{
    public class MikuniBucket : MonoBehaviour
    {
        private PhotonView _view;
        private bool _isCatching;
        private List<Mikuni> _caughtMikunis;
        private MikuniViewer _viewer;

        public readonly int Capacity = 5;
    
        public int MikuniCatched => _caughtMikunis.Count;
        private GameObject _cauldron;

        void Start()
        {
            _isCatching = false;
            _caughtMikunis = new List<Mikuni>();
            _view = transform.parent.GetComponent<PhotonView>();
            this.gameObject.SetActive(_view.IsMine);
            if (_view.IsMine)
            {
                PlayerHUD.HUD.mikuniBucketController = this;
            }
            
            _viewer = transform.parent.GetComponentInChildren<MikuniViewer>();
            if (_viewer == null) throw new Exception("Missing MikuniViewer script in hierarchy");
        }
        
        private void Update()
        {
            if (_view.IsMine && Input.GetKeyDown(KeyCode.R) && MikuniCatched > 0)
            {
                if (_cauldron != null)
                {
                    PlayerController controller = GetComponentInParent<PlayerController>();
                    bool success = _cauldron.GetComponent<MikuniBank>().Put(controller.Player, _caughtMikunis);
                    if (success)
                    {
                        _viewer.Destroy();
                        _caughtMikunis.Clear();
                        return;
                    }
                }
                ReleaseOne();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            GameObject go = other.gameObject;
            if (go.CompareTag("Cauldron"))
            {
                _cauldron = go;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Cauldron"))
            {
                _cauldron = null;
            }
        }

        void OnTriggerStay(Collider other)
        {
            GameObject obj = other.gameObject;
            if (!obj.CompareTag("Mikuni")) return;
            if (Input.GetMouseButtonDown(0) && !_isCatching && MikuniCatched < Capacity)
            {
                Mikuni target = obj.GetComponent<Mikuni>();
                if (obj.activeSelf && target.State != Mikuni.STATE_CAPTURED)
                {
                    _caughtMikunis.Add(target);
                    _viewer.DisplayMikuni(target, false);
                    _view.RPC("RPC_CaptureMikuni", RpcTarget.OthersBuffered, target._view.ViewID);
                    _isCatching = true;
                }
            }
            else
            {
                _isCatching = false;
            }
        
        }

        /**
         * Release all caught Mikunis in the world
         */
        public void ReleaseAll()
        {
            _viewer.Clear(null);
            _caughtMikunis.Clear();
        }

        /**
         * Release the first caught Mikuni in the world
         */
        public void ReleaseOne()
        {
            Mikuni mikuni = _caughtMikunis[0];
            _view.RPC("RPC_ReleaseMikuni", RpcTarget.OthersBuffered, mikuni._view.ViewID);
            _viewer.ReleaseMikuni(mikuni, false);
            _caughtMikunis.Remove(mikuni);
        }

        /**
         * Destroys all caught Mikunis
         */
        public void DestroyAll()
        {
            foreach (Mikuni mikuni in _caughtMikunis)
            {
                PhotonNetwork.Destroy(mikuni.gameObject);
            }
            _viewer.Destroy();
            _caughtMikunis.Clear();
        }
    }
}
