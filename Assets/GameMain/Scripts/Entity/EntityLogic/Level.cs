﻿using GameFramework.Event;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Level : Entity
    {
        [SerializeField] private LevelData mLevelData = null;
        private GameObject m_Invisible = null;
        private GameObject m_ResetPosition = null;

        private GameObject[] m_InvisibleArray = null;
        private GameObject[] m_ResetObjArray = null;
        private Vector3[] m_ResetPositionArray = null;
        protected override void OnShow(object userData)

        {
            base.OnShow(userData);
            mLevelData = userData as LevelData;
            GameEntry.Event.Subscribe(GameResetEventArgs.EventId,OnReset);
            
            InitInvisible();
            InitPosition();
            if (mLevelData == null)
            {
                Log.Error("Effect data is invalid.");
                return;
            }
            ResetLevel();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            GameEntry.Event.Unsubscribe(GameResetEventArgs.EventId,OnReset);
            base.OnHide(isShutdown, userData);
        }

        private void OnReset(object sender, GameEventArgs e)
        {
            ResetLevel();
        }

        private void ResetLevel()
        {
            ResetInvisible();
            ResetPosition();
        }

        private void InitInvisible()
        {
            m_Invisible = transform.GetChild(0).gameObject;
            m_InvisibleArray = new GameObject[m_Invisible.transform.childCount];
            for (int i = 0; i < m_Invisible.transform.childCount; i++)
            {
                m_InvisibleArray[i] = m_Invisible.transform.GetChild(i).gameObject;
            }
        }
        
        private void InitPosition()
        {
            m_ResetPosition = transform.GetChild(1).gameObject;
            m_ResetObjArray = new GameObject[m_ResetPosition.transform.childCount];
            m_ResetPositionArray = new Vector3[m_ResetPosition.transform.childCount];
            for (int i = 0; i < m_ResetPosition.transform.childCount; i++)
            {
                m_ResetObjArray[i] = m_ResetPosition.transform.GetChild(i).gameObject;
                m_ResetPositionArray[i] = m_ResetPosition.transform.GetChild(i).transform.position;
            }
        }

        private void ResetInvisible()
        {
            for (int i = 0; i < m_InvisibleArray.Length; i++)
            {
                m_InvisibleArray[i].transform.GetChild(0).GetComponent<TilemapRenderer>().enabled = false;
            }
        }

        private void ResetPosition()
        {
            for (int i = 0; i < m_ResetPositionArray.Length; i++)
            {
                m_ResetObjArray[i].transform.position = m_ResetPositionArray[i];
            }
        }
    }
}