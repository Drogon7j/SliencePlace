using System;
using System.Collections.Generic;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace GameMain
{
    public class ProcedureGame : ProcedureBase
    {
        public override bool UseNativeDialog => false;
        private GameState m_GameState = GameState.Undefined;
        private int? m_GameOverFormID = 0;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Event.Subscribe(ChangeGameStateEventArgs.EventId,ChangeGameState);
            
            m_GameState = GameState.Game;
        }
        
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            switch (m_GameState)
            {
                case GameState.Undefined:
                    break;
                case GameState.Game:
                    break;
                case GameState.Next:
                    break;
                case GameState.BackToMenu:
                    procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
                    ChangeState<ProcedureChangeScene>(procedureOwner);
                    m_GameState = GameState.Undefined;
                    break;
                case GameState.Reset:
                    break;
                case GameState.GameFailed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(ChangeGameStateEventArgs.EventId,ChangeGameState);
            
            
        }

        private void ChangeGameState(object sender, GameEventArgs e)
        {
            ChangeGameStateEventArgs ne = (ChangeGameStateEventArgs)e;
            m_GameState = ne.GameState;
            switch (m_GameState)
            {
                case GameState.Reset:
                    if (m_GameOverFormID != null)
                        GameEntry.UI.CloseUIForm(m_GameOverFormID.Value);
                    break;
                case GameState.GameFailed:
                    m_GameOverFormID = GameEntry.UI.OpenUIForm(UIFormId.GameOverForm);
                    break;
            }
        }
    }
}
