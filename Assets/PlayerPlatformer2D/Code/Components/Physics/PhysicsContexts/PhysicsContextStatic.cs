using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public class PhysicsContextStatic : PhysicsContextBase
	{
		[SerializeField]
		private PlayerRuntimeData m_RuntimeData = default;

		public override void Initialize()
		{
			m_ContextType = PhysicsContextType.Static;
		}

		public override void StartContext()
		{
			var data = m_RuntimeData.PhysicsContextMainRuntimeData;
			var rigidbody = m_RuntimeData.PlayerUnityComponentsRuntimeData.rigidBody;

			float maxSpeed = data.jumpSettings.OverwriteMaxHorizontalSpeed ? data.jumpSettings.MaxHorizontalSpeed : data.mainSettings.MaxSpeed;
			float maxSpeedSqr = maxSpeed * maxSpeed;

			rigidbody.gravityScale = -(data.jumpSettings.HighJump.GravityMultiplierDown * maxSpeedSqr) / Physics2D.gravity.y;
		}

		public override void FixedUpdateContext()
		{
			var rigidbody = m_RuntimeData.PlayerUnityComponentsRuntimeData.rigidBody;

			if (m_RuntimeData.PlayerCollisionRuntimeData.onGround)
				rigidbody.drag = 1000.0f;
			else
				rigidbody.drag = 0.0f;
		}
	}
}