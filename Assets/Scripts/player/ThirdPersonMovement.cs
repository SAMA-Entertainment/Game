using Cinemachine;
using menus;
using mikunis;
using Photon.Pun;
using tools;
using UnityEngine;

namespace player
{
    public class ThirdPersonMovement : MonoBehaviour
    {
        public CharacterController controller;
        public Transform cam;
        public Animator animator;
        public float speed = 6f;
        public float maxstamina = 10;
        public bool isRunning;
        public float jumpHeight = 1f;
        public float jumpMultiplier = 2f;
        public float turnSmoothTime = 0.1f;
        private float _turnSmoothVelocity;

        private PhotonView _view;
        public float Stamina => _stamina;
        private bool jumping;
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
            if (controller.isGrounded)
            {
                jumping = Input.GetButton("Jump") && !jumping;
            }

            if (transform.position.y > jumpHeight)
            {
                jumping = false;
            } 
            else if(jumping)
            {
                moveVector = jumpHeight * jumpMultiplier * Vector3.up - Physics.gravity;
            }
        
            if (!controller.isGrounded)
                moveVector += Physics.gravity;

            if (dir.magnitude >= 0.1f) // enough movement
            {
                float speed = this.speed;
                isRunning = Input.GetKey(KeyCode.LeftShift);
                if (isRunning)
                {
                    _stamina -= Time.deltaTime;
                    speed *= 1.5f;
                    if (_stamina < 0)
                    {
                        _stamina = 0;
                        isRunning = false;
                    }
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
                controller.Move(moveVector * Time.deltaTime);
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
    }
}
