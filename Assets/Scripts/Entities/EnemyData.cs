using System;
using UnityEngine;

namespace Entities
{
    [Serializable]
    public class EnemyData
    {
        public Vector3 position;
        public int health;
        public int spriteIndex;
        
        public EnemyData(Vector3 position, int health, int spriteIndex)
        {
            this.position = position;
            this.health = health;
            this.spriteIndex = spriteIndex;
        }
    }
}