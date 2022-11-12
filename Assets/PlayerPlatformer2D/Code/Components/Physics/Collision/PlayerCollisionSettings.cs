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

		[SerializeField]
		private float m_WallCollisionRadius = 0.5f;
		public float WallCollisionRadius { get { return m_WallCollisionRadius; } }

		[SerializeField]
		private Vector2 m_BottomOffset = -0.25f * Vector2.down;
		public Vector2 BottomOffset { get { return m_BottomOffset; } }

		[SerializeField]
		private Vector2 m_RightOffset = 0.25f * Vector2.right;
		public Vector2 RightOffset { get { return m_RightOffset; } }

		[SerializeField]
		private Vector2 m_LeftOffset = 0.25f * Vector2.left;
		public Vector2 LeftOffset { get { return m_LeftOffset; } }
	}
}