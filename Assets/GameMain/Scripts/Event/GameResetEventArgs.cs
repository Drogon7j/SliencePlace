using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class GameResetEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(GameResetEventArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int Type
        {
            get;
            set;
        }

        public static GameResetEventArgs Create(int type)
        {
            GameResetEventArgs gameResetEventArgs = ReferencePool.Acquire<GameResetEventArgs>();
            gameResetEventArgs.Type = type;
            return gameResetEventArgs;
        }

        public override void Clear()
        {
            
        }
    }
}