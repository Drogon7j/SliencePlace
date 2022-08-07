using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class ChangePlayerStateEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ChangePlayerStateEventArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public PlayerController.PlayerState PlayerState
        {
            get; 
            private set;
        }

        public static ChangePlayerStateEventArgs Create(PlayerController.PlayerState playerState)
        {
            ChangePlayerStateEventArgs changePlayerStateEventArgs = ReferencePool.Acquire<ChangePlayerStateEventArgs>();
            changePlayerStateEventArgs.PlayerState = playerState;
            return changePlayerStateEventArgs;
        }

        public override void Clear()
        {
            
        }
    }
}