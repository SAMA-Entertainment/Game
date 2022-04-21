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

        public PhotonView _view;

        protected void Start()
        {
            _view = GetComponent<PhotonView>();
            SetStateSilently(STATE_IDLE);
        }

        protected virtual void FixedUpdate()
        {
            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
                OnAnimation();
                return;
            }
            if (_view.IsMine && _state == STATE_FLEEING && agent.velocity.sqrMagnitude <= 0.05)
            {
                SetState(STATE_IDLE);
            }
            OnAnimation();
        }

        protected virtual void OnAnimation()
        {
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
        }

        protected void SetState(int state)
        {
            SetStateSilently(state);
            _view.RPC("RPC_SyncState", RpcTarget.OthersBuffered, state);
        }

        [PunRPC]
        protected void RPC_SyncState(int mikuniState)
        {
            SetStateSilently(mikuniState);
        }

        public virtual void SetStateSilently(int state)
        {
            _state = state;
            GetComponent<Rigidbody>().constraints =
                _state == STATE_IDLE ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;
            agent.enabled = state == STATE_FLEEING;
            SetCapturedSilently(state == STATE_CAPTURED);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _state == STATE_IDLE ? Color.magenta : (_state == STATE_FLEEING ? Color.cyan : Color.yellow);
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }
}
