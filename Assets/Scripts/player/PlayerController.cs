using menus;
using mikunis;
using network.controllers;
using Photon.Pun;
using tools;
using UnityEngine;
using ustensils;

namespace player
{
    public class PlayerController : MonoBehaviour
    {
        [HideInInspector]
        public PhotonPlayer Player;
        
        public CharacterController controller;
        public Transform cam;
        public Animator animator;
        public float speed = 6f;
        public float maxstamina = 10;
        public float turnSmoothTime = 0.1f;

        public Utensil _ustencil;
        public bool isRunning;
        private float _turnSmoothVelocity;
        private int previousSpeed;
        private PhotonView _view;
        private UtensilHolder _utensilHolder;
        public float Stamina => _stamina;
        private float _stamina = 10;
    
        private void Start()
        {
            _view = GetComponent<PhotonView>();
            if (_view != null)
            {
                TransformHelper.FindComponentInChildWithTag(this.gameObject, "MainCamera").SetActive(_view.IsMine); 
                if(_view.IsMine) PlayerHUD.HUD.movement = this;
            }
        }

        public void SetupUstencil()
        {
            _utensilHolder = GetComponentInChildren<UtensilHolder>();
            Transform tr = _ustencil.transform;
            tr.parent = _utensilHolder.transform;
            tr.localPosition = Vector3.zero;
            tr.rotation = Quaternion.identity;
        }

        void Update()
        {
            if (_view != null && !_view.IsMine) return;
            if (cam == null) return;
            // Get Input From the player
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized; 
            // Normalize the vector so the speed is constant
            // Here you can modify the speed (sprint)
            // Add gravity force
            Vector3 moveVector = Vector3.zero;
        
            if (!controller.isGrounded)
                moveVector += Physics.gravity;

            isRunning = Input.GetKey(KeyCode.LeftShift);
            if (dir.magnitude >= 0.1f) // enough movement
            {
                float speed = this.speed;
                if (isRunning && _stamina > 0.15)
                {
                    _stamina -= Time.deltaTime;
                    speed *= 1.8f;
                    if (_stamina < 0)
                    {
                        _stamina = 0;
                        isRunning = false;
                    }
                }

                if (speed >= 10 && previousSpeed != 10)
                {
                    _view.RPC("RPC_SyncCharacterAnim", RpcTarget.Others, 10);
                    previousSpeed = 10;
                } else if (speed >= 6 && previousSpeed != 6)
                {
                    _view.RPC("RPC_SyncCharacterAnim", RpcTarget.Others, 6);
                    previousSpeed = 6;
                }
            
                float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 movDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                animator.SetFloat("Speed", speed);
                controller.Move((moveVector + movDir.normalized * speed) * Time.deltaTime);
            }
            else
            {
                animator.SetFloat("Speed", 0f);
                if (previousSpeed != 0f)
                {
                    _view.RPC("RPC_SyncCharacterAnim", RpcTarget.Others, 0);
                }
                controller.Move(moveVector * Time.deltaTime);
                previousSpeed = 0;
            }
        
            if (!isRunning && _stamina < maxstamina)
            {
                _stamina += Time.deltaTime;
            }
        }

        /**
         * This function is used to trigger the "Fleeing" state of nearby mikunis. Nearby mikunis
         * are all mikunis within the collider on the current GameObject.
         */
        void OnTriggerStay(Collider other)
        {
            Mikuni mikuniController =
                other.gameObject.GetComponent<Mikuni>();
            if (mikuniController != null)
            {
                // Debug.Log("Mikuni detected");
                mikuniController.PlayerNear(transform);
            }
        }
        
        [PunRPC]
        void RPC_SyncCharacterAnim(int speed)
        {
            animator.SetFloat("Speed", speed);
        }

        [PunRPC]
        void RPC_CaptureMikuni(int viewId)
        {
            GameObject target = PhotonNetwork.GetPhotonView(viewId).gameObject;
            Mikuni capturedMikuni = target.GetComponent<Mikuni>();
            MikuniViewer viewer = GetComponentInChildren<MikuniViewer>();
            if (capturedMikuni == null || viewer == null)
            {
                Debug.LogWarning("Received RPC_CaptureMikuni but " + (capturedMikuni == null
                    ? " the target view is not a Mikuni" : "could not find MikuniViewer"));
                return;
            }
            viewer.DisplayMikuni(target.GetComponent<Mikuni>(), true);
        }
        
        [PunRPC]
        void RPC_ReleaseMikuni(int viewId)
        {
            GameObject target = PhotonNetwork.GetPhotonView(viewId).gameObject;
            Mikuni capturedMikuni = target.GetComponent<Mikuni>();
            MikuniViewer viewer = GetComponentInChildren<MikuniViewer>();
            if (capturedMikuni == null || viewer == null)
            {
                Debug.LogWarning("Received RPC_ReleaseMikuni but " + (capturedMikuni == null 
                    ? " the target view is not a Mikuni" : "could not find MikuniViewer"));
                return;
            }
            viewer.ReleaseMikuni(capturedMikuni.GetComponent<Mikuni>(), true);
        }
    }
}
