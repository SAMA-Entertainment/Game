using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace mikunis
{
    public abstract class Mikuni : MonoBehaviour
    {
    
        public readonly static int STATE_IDLE = 0;
        public readonly static int STATE_FLEEING = 1;
        public readonly static int STATE_CAPTURED = 2;
    
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
    
        protected virtual void FixedUpdate()
        {
            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
                return;
            }
            if (_state == STATE_FLEEING && agent.velocity.sqrMagnitude <= 0.05)
            {
                _state = STATE_IDLE;
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
            if (_state == STATE_CAPTURED) return;
            if(_state != STATE_FLEEING && spottedParticle != null){
                spottedParticle.Play();
                cooldown = 2;
                if (eyes != null)
                {
                    Material material = eyes.GetComponent<Renderer>().material;
                    material.SetTexture("_EyesTexture", capturedEyes);
                    material.SetFloat("_Shaking", 1);
                }
            }
            _state = STATE_FLEEING;

            Vector3 dir = transform.position - player.transform.position;
            Vector3 newPos = transform.position + dir;
 
            agent.SetDestination(newPos);
        }

        public void SetCaptured(bool captured)
        {
            _state = captured ? STATE_CAPTURED : STATE_IDLE;
            agent.enabled = !captured;
            if (eyes == null) return;
            Material material = eyes.GetComponent<Renderer>().material;
            material.SetTexture("_EyesTexture", scaredEyes);
            material.SetFloat("_Shaking", 1);
        }

    }
}
