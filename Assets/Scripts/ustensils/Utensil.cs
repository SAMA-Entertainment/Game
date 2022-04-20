using System;
using mikunis;
using UnityEngine;

namespace ustensils
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class Utensil : MonoBehaviour
    {
        public double speed;
        public double range;
        public uint capacity;
        public double strength;

        public event Action<Mikuni> OnMikuniDetected;
        private Collider hitBox;
        private bool _capturing;
        private int capturedCount;

        /**
         * Returns the the number of Mikuni captured during the last capturing session
         */
        public int CapturedCount => capturedCount;

        void Awake()
        {
            hitBox = GetComponent<Collider>();
            hitBox.enabled = false;
        }
        
        public void StartCapturingSession()
        {
            hitBox.enabled = true;
            if(!_capturing) capturedCount = 0;
            _capturing = true;
        }

        public void StopCapturingSession()
        {
            hitBox.enabled = false;
            _capturing = false;
        }

        void OnTriggerEnter(Collider other)
        {
            if (capturedCount >= capacity) return;
            if (other.gameObject.CompareTag("Mikuni"))
            {
                Mikuni mikuni = other.GetComponent<Mikuni>();
                if (mikuni != null)
                {
                    OnMikuniDetected?.Invoke(mikuni);
                }
            }
        }

        public void IncrementCaptureCounter()
        {
            if (!_capturing) return;
            capturedCount++;
        }
    }
}