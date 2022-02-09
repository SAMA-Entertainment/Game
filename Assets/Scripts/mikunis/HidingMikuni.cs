using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace mikunis
{
    public class HidingMikuni : Mikuni
    {

        public float bodyStartOffset;
        public Transform body;
        public Animator animator;
        // List of gameObject to hide when the Mikuni is hiding from the player
        public List<GameObject> bodyParts = new List<GameObject>();

        private bool _hiding;

        // Update is called once per frame
        void Update()
        {
            if (_hiding && State == STATE_FLEEING)
            {
                ShowBodyParts();
            } else if (!_hiding && State == STATE_IDLE)
            {
                HideBodyParts();
            }
        }

        public void ShowBodyParts()
        {
            if (!_hiding) return;
            if (animator != null)
            {
                animator.SetBool("IsRunning", true);
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
                animator.SetBool("IsRunning", false);
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
    }
}
