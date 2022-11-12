using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	[System.Serializable]
	public class JumpDefinition
	{
		[SerializeField]
		private float m_JumpHeight = 1.0f;

		[SerializeField]
		private float m_JumpHorizontalRangeUp = 1.0f;

		[SerializeField]
		private float m_JumpHorizontalRangeDown = 1.0f;

		public float GravityMultiplierUp { get { return GetGravityMultiplier(m_JumpHorizontalRangeUp * 0.5f); } }
		public float GravityMultiplierDown { get { return GetGravityMultiplier(m_JumpHorizontalRangeDown * 0.5f); } }
		public float YVelocityMultiplierUp { get { return GetYVelocityMultiplier(m_JumpHorizontalRangeUp * 0.5f); } }
		public float YVelocityMultiplierDown { get { return GetYVelocityMultiplier(m_JumpHorizontalRangeDown * 0.5f); } }

		private float GetGravityMultiplier(float halfRangeX)
		{
			float halfXRangeUpSqr = halfRangeX * halfRangeX;
			return 2.0f * m_JumpHeight / halfXRangeUpSqr;
		}

		private float GetYVelocityMultiplier(float halfRangeX)
		{
			return 2.0f * m_JumpHeight / halfRangeX;
		}
	}

	[CreateAssetMenu(fileName = "Jump Settings", menuName = "Player Platformer 2D/Physics/Contexts/Jump", order = 1)]
	public class PhysicsContext_JumpMotionSettings : PhysicsContext_BaseSettings
	{
		[SerializeField, Tooltip("when not overwriting the max speed, it means we will use the max speed the player has on the current motion settings for the jump physics")]
		private bool m_OverwriteMaxHorizontalSpeed = false;
		public bool OverwriteMaxHorizontalSpeed { get { return m_OverwriteMaxHorizontalSpeed; } }

		[ShowIf("OverwriteMaxHorizontalSpeed")] 
		[AllowNesting]
		[SerializeField]
		private float m_MaxHorizontalSpeed = 10.0f;
		public float MaxHorizontalSpeed { get { return m_MaxHorizontalSpeed; } }

		[SerializeField]
		private bool m_LowJumpDifferentFromHighJump = true;
		public bool LowJumpDifferentFromHighJump { get { return m_LowJumpDifferentFromHighJump; } }

		[SerializeField]
		private JumpDefinition m_HighJump = default;
		public JumpDefinition HighJump { get { return m_HighJump; } }

		[ShowIf("LowJumpDifferentFromHighJump")]
		[AllowNesting]
		[SerializeField]
		private JumpDefinition m_LowJump = default;
		public JumpDefinition LowJump { get { return LowJumpDifferentFromHighJump ? m_LowJump : m_HighJump; } }

		public override PhysicsContextSettingsType GetSettingsType()
		{
			return PhysicsContextSettingsType.Jump;
		}
	}
}