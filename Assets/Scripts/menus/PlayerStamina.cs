using UnityEngine;
using UnityEngine.UI;

namespace menus
{
    public class PlayerStamina : MonoBehaviour
    {
        public ThirdPersonMovement movement;
        public Slider progressBar;

        // Update is called once per frame
        void Update()
        {
            if (movement == null) return; // TODO: Remove this
            float ratio = movement.Stamina / movement.maxstamina;
            progressBar.value = ratio;
        }
    
    }
}
