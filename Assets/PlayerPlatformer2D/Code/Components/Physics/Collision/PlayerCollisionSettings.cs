using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	[CreateAssetMenu(fileName = "Collision Settings", menuName = "Player Platformer 2D/Physics/Collision Settings")]
	public class PlayerCollisionSettings : ScriptableObject
	{
		[Header("Layers")]
		[SerializeField]
		private LayerMask m_GroundLayer = default;
		public LayerMask GroundLayer { get { return m_GroundLayer; } }

		[Header("Settings")]
		[SerializeField]
		private float m_GroundCollisionRadius = 0.5f;
		public float GroundCollisionRadius { get { return m_GroundCollisionRadius; } }

		[Header("Ground Detection")]
		[SerializeField]
		private float m_GroundRaycastCenterOffsetX = 0.1f;
		public Vector2 GroundRaycastCenterOffset { get { return new Vector2(m_GroundRaycastCenterOffsetX, 0.0f); } }

		[SerializeField]
		private float m_GroundRaycastDistance = 0.5f;
		public float GroundRaycastDistance { get { return m_GroundRaycastDistance; } }

		[SerializeField]
		private float m_GroundPlanarSize = 1.0f;
		public float GroundPlanarSize { get { return m_GroundPlanarSize; } }

		[Header("Wall Detection")]
		[SerializeField]
		private float m_WallCollisionRadius = 0.5f;
		public float WallCollisionRadius { get { return m_WallCollisionRadius; } }

		[SerializeField]
		private Vector2 m_RightOffset = 0.25f * Vector2.right;
		public Vector2 RightOffset { get { return m_RightOffset; } }

		[SerializeField]
		private Vector2 m_LeftOffset = 0.25f * Vector2.left;
		public Vector2 LeftOffset { get { return m_LeftOffset; } }
	}
}