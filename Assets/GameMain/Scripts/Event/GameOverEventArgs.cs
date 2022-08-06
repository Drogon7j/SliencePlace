using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class GameOverEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(GameOverEventArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static GameOverEventArgs Create()
        {
            GameOverEventArgs gameOverEventArgs = ReferencePool.Acquire<GameOverEventArgs>();
            return gameOverEventArgs;
        }

        public override void Clear()
        {
            
        }
    }
}