using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class SendPlayerPositionEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SendPlayerPositionEventArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public Vector3 Position
        {
            get;
            set;
        }

        public static SendPlayerPositionEventArgs Create(Vector3 position)
        {
            SendPlayerPositionEventArgs sendPlayerPositionEventArgs = ReferencePool.Acquire<SendPlayerPositionEventArgs>();
            sendPlayerPositionEventArgs.Position = position;
            return sendPlayerPositionEventArgs;
        }

        public override void Clear()
        {
            
        }
    }
}