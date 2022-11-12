using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public class PlayerAnimator : MonoBehaviour
	{
		[SerializeField]
		private PlayerRuntimeData m_RuntimeData;

		public void PostUpdateAnimations()
		{
			var collisionData = m_RuntimeData.PlayerCollisionRuntimeData;
			var physicsData = m_RuntimeData.PlayerPhysicsRuntimeData;
			var animator = m_RuntimeData.PlayerUnityComponentsRuntimeData.animator;

			animator.SetBool("inAir", !collisionData.onGround);
			animator.SetBool("isSidewaysMoving", Mathf.Abs(physicsData.velocity.x) > 0.01f);
			animator.SetBool("isOnStickyWall", collisionData.onWall);
			animator.SetFloat("verticalSpeed", physicsData.velocity.y);
		}
	}
}