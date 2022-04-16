using System;
using System.Collections.Generic;
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

        void Start()
        {
            _isCatching = false;
            _caughtMikunis = new List<Mikuni>();
            _view = transform.parent.GetComponent<PhotonView>();
            this.enabled = _view.IsMine;
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
                ReleaseFirst();
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Mikuni")) return;
            if (Input.GetKeyDown(KeyCode.P) && !_isCatching && MikuniCatched < Capacity)
            {
                Mikuni target = other.gameObject.GetComponent<Mikuni>();
                _caughtMikunis.Add(target);
                _viewer.DisplayMikuni(target);
                _isCatching = true;
            }
            else
            {
                _isCatching = false;
            }
        
        }

        public void ReleaseAll()
        {
            _viewer.Clear(null);
            _caughtMikunis.Clear();
        }

        public void ReleaseFirst()
        {
            Mikuni mikuni = _caughtMikunis[0];
            _viewer.HideMikuni(mikuni);
            _caughtMikunis.Remove(mikuni);
        }

        public void DestroyAll()
        {
            foreach (Mikuni mikuni in _caughtMikunis)
            {
                PhotonNetwork.Destroy(mikuni.gameObject);
            }
        }
    }
}
