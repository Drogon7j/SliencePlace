using System.Collections;
using System.Collections.Generic;
using CourseMain;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class ChangeItemStateEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ChangeItemStateEventArgs).GetHashCode();
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

        public int ItemNum
        {
            get;
            set;
        }

        public static ChangeItemStateEventArgs Create(PlayerController.PlayerState  playerState,int itemNum)
        {
            ChangeItemStateEventArgs changeItemStateEventArgs = ReferencePool.Acquire<ChangeItemStateEventArgs>();
            changeItemStateEventArgs.PlayerState = playerState;
            changeItemStateEventArgs.ItemNum = itemNum;
            return changeItemStateEventArgs;
        }

        public override void Clear()
        {
            
        }
    }
}