using System;
using System.Collections;
using System.Collections.Generic;
using GameMain;
using UnityEngine;

public class EndPointCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameEntry.Event.Fire(this,ChangeLevelEventArgs.Create());
    }
}
