using System;
using System.Collections.Generic;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace GameMain
{
    public class ProcedureGame : ProcedureBase
    {
        public override bool UseNativeDialog => false;
        private GameState m_GameState = GameState.Undefined;
        private List<LevelData> m_LevelDatas = null;
        private int? m_GameOverFormID = 0;
        private int m_CurrentLevelID = 0;
        private int m_LevelCount = 0;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Event.Subscribe(ChangeGameStateEventArgs.EventId,ChangeGameState);
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId,ShowEntitySuccess);
            GameEntry.Event.Subscribe(HideEntityCompleteEventArgs.EventId,OnHideEntityComplete);
            GameEntry.Event.Subscribe(ChangeLevelEventArgs.EventId,ChangeLevel);

            GameEntry.UI.OpenUIForm(UIFormId.ItemForm);
            m_LevelDatas = new List<LevelData>();
            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            m_LevelCount = 1;
            GameEntry.DataNode.GetOrAddNode("LEVEL_COUNT");
            GameEntry.DataNode.SetData<VarInt32>("LEVEL_COUNT",m_LevelCount);
            m_CurrentLevelID = GameEntry.DataNode.GetData<VarInt32>("LEVEL_ID");
            
            for (int i = 0; i < m_LevelCount; i++)
            {
                var levelData = new LevelData(GameEntry.Entity.GenerateSerialId(), 10000 + i);
                Debug.Log(levelData.Id);
                m_LevelDatas.Add(levelData);
            }
            GameEntry.Entity.ShowLevel(m_LevelDatas[m_CurrentLevelID]);
            m_GameState = GameState.Game;
        }
        
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
#if DEBUG || UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Q))
            {
                GameEntry.Event.Fire(this,ChangeLevelEventArgs.Create());
            }
#endif
            switch (m_GameState)
            {
                case GameState.Undefined:
                    break;
                case GameState.Game:
                    break;
                case GameState.Next:
                    break;
                case GameState.BackToMenu:
                    m_GameState = GameState.Undefined;
                    procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
                    ChangeState<ProcedureChangeScene>(procedureOwner);
                    break;
                case GameState.GameFailed:
                    break;
                case GameState.GameClear:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(ChangeGameStateEventArgs.EventId,ChangeGameState);
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId,ShowEntitySuccess);
            GameEntry.Event.Unsubscribe(HideEntityCompleteEventArgs.EventId,OnHideEntityComplete);
            GameEntry.Event.Unsubscribe(ChangeLevelEventArgs.EventId,ChangeLevel);
            base.OnLeave(procedureOwner, isShutdown);
        }

        private void ChangeGameState(object sender, GameEventArgs e)
        {
            ChangeGameStateEventArgs ne = (ChangeGameStateEventArgs)e;
            m_GameState = ne.GameState;
            switch (m_GameState)
            {
                case GameState.Game:
                    break;
                case GameState.GameFailed:
                    m_GameOverFormID = GameEntry.UI.OpenUIForm(UIFormId.GameOverForm);
                    break;
            }
        }

        private void ChangeLevel(object sender, GameEventArgs e)
        {
            ChangeLevelEventArgs ne = (ChangeLevelEventArgs)e;
            GameEntry.Entity.HideEntity(m_LevelDatas[m_CurrentLevelID].Id);
        }

        private void ShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.Entity.Id != m_LevelDatas[m_CurrentLevelID].Id)
                return;
            GameEntry.Event.Fire(this,ChangeGameStateEventArgs.Create(GameState.Game));
        }

        private void OnHideEntityComplete(object sender, GameEventArgs e)
        {
            HideEntityCompleteEventArgs ne = (HideEntityCompleteEventArgs)e;
            if (ne.EntityId != m_LevelDatas[m_CurrentLevelID].Id)
                return;
            if (m_CurrentLevelID + 1 < m_LevelCount)
            {
                m_CurrentLevelID++;
                Debug.Log(m_CurrentLevelID);
                ChangeToNextLevel();
            }
            else
            {
                m_GameState = GameState.BackToMenu;
            }
            GameEntry.DataNode.SetData<VarInt32>("LEVEL_ID",m_CurrentLevelID);
        }
        
        private void ChangeToNextLevel()
        {
            GameEntry.Entity.ShowLevel(m_LevelDatas[m_CurrentLevelID]);
        }
    }
}
