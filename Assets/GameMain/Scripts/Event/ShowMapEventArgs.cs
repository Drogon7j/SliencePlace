using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class ShowMapEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ShowMapEventArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static ShowMapEventArgs Create()
        {
            ShowMapEventArgs showMapEventArgs = ReferencePool.Acquire<ShowMapEventArgs>();
            return showMapEventArgs;
        }

        public override void Clear()
        {
            
        }
    }
}