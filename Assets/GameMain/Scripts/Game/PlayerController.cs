using System;
using System.Collections;
using System.Collections.Generic;
using CourseMain;
using DG.Tweening;
using GameFramework.Event;
using GameMain;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Undefined,
        NoArms,
        GetKnife,
        GetGun,
        GetZeus,
        GetSonar,
        UseItem,
    }
    private PlayerState m_PlayerState = PlayerState.Undefined;
    private PlayerState m_NextState = PlayerState.Undefined;
    
    [Header("InputKey")] 
    private string m_KeyUp = "w";
    private string m_KeyDown = "s";
    private string m_KeyLeft = "a";
    private string m_KeyRight = "d";

    [Header("PlayerData")] 
    [SerializeField] private GameObject mPlayerModel = null;
    [SerializeField] private float mMoveSpeed = 0;
    [SerializeField] private float mMoveToggle = 0;
    [SerializeField] private float mRotateToggle= 0;
    [SerializeField] private bool mCameraFollow = false;
    
    [Header("ItemData")] 
    [SerializeField] private int mKnifeNum = 0;
    [SerializeField] private int mGunNum = 0;
    [SerializeField] private int mZeusNum = 0;
    [SerializeField] private int mSonarNum = 0;

    private GameObject[] m_AttackSoundPosts = null;
    
    private int m_NowKnifeNum = 0;
    private int m_NowGunNum = 0;
    private int m_NowZeusNum = 0;
    private int m_NowSonarNum = 0;

    private float m_NowTime = 0;
    private float m_TargetTime = 1.0f;
    private bool m_StartCountSignal = false;
    
    private Rigidbody2D m_Rigid = null;
    private float m_DUp = 0;
    private float m_DRight = 0;
    
    private float m_StopTime = 0.5f;
    private float m_RotateSpeed = 0.2f;
    private float m_Threshold = 0.1f;
    
    private float m_TargetDUp = 0;
    private float m_TargetDRight = 0;
    private float m_VelocityDup = 0;
    private float m_VelocityDRight = 0;
    private float m_VelocityDmag = 0;
    
    private float m_DMag = 0;
    private Vector3 m_DVec = Vector3.zero;

    private Vector2 m_CircleInput = Vector2.zero;
    private Vector3 m_MovingVec = Vector3.zero;

    private GameObject[] m_SoundPosts = null;
    private GameObject m_Mask = null;
    private GameState m_GameState = GameState.Undefined;


    private bool m_IsPlayFoot = false;
    private int? m_FootSound = null;
    private void Awake()
    {
        m_Rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        m_FootSound = new int();
        GameEntry.Event.Subscribe(ChangeGameStateEventArgs.EventId,ChangeGameState);
        GameEntry.Event.Subscribe(ChangePlayerStateEventArgs.EventId,ChangePlayerState);
        GameEntry.Event.Subscribe(OnEnemyTriggerEventArgs.EventId,OnEnemyTrigger);
        m_SoundPosts = new GameObject[transform.GetChild(0).childCount];
        m_Mask = transform.GetChild(1).gameObject;
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            m_SoundPosts[i] = transform.GetChild(0).GetChild(i).gameObject;
        }
        
        m_AttackSoundPosts = new GameObject[transform.GetChild(2).childCount];
        for (int i = 0; i < transform.GetChild(2).childCount; i++)
        {
            m_AttackSoundPosts[i] = transform.GetChild(2).GetChild(i).gameObject;
        }
        
        m_PlayerState = PlayerState.NoArms;
        m_IsPlayFoot = false;
        m_StartCountSignal = false;
        m_NextState = PlayerState.Undefined;
    }

    private void Update()
    {
        Debug.Log(m_PlayerState);
        if (m_GameState != GameState.Game)
            return;
        GameEntry.Event.Fire(this,SendPlayerPositionEventArgs.Create(transform.position));
        switch (m_PlayerState)
        {
            case PlayerState.Undefined:
                RandomSoundPosts();
                return;
                break;
            case PlayerState.NoArms:
                GetInput();
                WalkSoundPosts();
                break;
            case PlayerState.GetKnife:
                GetInput();
                WalkSoundPosts();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(2).gameObject.SetActive(true);
                    RandomAttackSoundPosts();
                    m_StartCountSignal = true;
                    m_NextState = PlayerState.GetKnife;
                    m_PlayerState = PlayerState.UseItem;
                }
                break;
            case PlayerState.GetGun:
                
                break;
            case PlayerState.GetZeus:
                break;
            case PlayerState.GetSonar:
                GetInput();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_PlayerState = PlayerState.UseItem;
                    m_Mask.transform.DOScale(Vector3.one * 4, 0.5f).OnComplete(() =>
                    {
                        m_Mask.transform.DOScale(Vector3.one * 4.01f, 2.5f).OnComplete(() =>
                        {
                            m_Mask.transform.DOScale(Vector3.zero, 1.0f).OnComplete(() =>
                            {
                                m_StartCountSignal = true;
                                m_NextState = PlayerState.GetSonar;
                            });
                        });
                    });
                    return;
                }
                WalkSoundPosts();
                break;
            case PlayerState.UseItem:
                GetInput();
                WalkSoundPosts();
                if (!m_StartCountSignal)
                    return;
                m_NowTime += Time.deltaTime;
                if (m_NowTime >= m_TargetTime)
                {
                    switch (m_NextState)
                    {
                        case PlayerState.NoArms:
                            break;
                        case PlayerState.GetKnife:
                            m_NowKnifeNum++;
                            transform.GetChild(0).gameObject.SetActive(true);
                            transform.GetChild(2).gameObject.SetActive(false);
                            if (m_NowKnifeNum < mKnifeNum)
                            {
                                m_PlayerState = m_NextState;
                                m_StartCountSignal = false;
                                m_NowTime = 0;
                            }
                            else
                            {
                                m_PlayerState = PlayerState.NoArms;
                                m_StartCountSignal = false;
                                m_NowTime = 0;
                            }
                            break;
                        case PlayerState.GetGun:
                            m_NowGunNum++;
                            if (m_NowGunNum < mGunNum)
                            {
                                m_PlayerState = m_NextState;
                                m_StartCountSignal = false;
                                m_NowTime = 0;
                            }
                            else
                            {
                                m_PlayerState = PlayerState.NoArms;
                                m_StartCountSignal = false;
                                m_NowTime = 0;
                            }
                            break;
                        case PlayerState.GetSonar:
                            m_NowSonarNum++;
                            if (m_NowSonarNum < mSonarNum)
                            {
                                m_PlayerState = m_NextState;
                                m_StartCountSignal = false;
                                m_NowTime = 0;
                            }
                            else
                            {
                                m_PlayerState = PlayerState.NoArms;
                                m_StartCountSignal = false;
                                m_NowTime = 0;
                            }
                            break;
                    }
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (mCameraFollow)
        {
            Camera.main.gameObject.transform.position = new Vector3(transform.position.x,
                transform.position.y, Camera.main.gameObject.transform.position.z);
        }
    }

    private void OnDisable()
    {
        GameEntry.Event.Unsubscribe(ChangeGameStateEventArgs.EventId,ChangeGameState);
        GameEntry.Event.Unsubscribe(ChangePlayerStateEventArgs.EventId,ChangePlayerState);
        GameEntry.Event.Unsubscribe(OnEnemyTriggerEventArgs.EventId,OnEnemyTrigger);
    }

    private void GetInput()
    {
        m_TargetDUp = (Input.GetKey(m_KeyUp) ? 1.0f : 0) - (Input.GetKey(m_KeyDown) ? 1.0f : 0);
        m_TargetDRight = (Input.GetKey(m_KeyRight) ? 1.0f : 0) - (Input.GetKey(m_KeyLeft) ? 1.0f : 0);

        m_DUp = Mathf.SmoothDamp(m_DUp, m_TargetDUp, ref m_VelocityDup, mMoveToggle);
        m_DRight = Mathf.SmoothDamp(m_DRight, m_TargetDRight, ref m_VelocityDRight, mMoveToggle);
        
        m_CircleInput = SquareToCircle(new Vector2(m_DRight, m_DUp));
        m_DMag = Mathf.Sqrt(m_CircleInput.y * m_CircleInput.y) + (m_CircleInput.x * m_CircleInput.x);
        //m_DVec = m_CircleInput.x * transform.right + m_CircleInput.y * transform.up;
        //transform.position += new Vector3(m_CircleInput.x,m_CircleInput.y,0) * mMoveSpeed;
        m_Rigid.velocity = new Vector2(m_CircleInput.x,m_CircleInput.y) * mMoveSpeed;
        m_DMag = Mathf.Clamp(m_DMag * 1.5f, 0, 1f);
        // mPlayerModel.transform.up = Vector2.Lerp(mPlayerModel.transform.up, 
        //     new Vector2(m_CircleInput.x, m_CircleInput.y), mRotateToggle);
        var TargetAngle = Vector2.SignedAngle(m_CircleInput, Vector2.up);
        //Debug.Log(TargetAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.Euler(transform.rotation.x, transform.rotation.y, -TargetAngle), 0.5f);
        
    }

    private void WalkSoundPosts()
    {
        if (m_DMag > 0.1f)
        {
            RandomSoundPosts();
            if (!m_IsPlayFoot)
            {
                m_FootSound = GameEntry.Sound.PlaySound(10000);
                m_IsPlayFoot = true;
            }
        }
        else
        {
            ResetSoundPosts();
            if (m_IsPlayFoot)
            {
                if (m_FootSound != null) 
                    GameEntry.Sound.StopSound((int)m_FootSound);
                m_IsPlayFoot = false;
            }
        }
    }
    
    private Vector2 SquareToCircle(Vector2 input)
    {
        var output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }

    private void ChangeGameState(object sender,GameEventArgs e)
    {
        ChangeGameStateEventArgs ne = (ChangeGameStateEventArgs)e;
        m_GameState = ne.GameState;
        if (m_GameState == GameState.GameFailed)
        {
            m_PlayerState = PlayerState.NoArms;
        }
    }

    private void ChangePlayerState(object sender, GameEventArgs e)
    {
        ChangePlayerStateEventArgs ne = (ChangePlayerStateEventArgs)e;
        m_PlayerState = ne.PlayerState;
        switch (m_PlayerState)
        {
            case PlayerState.Undefined:
                break;
            case PlayerState.NoArms:
                break;
            case PlayerState.GetKnife:
                m_NowKnifeNum = 0;
                break;
            case PlayerState.GetGun:
                m_NowGunNum = 0;
                break;
            case PlayerState.GetZeus:
                m_NowZeusNum = 0;
                break;
            case PlayerState.GetSonar:
                m_NowSonarNum = 0;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Debug.Log(m_PlayerState);
    }

    private void RandomSoundPosts()
    {
        for (int i = 0; i < m_SoundPosts.Length; i++)
        {
            var scaleY = Random.Range(0.5f, 3.0f);
            var randomVector = new Vector3(m_SoundPosts[i].transform.localScale.x, scaleY,
                m_SoundPosts[i].transform.localScale.z);
            m_SoundPosts[i].transform.localScale =
                Vector3.Lerp(m_SoundPosts[i].transform.localScale, randomVector, 0.5f);
        }
    }

    private void ResetSoundPosts()
    {
        for (int i = 0; i < m_SoundPosts.Length; i++)
        {
            m_SoundPosts[i].transform.localScale =
                Vector3.Lerp(m_SoundPosts[i].transform.localScale, new Vector3(m_SoundPosts[i].transform.localScale.x,
                    0.5f,m_SoundPosts[i].transform.localScale.z), 0.5f);
        }
    }

    private void RandomAttackSoundPosts()
    {
        for (int i = 0; i < m_AttackSoundPosts.Length; i++)
        {
            var scaleY = Random.Range(0.5f, 3.0f);
            var randomVector = new Vector3(m_SoundPosts[i].transform.localScale.x, scaleY,
                m_SoundPosts[i].transform.localScale.z);
            m_SoundPosts[i].transform.localScale =
                Vector3.Lerp(m_SoundPosts[i].transform.localScale, randomVector, 0.5f);
        }
    }

    private void OnEnemyTrigger(object sender, GameEventArgs e)
    {
        OnEnemyTriggerEventArgs ne = (OnEnemyTriggerEventArgs)e;
        switch (m_PlayerState)
        {
            case PlayerState.NoArms:
            case PlayerState.GetKnife:
            case PlayerState.GetGun:
            case PlayerState.GetSonar:
            case PlayerState.UseItem:
                GameEntry.Sound.StopAllLoadedSounds();
                GameEntry.Sound.PlaySound(10002);
                m_PlayerState = PlayerState.Undefined;
                GameEntry.Event.Fire(this, ChangeEnemyStateEventArgs.Create(EnemyController.EnemyState.FollowPlayer));
                Invoke(nameof(GameFailed),7.0f);
                break;
            case PlayerState.GetZeus:
                m_NowZeusNum++;
                if (m_NowZeusNum == mZeusNum)
                {
                    m_PlayerState = PlayerState.NoArms;
                    ne.Enemy.SetActive(false);
                }
                break;
            case PlayerState.Undefined:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void GameFailed()
    {
        GameEntry.Event.Fire(this,ChangeGameStateEventArgs.Create(GameState.GameFailed));
    }
}
