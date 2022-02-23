using System;
using player;
using UnityEngine;
using UnityEngine.UI;

namespace menus
{
    public class PlayerStamina : MonoBehaviour
    {
        public static PlayerStamina Stamina;
        
        public ThirdPersonMovement movement;
        public Slider progressBar;
        
        private void OnEnable()
        {
            if (Stamina == null) Stamina = this;
        }

        void Update()
        {
            if (movement == null) return;
            float ratio = movement.Stamina / movement.maxstamina;
            progressBar.value = ratio;
        }
    
    }
}
