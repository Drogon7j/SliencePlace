using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using GameMain;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    private void Start()
    {
        //GameEntry.Event.Subscribe(ChangeGameStateEventArgs.EventId,OnReset);
        m_Rigid = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        GetInput();
    }

    private void OnDisable()
    {
        //GameEntry.Event.Unsubscribe(ChangeGameStateEventArgs.EventId,OnReset);
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
        Debug.Log(TargetAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.Euler(transform.rotation.x, transform.rotation.y, -TargetAngle), 0.5f);

        if (m_DMag > 0.1f)
        {
            
        }
    }
    
    private Vector2 SquareToCircle(Vector2 input)
    {
        var output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }

    private void OnReset(object sender,GameEventArgs e)
    {
        ChangeGameStateEventArgs ne = (ChangeGameStateEventArgs)e;
        switch (ne.GameState)
        {
            case GameState.Reset:
                transform.position = Vector3.zero;
                break;
        }
    }
}
