using UnityEngine;

namespace player
{
    public class Katana : Utensil
    {
        public Katana(Transform player) : base(player, 6, 2, 4, 10)
        {
        }
    }
}