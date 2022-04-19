using System;
using UnityEngine;

namespace player
{
    public class AttackEvent : MonoBehaviour
    {
        public event Action OnAttackStarted;

        void TriggerAttackStart()
        {
            OnAttackStarted?.Invoke();
        }
        
        public event Action OnAttackEnded;

        void TriggerAttackEnd()
        {
            OnAttackEnded?.Invoke();
        }
    }
}