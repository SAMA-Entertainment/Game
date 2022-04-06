using UnityEngine;

namespace player
{
    public class GrappelingFork : Utensil
    {
        public GrappelingFork(Transform player) : base(player, 5, 10, 1, 10)
        {
        }
    }
}