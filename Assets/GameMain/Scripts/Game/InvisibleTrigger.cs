using System;
using System.Collections;
using System.Collections.Generic;
using GameMain;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InvisibleTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name != "Player")
            return;
        gameObject.GetComponent<TilemapRenderer>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name != "Player")
            return;
        gameObject.GetComponent<TilemapRenderer>().enabled = true;
    }
}