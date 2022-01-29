using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class RollingMikuniController : MonoBehaviour
    {

        private readonly static int STATE_IDLE = 0;
        private readonly static int STATE_FLEEING = 1;

        private int _state;
        
        public Transform rotatingBody;
        public NavMeshAgent agent;
        public float rotSpeedX = 250f;
        public ParticleSystem spottedParticle;

        private void FixedUpdate()
        {
            float speed = agent.velocity.magnitude/agent.speed;
            if (speed == 0 && _state == STATE_FLEEING)
            {
                _state = STATE_IDLE;
            }
            else
            {
                rotatingBody.Rotate(Vector3.right, rotSpeedX * speed * Time.deltaTime);
            }
        }

        public void PlayerNear(Transform player)
        {
            if(_state != STATE_FLEEING){
                spottedParticle.Play();
            }
            _state = STATE_FLEEING;

            Vector3 dir = transform.position - player.transform.position;
            Vector3 newPos = transform.position + dir;
 
            agent.SetDestination(newPos);
        }
    }
}
