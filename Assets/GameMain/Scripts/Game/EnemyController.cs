using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using GameMain;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CourseMain
{
	public class EnemyController : MonoBehaviour
	{
		public enum EnemyState
		{
			Undefined,
			Stand,
			Patrol,
			Follow,
			FollowPlayer
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
		
		private bool m_ShowSound = false;

		private GameObject[] m_SoundPosts = null;
		
		private Vector3 m_Distance = Vector3.zero;
		private bool m_IsPlayMoan = false;
		private int? m_MoanSound = null;
		// Use this for initialization
		void Start()
		{
			m_EnemyState = mFollowTargets.Length == 0 ? EnemyState.Stand : EnemyState.Patrol;
		}

		private void OnEnable()
		{
			GameEntry.Event.Subscribe(ChangeGameStateEventArgs.EventId,ChangeGameState);
			GameEntry.Event.Subscribe(SendPlayerPositionEventArgs.EventId,ReceivePos);
			GameEntry.Event.Subscribe(ChangeEnemyStateEventArgs.EventId,ChangeEnemyState);
			GameEntry.Event.Subscribe(GameResetEventArgs.EventId,GameReset);
			m_SoundPosts = new GameObject[transform.GetChild(0).childCount];
			for (int i = 0; i < transform.GetChild(0).childCount; i++)
			{
				m_SoundPosts[i] = transform.GetChild(0).GetChild(i).gameObject;
			}
			ResetSoundPosts();
			m_MoanSound = new int();
			m_IsPlayMoan = false;
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
				case EnemyState.FollowPlayer:
					transform.position = Vector3.Lerp(transform.position, m_Distance, 0.1f);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			if (m_ShowSound)
			{
				RandomSoundPosts();
				if (!m_IsPlayMoan)
				{
					var randomNum = Random.Range(0, 6);
					m_MoanSound = GameEntry.Sound.PlaySound(10012 + randomNum);
					m_IsPlayMoan = true;
				}
			}
			else
			{
				ResetSoundPosts();
				if (m_IsPlayMoan)
				{
					// if (m_MoanSound != null) 
					// 	GameEntry.Sound.StopSound((int)m_MoanSound);
					m_IsPlayMoan = false;
				}
			}
			
		}

		private void OnDisable()
		{
			if (m_MoanSound != null) 
				GameEntry.Sound.StopSound(m_MoanSound.Value);
			GameEntry.Event.Unsubscribe(SendPlayerPositionEventArgs.EventId,ReceivePos);
			GameEntry.Event.Unsubscribe(ChangeGameStateEventArgs.EventId,ChangeGameState);
			GameEntry.Event.Unsubscribe(ChangeEnemyStateEventArgs.EventId,ChangeEnemyState);
			GameEntry.Event.Unsubscribe(GameResetEventArgs.EventId,GameReset);
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
	            var distance =
		            Mathf.Abs(Vector3.Distance(m_StartPos, mFollowTargets[m_CurrentTarget].transform.position));
	            if (distance == 0)
	            {
		            distance = 0.0001f;
	            }
				mEnemyModel.transform.position = Vector3.Lerp(m_StartPos,
				mFollowTargets[m_CurrentTarget].transform.position, (Time.unscaledTime - m_StartTime) * mSpeed /
				distance);
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
			if (m_EnemyState == EnemyState.Undefined)
				return;
			if (col.gameObject.name == "Player")
			{
				GameEntry.Event.Fire(this,OnEnemyTriggerEventArgs.Create(gameObject));
			}
			else if (col.gameObject.name == "AttackSoundPosts")
			{
				var randomNum = Random.Range(0, 2);
				GameEntry.Sound.PlaySound(10005 + randomNum);
				transform.gameObject.SetActive(false);
			}

		}
		
		private void ChangeGameState(object sender,GameEventArgs e)
		{
			ChangeGameStateEventArgs ne = (ChangeGameStateEventArgs)e;
			m_GameState = ne.GameState;
			switch (m_GameState)
			{
				case GameState.GameFailed:
					m_EnemyState = EnemyState.Undefined;
					ResetSoundPosts();
					break;
				case GameState.Game:
					m_EnemyState = mFollowTargets.Length == 0 ? EnemyState.Stand : EnemyState.Patrol;
					break;
			}
		}

		private void ReceivePos(object sender, GameEventArgs e)
		{
			SendPlayerPositionEventArgs ne = (SendPlayerPositionEventArgs)e;
			m_Distance = ne.Position;
			var distance = Vector2.Distance(transform.position, ne.Position);
			//Debug.Log(distance);
			if (distance < 5)
			{
				m_ShowSound = true;
			}
			else
			{
				m_ShowSound = false;
			}
		}

		private void GameReset(object sender, GameEventArgs e)
		{
			
		}

		private void ChangeEnemyState(object sender, GameEventArgs e)
		{
			ChangeEnemyStateEventArgs ne = (ChangeEnemyStateEventArgs)e;
			m_EnemyState = ne.EnemyState;
		}
		
		private void RandomSoundPosts()
		{
			for (int i = 0; i < m_SoundPosts.Length; i++)
			{
				var scaleY = Random.Range(0.1f, 2f);
				var randomVector = new Vector3(1.0f, scaleY,
					m_SoundPosts[i].transform.localScale.z);
				m_SoundPosts[i].transform.localScale =
					Vector3.Lerp(m_SoundPosts[i].transform.localScale, randomVector, 0.9f);
			}
		}
		
		private void ResetSoundPosts()
		{
			for (int i = 0; i < m_SoundPosts.Length; i++)
			{
				m_SoundPosts[i].transform.localScale = Vector3.zero;
			}
		}
	}
}

