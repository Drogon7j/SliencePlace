
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class MenuForm : UGuiForm
    {
        [SerializeField] private GameObject mPageMenu = null;
        [SerializeField] private GameObject mPageSelect = null;
        private ProcedureMenu m_ProcedureMenu = null;

        public void OnNumButtonClick(int num)
        {
            GameEntry.DataNode.SetData<VarInt32>("LEVEL_ID",num);
            m_ProcedureMenu.StartGame();
        }
        
        public void OnStartButtonClick()
        {
            mPageMenu.SetActive(false);
            mPageSelect.SetActive(true);
            
        }
        
        public void OnQuitButtonClick()
        {
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit);
            // GameEntry.UI.OpenDialog(new DialogParams()
            // {
            //     Mode = 2,
            //     Title = GameEntry.Localization.GetString("AskQuitGame.Title"),
            //     Message = GameEntry.Localization.GetString("AskQuitGame.Message"),
            //     OnClickConfirm = delegate (object userData) { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); },
            // });
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);
            GameEntry.DataNode.GetOrAddNode("LEVEL_ID");
            m_ProcedureMenu = (ProcedureMenu)userData;
            if (m_ProcedureMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when open MenuForm.");
                return;
            }
            mPageMenu.SetActive(true);
            mPageSelect.SetActive(false);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            m_ProcedureMenu = null;

            base.OnClose(isShutdown, userData);
        }
    }
}
