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
            float ratio = movement.Stamina / movement.maxstamina;
            progressBar.value = ratio;
        }
    
    }
}
