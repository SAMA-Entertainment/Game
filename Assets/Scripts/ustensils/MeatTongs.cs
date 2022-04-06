using UnityEngine;

namespace player
{
    public class MeatTongs : Utensil
    {
        public MeatTongs(Transform player) : base(player, 4, 4, 6, 8)
        {
        }
    }
}