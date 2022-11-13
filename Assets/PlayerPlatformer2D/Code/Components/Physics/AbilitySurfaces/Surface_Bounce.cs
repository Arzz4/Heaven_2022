using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public class Surface_Bounce : Surface_Base
	{
		private bool stop;
		public float gravity = 30;
        public Rigidbody2D body;

		private void Start()
		{
			body = GetComponent<Rigidbody2D>();
			body.velocity = sourceVelocity;
		}

		private void FixedUpdate()
		{
            if (!stop)
            {
                Vector2 velocity = body.velocity;
                velocity.y -= gravity * Time.deltaTime;
				body.velocity = velocity;
            }
        }

		public void OnPlayerTouchesSurface(PlayerRuntimeData aPlayer)
		{
			if (!ValidateSurfaceAbilityActivation(aPlayer))
				return;

			aPlayer.PhysicsContextMainRuntimeData.queuedJump = true;
		}
	}

	
}