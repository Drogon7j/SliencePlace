using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class ChangeLevelEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ChangeLevelEventArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static ChangeLevelEventArgs Create()
        {
            ChangeLevelEventArgs changeLevelEventArgs = ReferencePool.Acquire<ChangeLevelEventArgs>();
            return changeLevelEventArgs;
        }

        public override void Clear()
        {
            
        }
    }
}