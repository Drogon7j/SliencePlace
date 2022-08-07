using System.Collections;
using System.Collections.Generic;
using CourseMain;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class ChangeEnemyStateEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ChangeEnemyStateEventArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public EnemyController.EnemyState EnemyState
        {
            get; 
            private set;
        }

        public static ChangeEnemyStateEventArgs Create(EnemyController.EnemyState enemyState)
        {
            ChangeEnemyStateEventArgs changeEnemyStateEventArgs = ReferencePool.Acquire<ChangeEnemyStateEventArgs>();
            changeEnemyStateEventArgs.EnemyState = enemyState;
            return changeEnemyStateEventArgs;
        }

        public override void Clear()
        {
            
        }
    }
}