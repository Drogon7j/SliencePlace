using System;
using System.Collections;
using System.Collections.Generic;
using GameMain;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InvisibleTrigger : MonoBehaviour
{
    [SerializeField] private bool m_IsSprite = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name != "Player")
            return;
        if (m_IsSprite)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<TilemapRenderer>().enabled = true;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name != "Player")
            return;
        if (m_IsSprite)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<TilemapRenderer>().enabled = true;
        }
    }
}