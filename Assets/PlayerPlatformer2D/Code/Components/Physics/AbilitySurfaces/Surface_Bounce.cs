using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public class Surface_Bounce : Surface_Base
	{
		public float gravity = 30;
        private Rigidbody2D body;
		private Animator animator;
		private bool idle;

		private void Start()
		{
			body = GetComponent<Rigidbody2D>();
			body.velocity = sourceVelocity;
			animator = GetComponent<Animator>();
			idle = false;	
		}

		void Update()
		{
			if(!idle && Mathf.Abs(body.velocity.y) < 0.01f)
			{
                animator.Play("Bouncer_Platform_Idle");
				idle = true;	
            }
        }

		public void OnPlayerTouchesSurface(PlayerRuntimeData aPlayer)
		{
			if (!ValidateSurfaceAbilityActivation(aPlayer))
				return;

			aPlayer.PhysicsContextMainRuntimeData.queuedJump = true;
            animator.Play("Bouncer_Platform_Bounce");
        }
	}

	
}