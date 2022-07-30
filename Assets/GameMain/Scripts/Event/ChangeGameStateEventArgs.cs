using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class ChangeGameStateEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ChangeGameStateEventArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public GameState GameState
        {
            get; 
            private set;
        }

        public static ChangeGameStateEventArgs Create(GameState gameState)
        {
            ChangeGameStateEventArgs changeGameStateEventArgs = ReferencePool.Acquire<ChangeGameStateEventArgs>();
            changeGameStateEventArgs.GameState = gameState;
            return changeGameStateEventArgs;
        }

        public override void Clear()
        {
            
        }
    }
}