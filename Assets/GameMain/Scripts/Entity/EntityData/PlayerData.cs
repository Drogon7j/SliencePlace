using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameMain
{
    [Serializable]
    public class PlayerData : EntityData
    {
        // public float moveSpeed = 0;
        // public float moveToggle = 0;
        // public float rotateSpeed = 0;
        // public float rotateVelocity= 0;

        public PlayerData(int entityId, int typeId, float moveSpeed, float moveToggle, float rotateSpeed,
            float rotateVelocity)
            : base(entityId, typeId)
        {
            // this.moveSpeed = moveSpeed;
            // this.moveSpeed = moveToggle;
            // this.rotateSpeed = rotateSpeed;
            // this.rotateVelocity = rotateVelocity;
        }
    }
}
