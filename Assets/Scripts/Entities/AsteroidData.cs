using System;
using UnityEngine;

namespace Entities
{
    [Serializable]
    public class AsteroidData
    {
        public Vector3 position;
        public int health;
        public int spriteIndex;
        
        public AsteroidData(Vector3 position, int health, int spriteIndex)
        {
            this.position = position;
            this.health = health;
            this.spriteIndex = spriteIndex;
        }
    }
}