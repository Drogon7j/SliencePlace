using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using GameMain;
using UnityEngine;
namespace CourseMain
{
	public class EnemyController : MonoBehaviour
	{
		private enum EnemyState
		{
			Undefined,
			Stand,
			Patrol,
			Follow
		}

		private EnemyState m_EnemyState = EnemyState.Undefined;
		private GameState m_GameState = GameState.Undefined;
		[SerializeField] private float mSpeed = 0.5f;
		[SerializeField] private GameObject mEnemyModel = null;
		[SerializeField] private GameObject[] mFollowTargets = null;
		
		private int m_CurrentTarget = 0;
		private bool m_IsFollow = false;
		private float m_StartTime = 0;
		private Vector3 m_StartPos = Vector3.zero;
		// Use this for initialization
		void Start()
		{
			m_EnemyState = mFollowTargets.Length == 0 ? EnemyState.Stand : EnemyState.Patrol;
		}

		private void OnEnable()
		{
			GameEntry.Event.Subscribe(ChangeGameStateEventArgs.EventId,ChangeGameState);
		}

		// Update is called once per frame
		void Update()
		{
			if (m_GameState != GameState.Game)
				return;
			switch (m_EnemyState)
			{
				case EnemyState.Undefined:
					break;
				case EnemyState.Stand:
					break;
				case EnemyState.Patrol:
					Follow();
					break;
				case EnemyState.Follow:
					
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			
		}

		private void OnDisable()
		{
			GameEntry.Event.Unsubscribe(ChangeGameStateEventArgs.EventId,ChangeGameState);
		}

		private void Follow()
        {
			UniformMotion();
			ChangeFollowTarget();
        }

		private void UniformMotion()
		{
			if (!m_IsFollow)
			{
				m_StartTime = Time.unscaledTime;
				m_StartPos = mEnemyModel.transform.position;
				m_IsFollow = true;
			}
			//mEnemyModel.transform.LookAt(mFollowTargets[m_CurrentTarget].transform.position);
            if (m_StartPos == Vector3.zero)
            {
				mEnemyModel.transform.position = Vector3.Lerp(m_StartPos,
				mFollowTargets[m_CurrentTarget].transform.position, (Time.unscaledTime - m_StartTime) * mSpeed);
			}
            else
            {
				mEnemyModel.transform.position = Vector3.Lerp(m_StartPos,
				mFollowTargets[m_CurrentTarget].transform.position, (Time.unscaledTime - m_StartTime) * mSpeed /
				Mathf.Abs(Vector3.Distance(m_StartPos, mFollowTargets[m_CurrentTarget].transform.position)));
			}
		}

		private void ChangeFollowTarget()
		{
			if (!(Vector3.Distance(mEnemyModel.transform.position,
				mFollowTargets[m_CurrentTarget].transform.position) <= 0.1f))
				return;
			m_IsFollow = false;
			if (m_CurrentTarget == mFollowTargets.Length - 1)
			{
				m_CurrentTarget = 0;
				return;
			}
			m_CurrentTarget += 1;
		}

		private void OnTriggerEnter2D(Collider2D col)
		{
			if (col.gameObject.name != "Player")
				return;
			Debug.Log(1);
			GameEntry.Event.Fire(this,ChangeGameStateEventArgs.Create(GameState.GameFailed));
		}
		
		private void ChangeGameState(object sender,GameEventArgs e)
		{
			ChangeGameStateEventArgs ne = (ChangeGameStateEventArgs)e;
			m_GameState = ne.GameState;
		}
	}
}

