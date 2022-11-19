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
			SetFallGravityMultiplier();
		}

		public override void FixedUpdateContext()
		{
			var collisionData = m_RuntimeData.PlayerCollisionRuntimeData;
			var rigidbody = m_RuntimeData.PlayerUnityComponentsRuntimeData.rigidBody;

			if (collisionData.onGround)
				rigidbody.drag = 1000.0f;
			else
				rigidbody.drag = 0.0f;


			if (collisionData.onEdge)
			{
				rigidbody.gravityScale = 0.0f;
				rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0.0f);
			}
			else if (!collisionData.onStickySurface)
				SetFallGravityMultiplier();
		}

		private void SetFallGravityMultiplier()
		{
			var data = m_RuntimeData.PhysicsContextMainRuntimeData;
			var rigidbody = m_RuntimeData.PlayerUnityComponentsRuntimeData.rigidBody;

			float maxSpeed = data.jumpSettings.OverwriteMaxHorizontalSpeed ? data.jumpSettings.MaxHorizontalSpeed : data.mainSettings.MaxSpeed;
			float maxSpeedSqr = maxSpeed * maxSpeed;

			rigidbody.gravityScale = -(data.jumpSettings.HighJump.GravityMultiplierDown * maxSpeedSqr) / Physics2D.gravity.y;
		}
	}
}