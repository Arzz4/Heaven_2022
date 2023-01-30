using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayUtility
{
	public class ObjectLifetime : MonoBehaviour
	{
		[SerializeField]
		private float m_LifeTime = 1.0f;
		public float LifeTime { get { return m_LifeTime; } }

		private float m_StartTimestamp = 0.0f;

		private void OnEnable()
		{
			m_StartTimestamp = Time.time;
		}

		private void Update()
		{
			if (Time.time - m_StartTimestamp > m_LifeTime)
				ObjectPoolSystem.Instance.RecycleInstance(gameObject);
		}
	}
}