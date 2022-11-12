using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public class PlayerUnityComponentsRuntimeData
	{
		public Rigidbody2D rigidBody = default;
		public Collider2D collider = default;
		public Transform physicsTransform = default;
		public Transform visualsTransform = default;
		public Transform physicsContextsRoot = default;
		public PlayerPhysics physicsController = default;
		public Animator animator = default;
	}

	public class PlayerUnityComponents : MonoBehaviour
	{
		[SerializeField]
		private PlayerRuntimeData m_RuntimeData = default;

		[SerializeField]
		private Transform m_PhysicsContextsRoot = default;

		[SerializeField]
		private Transform m_VisualsTransform = default;

		[SerializeField]
		private Rigidbody2D m_RigidBody = default;

		[SerializeField]
		private Collider2D m_Collider = default;

		[SerializeField]
		private Animator m_Animator = default;

		public void Initialize()
		{
			var data = m_RuntimeData.PlayerUnityComponentsRuntimeData;
			data.rigidBody = m_RigidBody;
			data.collider = m_Collider;
			data.animator = m_Animator;
			data.physicsTransform = m_RigidBody.transform;
			data.visualsTransform = m_VisualsTransform;
			data.physicsContextsRoot = m_PhysicsContextsRoot;
			data.physicsController = m_RigidBody.GetComponent<PlayerPhysics>();
		}
	}
}