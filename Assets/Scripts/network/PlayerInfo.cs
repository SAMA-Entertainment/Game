using System;
using UnityEngine;

namespace network
{
    public class PlayerInfo : MonoBehaviour
    {
        public static readonly string PLAYER_SKIN = "PlayerSkin";
        public static readonly string PLAYER_USTENCIL = "PlayerUstencil";
        public static PlayerInfo PInfo;

        public GameObject[] allCharacters;
        public GameObject[] allUstencils;
        
        [HideInInspector]
        public int selectedSkin;
        [HideInInspector]
        public int selectedUstencil;

        private void OnEnable()
        {
            if(PInfo != null && PInfo != this)
            {
                Destroy(PInfo.gameObject);
            }
            PInfo = this;
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            if (PlayerPrefs.HasKey(PLAYER_SKIN)) selectedSkin = PlayerPrefs.GetInt(PLAYER_SKIN);
            else SetSelectedSkin(0);

            if (PlayerPrefs.HasKey(PLAYER_USTENCIL)) selectedUstencil = PlayerPrefs.GetInt(PLAYER_USTENCIL);
            else SetSelectedUstencil(0);
        }

        public void SetSelectedSkin(int selectedSkin)
        {
            this.selectedSkin = selectedSkin;
            PlayerPrefs.SetInt(PLAYER_SKIN, selectedSkin);
        }
        
        public void SetSelectedUstencil(int selectedUstencil)
        {
            this.selectedUstencil = selectedUstencil;
            PlayerPrefs.SetInt(PLAYER_USTENCIL, selectedUstencil);
        }

    }
}
