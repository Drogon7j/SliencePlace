using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class OnEnemyTriggerEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(OnEnemyTriggerEventArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public GameObject Enemy
        {
            get;
            set;
        }

        public static OnEnemyTriggerEventArgs Create(GameObject enemy)
        {
            OnEnemyTriggerEventArgs onEnemyTriggerEventArgs = ReferencePool.Acquire<OnEnemyTriggerEventArgs>();
            onEnemyTriggerEventArgs.Enemy = enemy;
            return onEnemyTriggerEventArgs;
        }

        public override void Clear()
        {
            
        }
    }
}