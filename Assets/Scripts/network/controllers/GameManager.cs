using System;
using UnityEngine;

namespace network.controllers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public Transform[] redSpawnPoints;
        public Transform[] blueSpawnPoints;
        [HideInInspector]
        public int nextPlayersTeam = 0;
        [HideInInspector]
        public uint[] scores = new uint[2];

        private void OnEnable()
        {
            if(Instance == null) Instance = this;
        }

        public void UpdateTeam()
        {
            nextPlayersTeam = (nextPlayersTeam + 1) % 2;
        }
    }
}