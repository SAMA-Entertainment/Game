using System;
using UnityEngine;

namespace network
{
    public class PlayerInfo : MonoBehaviour
    {
        public static readonly string PLAYER_SKIN = "PlayerSkin";
        public static PlayerInfo PInfo;

        public int selectedSkin;
        public GameObject[] allCharacters;

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
            if (PlayerPrefs.HasKey(PLAYER_SKIN))
            {
                selectedSkin = PlayerPrefs.GetInt(PLAYER_SKIN);
            }
            else
            {
                SetSelectedSkin(0);
            }
        }

        public void SetSelectedSkin(int selectedSkin)
        {
            this.selectedSkin = selectedSkin;
            PlayerPrefs.SetInt(PLAYER_SKIN, selectedSkin);
        }

    }
}
