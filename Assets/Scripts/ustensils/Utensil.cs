using System.Collections.Generic;
using mikunis;
using UnityEngine;

namespace player
{
    public abstract class Utensil
    {
        private readonly Transform player;
        private readonly double speed;
        private readonly double range;
        private readonly uint capacity;
        private readonly double strength;

        public double Speed => speed;
        public double Range => range;
        public uint Capacity => capacity;
        public double Strength => strength;

        public Utensil(Transform player, double speed, double range, uint capacity, double strength)
        {
            this.speed = speed;
            this.range = range;
            this.capacity = capacity;
            this.strength = strength;
        }

        public List<Mikuni> GetMikunisInRange()
        {
            return new List<Mikuni>();
        }
    }
}