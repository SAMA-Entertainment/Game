using player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace menus
{
    public class PlayerHUD : MonoBehaviour
    {
        public static PlayerHUD HUD;
        
        [HideInInspector]
        public ThirdPersonMovement movement;
        [HideInInspector] 
        public CatchMikuni mikuniCatchController;
        
        public Slider progressBar;
        public GameObject mikuniCounterObject;
        
        private void OnEnable()
        {
            if (HUD == null) HUD = this;
        }

        void LateUpdate()
        {
            if (movement == null || mikuniCatchController == null) return;
            float ratio = movement.Stamina / movement.maxstamina;
            progressBar.value = ratio;
            mikuniCounterObject.GetComponent<TextMeshProUGUI>().text = 
                $"Mikunis: {mikuniCatchController.MikuniCatched}";
        }
    
    }
}
