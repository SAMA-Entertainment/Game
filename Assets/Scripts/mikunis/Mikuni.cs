using System;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

namespace mikunis
{
    public class Mikuni : MonoBehaviour
    {
    
        public static readonly int STATE_IDLE = 0;
        public static readonly int STATE_FLEEING = 1;
        public static readonly int STATE_CAPTURED = 2;
    
        private int _state;
        private float cooldown;
        public int State => _state;
    
        [NotNull]
        public NavMeshAgent agent;
        public ParticleSystem spottedParticle;
        public GameObject eyes;

        public Texture normalEyes;
        public Texture scaredEyes;
        public Texture capturedEyes;

        private PhotonView _view;

        private void Start()
        {
            _view = GetComponent<PhotonView>();
        }

        protected virtual void FixedUpdate()
        {
            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
                return;
            }
            if (_view.IsMine && _state == STATE_FLEEING && agent.velocity.sqrMagnitude <= 0.05)
            {
                SetState(STATE_IDLE);
                if (eyes != null)
                {
                    Material material = eyes.GetComponent<Renderer>().material;
                    material.SetTexture("_EyesTexture", normalEyes);
                    material.SetFloat("_Shaking", 0);
                }
            }
        }

        private void OnDestroy()
        {
            _state = STATE_CAPTURED;
        }

        /**
         * This function is called when this Mikuni should switch its current state to STATE_FLEEING and
         * start fleeing from the target
         */
        public void PlayerNear(Transform player)
        {
            if (!_view.IsMine || _state == STATE_CAPTURED) return;
            if (_state != STATE_FLEEING && spottedParticle != null){
                spottedParticle.Play();
                cooldown = 2;
                if (eyes != null)
                {
                    Material material = eyes.GetComponent<Renderer>().material;
                    material.SetTexture("_EyesTexture", capturedEyes);
                    material.SetFloat("_Shaking", 1);
                }
            }
            SetState(STATE_FLEEING);

            Vector3 dir = transform.position - player.transform.position;
            Vector3 newPos = transform.position + dir;
 
            agent.SetDestination(newPos);
        }

        public void SetCaptured(bool captured)
        {
            SetState(captured ? STATE_CAPTURED : STATE_IDLE);
            SetCapturedSilently(captured);
        }

        private void SetCapturedSilently(bool captured)
        {
            agent.enabled = !captured;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = captured;
            rb.detectCollisions = !captured;
            if (eyes == null) return;
            Material material = eyes.GetComponent<Renderer>().material;
            material.SetTexture("_EyesTexture", scaredEyes);
            material.SetFloat("_Shaking", 1);
        }

        protected void SetState(int state)
        {
            _state = state;
            _view.RPC("RPC_SyncState", RpcTarget.OthersBuffered, state);
        }

        [PunRPC]
        protected void RPC_SyncState(int mikuniState)
        {
            Debug.Log("RPC_SyncState => " + mikuniState);
            _state = mikuniState;
            SetCapturedSilently(mikuniState == STATE_CAPTURED);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _state == STATE_IDLE ? Color.magenta : (_state == STATE_FLEEING ? Color.cyan : Color.yellow);
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }
}
