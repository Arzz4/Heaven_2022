using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{

	[CreateAssetMenu(fileName = "Main Motion Settings", menuName = "Player Platformer 2D/Physics/Contexts/Main Motion", order = 1)]
	public class PhysicsContext_MainMotionSettings : PhysicsContext_BaseSettings
	{
		[Header("General Settings")]
		[SerializeField, Min(0.0f)]
		private float m_DefaultLinearDrag = 0.0f;
		public float DefaultLinearDrag { get { return m_DefaultLinearDrag; } }

		[SerializeField, Min(0.0f)]
		private float m_MaxSpeed = 10;
		public float MaxSpeed { get { return m_MaxSpeed; } }

		[SerializeField, Min(0.0f)]
		private AnimationCurve m_HorizontalMotionInputRemapping = AnimationCurve.EaseInOut(0, 0, 1, 1);
		public AnimationCurve HorizontalMotionInputRemapping { get { return m_HorizontalMotionInputRemapping; } }

		[SerializeField, Min(0.0f)]
		private float m_Acceleration = 112.0f;
		public float Acceleration { get { return m_Acceleration; } }

		[SerializeField, Min(0.0f)]
		private float m_Deceleration = 45.0f;
		public float Deceleration { get { return m_Deceleration; } }

		[Header("Air Accel Settings")]

		[SerializeField, Min(0.0f)]
		private float m_AirAcelerationMultiplierForInput = 1.0f;
		public float AirAcelerationMultiplierForInput { get { return m_AirAcelerationMultiplierForInput; } }

		[SerializeField, Min(0.0f)]
		private float m_AirAcelerationMultiplierNoInput = 0.25f;
		public float AirAcelerationMultiplierNoInput { get { return m_AirAcelerationMultiplierNoInput; } }

		[Header("Fall Settings")]
		[SerializeField, Min(0.0f)]
		private float m_FallMultiplier = 2.5f;
		public float FallMultiplier { get { return m_FallMultiplier; } }

		[SerializeField, Min(0.0f)]
		private float m_MaxFallSpeed = 10.0f;
		public float MaxFallSpeed { get { return m_MaxFallSpeed; } }

		public override PhysicsContextSettingsType GetSettingsType()
		{
			return PhysicsContextSettingsType.Motion;
		}
	}
}