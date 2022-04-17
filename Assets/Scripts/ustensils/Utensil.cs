using UnityEngine;

namespace ustensils
{
    public class Utensil : MonoBehaviour
    {
        public double speed;
        public double range;
        public uint capacity;
        public double strength;

        public Collider hitBox;
    }
}