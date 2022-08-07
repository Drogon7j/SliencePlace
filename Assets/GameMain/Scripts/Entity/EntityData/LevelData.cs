using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameMain
{
    [Serializable]
    public class LevelData : EntityData
    {
        public LevelData(int entityId, int typeId) : base(entityId, typeId)
        {
            
        }
    }
}
