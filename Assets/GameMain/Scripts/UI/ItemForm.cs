using System;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class ItemForm : UGuiForm
    {
        [SerializeField] private Sprite mNoArm = null;
        [SerializeField] private Sprite mKnife = null;
        [SerializeField] private Sprite mGun = null;
        [SerializeField] private Sprite mZeus = null;
        [SerializeField] private Sprite mSonar = null;
        [SerializeField] private Text mGuide = null;
        [SerializeField] private Text mNum = null;

        private Image m_Image = null;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(ChangeItemStateEventArgs.EventId,ChangeItemState);
            m_Image = transform.GetChild(0).GetComponent<Image>();
            m_Image.sprite = mNoArm;
            mGuide.gameObject.SetActive(false);
            mNum.gameObject.SetActive(false);
        }
        
        protected override void OnClose(bool isShutdown, object userData)
        {
            GameEntry.Event.Unsubscribe(ChangeItemStateEventArgs.EventId,ChangeItemState);
            base.OnClose(isShutdown, userData);
        }

        private void ChangeItemState(object sender, GameEventArgs e)
        {
            ChangeItemStateEventArgs ne = (ChangeItemStateEventArgs)e;
            switch (ne.PlayerState)
            {
                case PlayerController.PlayerState.NoArms:
                    m_Image.sprite = mNoArm;
                    mGuide.gameObject.SetActive(false);
                    mNum.gameObject.SetActive(false);
                    break;
                case PlayerController.PlayerState.GetKnife:
                    m_Image.sprite = mKnife;
                    mGuide.gameObject.SetActive(true);
                    break;
                case PlayerController.PlayerState.GetGun:
                    m_Image.sprite = mGun;
                    mGuide.gameObject.SetActive(true);
                    break;
                case PlayerController.PlayerState.GetZeus:
                    m_Image.sprite = mZeus;
                    mGuide.gameObject.SetActive(false);
                    break;
                case PlayerController.PlayerState.GetSonar:
                    m_Image.sprite = mSonar;
                    mGuide.gameObject.SetActive(true);
                    break;
            }
            if (ne.ItemNum == 0)
            {
                m_Image.sprite = mNoArm;
                mGuide.gameObject.SetActive(false);
                mNum.gameObject.SetActive(false);
            }
            else
            {
                mNum.gameObject.SetActive(true);
                mNum.text = ne.ItemNum.ToString();
            }
        }
    }
}
