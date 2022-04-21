using System;
using System.Collections.Generic;
using UnityEngine;

namespace mikunis
{
    public class HidingMikuni : Mikuni
    {
        public float bodyStartOffset;
        public Transform body;
        public Animator animator;

        public bool jumpingAnimation = true;
        
        // List of gameObject to hide when the Mikuni is hiding from the player
        public List<GameObject> bodyParts = new List<GameObject>();

        /* Jumping properties */
        private const double MaxPhase = 2 * Math.PI;
        private float jumpFrequency = 0.5f;
        private float jumpHeight = 0.6f;
        private float jumpUpdateSpeed = 0.15f; // in rad/s
        private float jumpLeaningEffect = 1.5f;
        private float jumpMaxLeaningAngle = 15; // in degrees
        
        private Vector3 _basePosition;
        private Quaternion _baseRotation;
        private bool _hiding;
        private double _jumpPhase; // in rad

        private new void Start()
        {
            base.Start();
            _basePosition = body.localPosition;
            _baseRotation = body.localRotation;
        }

        protected override void OnAnimation()
        {
            if (!jumpingAnimation || State != STATE_FLEEING) return;

            double attenuation = agent.velocity.sqrMagnitude / Math.Pow(agent.speed, 2);
            double vOffset = attenuation * Math.Sin(jumpFrequency * _jumpPhase);
            _jumpPhase += jumpUpdateSpeed;
            if (_jumpPhase > MaxPhase) _jumpPhase -= MaxPhase;

            var rotation = (float) (vOffset - 0.5) * jumpLeaningEffect * jumpMaxLeaningAngle * Vector3.forward;
            var position = Vector3.up * (float) vOffset * jumpHeight;
            
            body.localPosition = _basePosition + position;
            body.localRotation = _baseRotation * Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        }

        public void ShowBodyParts()
        {
            if (!_hiding) return;
            if (animator != null)
            {
                animator.SetBool("IsHiding", false);
            }
            _hiding = false;
            body.transform.position += bodyStartOffset * Vector3.up;
            UpdateBodyParts();
        }

        public void HideBodyParts()
        {
            if (_hiding) return;
            if (animator != null)
            {
                animator.SetBool("IsHiding", true);
            }
            _hiding = true;
            body.transform.position += bodyStartOffset * Vector3.down;
            UpdateBodyParts();
        }

        private void UpdateBodyParts()
        {
            foreach (GameObject o in bodyParts)
            {
                Renderer objectRenderer = o.GetComponent<Renderer>();
                if (objectRenderer != null)
                {
                    objectRenderer.enabled = !_hiding;
                }
            }
        }

        public override void SetStateSilently(int state)
        {
            base.SetStateSilently(state);
            if (_hiding && State == STATE_FLEEING)
            {
                ShowBodyParts();
            }
            else if (!_hiding && State == STATE_IDLE)
            {
                if (jumpingAnimation)
                {
                    body.localRotation = _baseRotation;
                    body.localPosition = _basePosition;
                }
                HideBodyParts();
            }
        }
    }
}
