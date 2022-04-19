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

        private void Awake()
        {
            hitBox = GetComponent<Collider>();
            hitBox.enabled = false;
        }
        
        public void StartCapturingSession()
        {
            hitBox.enabled = true;
            _capturing = true;
            capturedCount = 0;
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
                OnMikuniDetected?.Invoke(mikuni);
            }
        }

        public void IncrementCaptureCounter()
        {
            if (!_capturing) return;
            capturedCount++;
        }
    }
}