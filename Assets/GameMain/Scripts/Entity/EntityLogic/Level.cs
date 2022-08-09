using GameFramework.Event;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Level : Entity
    {
        public enum LevelState
        {
            
        }
        
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
            GameEntry.Event.Subscribe(ShowMapEventArgs.EventId,ShowMap);
            
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
            GameEntry.Event.Unsubscribe(ShowMapEventArgs.EventId,ShowMap);
            base.OnHide(isShutdown, userData);
        }

        private void OnReset(object sender, GameEventArgs e)
        {
            GameResetEventArgs ne = (GameResetEventArgs)e;
            ResetLevel();
            if (ne.Type == 1)
            {
                GameEntry.Event.Fire(this,ChangeGameStateEventArgs.Create(GameState.BackToMenu));
            }
        }

        private void ResetLevel()
        {
            ResetInvisible();
            ResetPosition();
        }

        private void InitInvisible()
        {
            m_Invisible = transform.GetChild(0).GetChild(0).gameObject;
            if (m_Invisible.transform.childCount == 0)
                return;
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
            if (m_Invisible.transform.childCount == 0)
                return;
            for (int i = 0; i < m_InvisibleArray.Length; i++)
            {
                TilemapRenderer tilemapRenderer = null;
                m_InvisibleArray[i].TryGetComponent<TilemapRenderer>(out tilemapRenderer);
                if (tilemapRenderer != null)
                {
                    tilemapRenderer.enabled = false;
                }
                
                SpriteRenderer spriteRenderer = null;
                m_InvisibleArray[i].TryGetComponent<SpriteRenderer>(out spriteRenderer);
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = false;
                }
            }
        }

        private void ResetPosition()
        {
            for (int i = 0; i < m_ResetPositionArray.Length; i++)
            {
                m_ResetObjArray[i].transform.position = m_ResetPositionArray[i];
                m_ResetObjArray[i].gameObject.SetActive(true);
            }
        }
        
        private void ShowInvisible()
        {
            for (int i = 0; i < m_InvisibleArray.Length; i++)
            {
                TilemapRenderer tilemapRenderer = null;
                m_InvisibleArray[i].TryGetComponent<TilemapRenderer>(out tilemapRenderer);
                if (tilemapRenderer != null)
                {
                    tilemapRenderer.enabled = true;
                }
                
                SpriteRenderer spriteRenderer = null;
                m_InvisibleArray[i].TryGetComponent<SpriteRenderer>(out spriteRenderer);
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = true;
                }
            }
        }

        private void ShowMap(object sender, GameEventArgs e)
        {
            ShowInvisible();
        }
    }
}