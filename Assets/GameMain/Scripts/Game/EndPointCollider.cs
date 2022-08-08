using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using GameMain;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class EndPointCollider : MonoBehaviour
    {
        [SerializeField] private bool mIsHide = false;
        private BoxCollider2D m_Collider2D = null;
        private SpriteRenderer m_Renderer = null;

        private void OnEnable()
        {
            if (!mIsHide)
                return;
            GameEntry.Event.Subscribe(ShowMapEventArgs.EventId, ShowMap);
            m_Collider2D = GetComponent<BoxCollider2D>();
            m_Renderer = GetComponent<SpriteRenderer>();
            m_Collider2D.enabled = false;
            m_Renderer.enabled = false;
        }

        private void OnDisable()
        {
            if (!mIsHide)
                return;
            GameEntry.Event.Unsubscribe(ShowMapEventArgs.EventId, ShowMap);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var value = GameEntry.DataNode.GetData<VarInt32>("LEVEL_COUNT");
            if (GameEntry.DataNode.GetData<VarInt32>("LEVEL_ID")
                < value - 1)
            {
                GameEntry.Sound.PlaySound(10008);
                Invoke(nameof(ChangeLevel), 6.0f);
            }
            else
            {
                GameEntry.Sound.PlaySound(10009);
                Invoke(nameof(ChangeLevel), 11.0f);
            }
        }

        private void ChangeLevel()
        {

            GameEntry.Event.Fire(this, ChangeLevelEventArgs.Create());
        }

        private void ShowMap(object sender, GameEventArgs e)
        {
            GameEntry.Sound.PlaySound(10001);
            m_Collider2D.enabled = true;
            m_Renderer.enabled = true;
        }
    }
}