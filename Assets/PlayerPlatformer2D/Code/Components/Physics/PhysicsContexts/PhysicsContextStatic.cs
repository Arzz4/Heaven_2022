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
			var rigidbody = m_RuntimeData.PlayerUnityComponentsRuntimeData.rigidBody;
			rigidbody.velocity = Vector2.zero;
		}

		public override void FixedUpdateContext()
		{
			var rigidbody = m_RuntimeData.PlayerUnityComponentsRuntimeData.rigidBody;
			rigidbody.velocity = new Vector2(0.0f, rigidbody.velocity.y);
		}
	}
}