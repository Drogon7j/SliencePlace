using System;
using System.Collections;
using System.Collections.Generic;
using GameMain;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoxCollider : MonoBehaviour
{
    [SerializeField] private bool mRandomKnife = false;
    [SerializeField] private bool mRandomGun = false;
    [SerializeField] private bool mRandomZeus = false;
    [SerializeField] private bool mRandomSonar = false;
    [SerializeField] private bool mRandomMap = false;
    private bool m_RandomOver = false;

    private void OnEnable()
    {
        m_RandomOver = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name != "Player")
            return;
        if (!mRandomKnife && !mRandomGun && !mRandomZeus && !mRandomSonar && !mRandomMap)
            return;
        while (!m_RandomOver)
        {
            var randomNum = Random.Range(0, 5);
            //Debug.LogWarning(randomNum);
            switch (randomNum)
            {
                case 0:
                    if (mRandomKnife)
                    {
                        GameEntry.Event.Fire(this,
                            ChangePlayerStateEventArgs.Create(PlayerController.PlayerState.GetKnife));
                        m_RandomOver = true;
                    }
                    break;
                case 1:
                    if (mRandomGun)
                    {
                        GameEntry.Event.Fire(this,
                            ChangePlayerStateEventArgs.Create(PlayerController.PlayerState.GetGun));
                        m_RandomOver = true;
                    }
                    break;
                case 2:
                    if (mRandomZeus)
                    {
                        GameEntry.Event.Fire(this,
                            ChangePlayerStateEventArgs.Create(PlayerController.PlayerState.GetZeus));
                        m_RandomOver = true;
                    }
                    break;
                case 3:
                    if (mRandomSonar)
                    {
                        GameEntry.Event.Fire(this,
                            ChangePlayerStateEventArgs.Create(PlayerController.PlayerState.GetSonar));
                        m_RandomOver = true;
                    }
                    break;
                case 4:
                    if (mRandomMap)
                    {
                        GameEntry.Event.Fire(this, ShowMapEventArgs.Create());
                        m_RandomOver = true;
                    }
                    break;
            }
        }
        gameObject.SetActive(false);
    }
}
