using System;
using UnityEngine;

namespace network.controllers
{
    public class GameSetup : MonoBehaviour
    {
        public static GameSetup Instance;
        public Transform[] spawnPoints; 

        private void OnEnable()
        {
            if(Instance == null) Instance = this;
        }
    }
}