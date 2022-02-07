using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace mikunis
{
    public abstract class Mikuni : MonoBehaviour
    {
    
        public readonly static int STATE_IDLE = 0;
        public readonly static int STATE_FLEEING = 1;
    
        private int _state;
        public int State => _state;
    
        [NotNull]
        public NavMeshAgent agent;
        public ParticleSystem spottedParticle;
    
        protected virtual void FixedUpdate()
        {
            if (_state == STATE_FLEEING && agent.velocity.sqrMagnitude <= 0.05)
            {
                _state = STATE_IDLE;
            }
        }
    
        /**
         * This function is called when this Mikuni should switch its current state to STATE_FLEEING and
         * start fleeing from the target
         */
        public void PlayerNear(Transform player)
        {
            if(_state != STATE_FLEEING && spottedParticle != null){
                spottedParticle.Play();
            }
            _state = STATE_FLEEING;

            Vector3 dir = transform.position - player.transform.position;
            Vector3 newPos = transform.position + dir;
 
            agent.SetDestination(newPos);
        }

    }
}
