using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class GameOverForm : UGuiForm
    {
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            
        }
        
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
        
        public void OnRestartButtonClick()
        {
            GameEntry.Event.Fire(this,GameResetEventArgs.Create());
            GameEntry.Event.Fire(this,ChangeGameStateEventArgs.Create(GameState.Game));
            Close();
        }
        
        public void OnQuitButtonClick()
        {
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit);
        }
    }
}
