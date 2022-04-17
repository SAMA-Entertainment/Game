using System;
using UnityEngine;

namespace player
{
    public class AttackEvent : MonoBehaviour
    {
        public event Action OnAttack;

        void TriggerAttack()
        {
            OnAttack?.Invoke();
        }
    }
}