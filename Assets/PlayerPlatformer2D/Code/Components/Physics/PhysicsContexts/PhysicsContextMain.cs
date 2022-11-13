using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerPlatformer2D.PlayerInputRuntimeData;

namespace PlayerPlatformer2D
{
	public class PhysicsContextMainRuntimeData
	{
		public enum JumpState
		{
			None,
			TakingOff,
			OnAir,
			Landed
		}

		public JumpState jumpState = JumpState.None;
		public bool lowJump = false;
		public bool queuedJump = false;
		public bool onStickySurface = false;

		public struct AirAccelerationValues
		{
			public float airAccelerationForInput;
			public float airAccelerationNoInput;
		}

		public AirAccelerationValues currentAirAccelerationValues;

		public PhysicsContext_MainMotionSettings mainSettings = default;
		public PhysicsContext_JumpMotionSettings jumpSettings = default;
	}

	public class PhysicsContextMain : PhysicsContextBase
	{
		[SerializeField]
		private PlayerRuntimeData m_RuntimeData = default;

		public override void Initialize()
		{
			m_ContextType = PhysicsContextType.Main;
		}

		public override void StartContext()
		{
			var data = m_RuntimeData.PhysicsContextMainRuntimeData;
			var mainMotion = data.mainSettings;
			var rigidbody = m_RuntimeData.PlayerUnityComponentsRuntimeData.rigidBody;

			// physics initialization
			rigidbody.drag = mainMotion.DefaultLinearDrag;
			SetJumpGravityMultiplier(data.jumpSettings.HighJump.GravityMultiplierDown);
		}

		public override void EndContext()
		{
			var data = m_RuntimeData.PhysicsContextMainRuntimeData;
			data.jumpState = PhysicsContextMainRuntimeData.JumpState.None;
		}

		public override void UpdateContext()
		{
			var data = m_RuntimeData.PhysicsContextMainRuntimeData;
			var frameInput = m_RuntimeData.PlayerInputRuntimeData.frameInput;
			var collisionData = m_RuntimeData.PlayerCollisionRuntimeData;

			// air acceleration values (to allow hot reloading of tweaking values at runtime)
			data.currentAirAccelerationValues.airAccelerationForInput = data.mainSettings.AirAcelerationMultiplierForInput;
			data.currentAirAccelerationValues.airAccelerationNoInput = data.mainSettings.AirAcelerationMultiplierNoInput;

			// start jump
			bool canJump = collisionData.onGround || data.onStickySurface;
			bool jumpInput = frameInput.buttonPress[(int)ButtonInputType.Jump];
			float elapsedTimeSinceGroundLeft = Time.time - collisionData.leftGroundTimestamp;
			
			if (canJump && (jumpInput || data.queuedJump))
				Jump();

			else if (jumpInput && (!collisionData.onGround && !collisionData.onWall) && elapsedTimeSinceGroundLeft < 0.1f)
			{
				Jump();
				return; // do not update the jump state in this case, we need at least 1 frame on TakingOff state
			}

			// update jump state
			UpdateJumpState();
		}

		public override void PostUpdateContext()
		{
			var data = m_RuntimeData.PhysicsContextMainRuntimeData;
			var orientationData = m_RuntimeData.PlayerOrientationRuntimeData;
			var rigidbody = m_RuntimeData.PlayerUnityComponentsRuntimeData.rigidBody;
			var collisionData = m_RuntimeData.PlayerCollisionRuntimeData;

			// facing dir towards the other side of the wall
			if (data.onStickySurface && collisionData.onWall)
			{
				orientationData.facingSign = -collisionData.wallSide;
				return;
			}
			else if(data.onStickySurface)
			{
				var leftJoystickX = m_RuntimeData.PlayerInputRuntimeData.frameInput.leftJoystickData.rawInput.x;
				orientationData.facingSign = leftJoystickX > 0 ? 1 : (leftJoystickX < 0.0f ? -1 : orientationData.facingSign);
				return;
			}

			// facing update based on physics movement
			orientationData.facingSign = rigidbody.velocity.x > 0 ? 1 : (rigidbody.velocity.x < 0.0f ? -1 : orientationData.facingSign);
		}

		public override void FixedUpdateContext()
		{
			var data = m_RuntimeData.PhysicsContextMainRuntimeData;
			var mainMotion = data.mainSettings;
			var frameInput = m_RuntimeData.PlayerInputRuntimeData.frameInput;
			var collisionData = m_RuntimeData.PlayerCollisionRuntimeData;
			var rigidbody = m_RuntimeData.PlayerUnityComponentsRuntimeData.rigidBody;
			var jumpSettings = data.jumpSettings;

			// sticky walls
			data.onStickySurface = false;
			bool isJumpStateGoingUp = data.jumpState == PhysicsContextMainRuntimeData.JumpState.TakingOff || (data.jumpState == PhysicsContextMainRuntimeData.JumpState.OnAir && rigidbody.velocity.y > 0);
			bool commonValidationForStickySurfaceAction = !frameInput.buttonPress[(int)ButtonInputType.Jump] && !isJumpStateGoingUp;
			if ((collisionData.onWall || collisionData.onGround) && collisionData.onStickySurface && commonValidationForStickySurfaceAction)
			{
				rigidbody.gravityScale = 0.0f;
				rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0.0f);
				data.onStickySurface = true;
			}

			// update gravity multiplier according to hold jump (high/low)
			if (data.jumpState == PhysicsContextMainRuntimeData.JumpState.OnAir)
			{
				if (rigidbody.velocity.y > 0)
				{
					if (!frameInput.buttonHoldRaw[(int)ButtonInputType.Jump] && !data.lowJump)
					{
						data.lowJump = true;
						SetJumpVerticalVelocity(jumpSettings.LowJump.YVelocityMultiplierUp, true);
						SetJumpGravityMultiplier(jumpSettings.LowJump.GravityMultiplierUp);
					}
				}
				else if (rigidbody.velocity.y < 0)
				{
					float gravityMultiplierDown = data.lowJump ? jumpSettings.LowJump.GravityMultiplierDown : jumpSettings.HighJump.GravityMultiplierDown;
					SetJumpGravityMultiplier(gravityMultiplierDown);
				}
			}

			// 2d movement
			if (data.onStickySurface)
			{
				rigidbody.velocity = Vector2.zero;
			}
			else
			{
				Vector2 aJoystickInput = frameInput.leftJoystickData.rawInput;
				float joystickX_Remapped = System.Math.Sign(aJoystickInput.x) * mainMotion.HorizontalMotionInputRemapping.Evaluate(Mathf.Abs(aJoystickInput.x));
				float currentSpeedX = rigidbody.velocity.x;
				float targetSpeedX = joystickX_Remapped * mainMotion.MaxSpeed;
				float accelerationX = Mathf.Abs(targetSpeedX) > Mathf.Abs(currentSpeedX) ? mainMotion.Acceleration : mainMotion.Deceleration;
				float airAccelerationMultiplier = targetSpeedX != 0.0f && aJoystickInput.x * rigidbody.velocity.x < 0 ? data.currentAirAccelerationValues.airAccelerationForInput : data.currentAirAccelerationValues.airAccelerationNoInput;

				if (collisionData.onGround)
				{
					rigidbody.velocity = new Vector2(Mathf.MoveTowards(currentSpeedX, targetSpeedX, Time.fixedDeltaTime * accelerationX), rigidbody.velocity.y);
				}
				else
				{
					rigidbody.velocity = new Vector2(Mathf.MoveTowards(currentSpeedX, targetSpeedX, Time.fixedDeltaTime * airAccelerationMultiplier * accelerationX), rigidbody.velocity.y);
				}
			}

			// cap to max falling speed 
			if (!collisionData.onGround)
				rigidbody.velocity = new Vector2(rigidbody.velocity.x, Mathf.Max(-mainMotion.MaxFallSpeed, rigidbody.velocity.y));
		}

		#region JUMP

		private void Jump()
		{
			var data = m_RuntimeData.PhysicsContextMainRuntimeData;
			var jumpSettings = data.jumpSettings;
			var collisionData = m_RuntimeData.PlayerCollisionRuntimeData;

			SetJumpVerticalVelocity(jumpSettings.HighJump.YVelocityMultiplierUp);
			SetJumpGravityMultiplier(jumpSettings.HighJump.GravityMultiplierUp);

			// wall jump modifier
			if (collisionData.onWall && data.onStickySurface)
				SetJumpHorizontalVelocity(jumpSettings.HighJump.YVelocityMultiplierUp, collisionData.wallSide);

			data.jumpState = PhysicsContextMainRuntimeData.JumpState.TakingOff;
			data.lowJump = false;
			data.queuedJump = false;
		}

		private void UpdateJumpState()
		{
			var data = m_RuntimeData.PhysicsContextMainRuntimeData;
			var jumpSettings = data.jumpSettings;
			var collisionData = m_RuntimeData.PlayerCollisionRuntimeData;
			var rigidbody = m_RuntimeData.PlayerUnityComponentsRuntimeData.rigidBody;

			if (data.jumpState == PhysicsContextMainRuntimeData.JumpState.None)
				return;

			bool touchingTraversalObjects = collisionData.onGround || collisionData.onWall;
			bool falling = rigidbody.velocity.y < 0;

			if (data.jumpState == PhysicsContextMainRuntimeData.JumpState.TakingOff && (!touchingTraversalObjects || falling))
			{
				data.jumpState = PhysicsContextMainRuntimeData.JumpState.OnAir;
			}
			else if (data.jumpState == PhysicsContextMainRuntimeData.JumpState.OnAir && touchingTraversalObjects)
			{
				data.jumpState = PhysicsContextMainRuntimeData.JumpState.Landed;
			}
			else if (data.jumpState == PhysicsContextMainRuntimeData.JumpState.Landed)
			{
				data.jumpState = PhysicsContextMainRuntimeData.JumpState.None;
			}
		}

		private void SetJumpVerticalVelocity(float verticalVelocityMultiplier, bool capToMinimum = false) 
		{
			var data = m_RuntimeData.PhysicsContextMainRuntimeData;
			var rigidbody = m_RuntimeData.PlayerUnityComponentsRuntimeData.rigidBody;

			float maxSpeed = data.jumpSettings.OverwriteMaxHorizontalSpeed ? data.jumpSettings.MaxHorizontalSpeed : data.mainSettings.MaxSpeed;
			float velocityY = verticalVelocityMultiplier * maxSpeed;
			
			if (capToMinimum)
				velocityY = Mathf.Min(rigidbody.velocity.y, velocityY);

			rigidbody.velocity = new Vector2(rigidbody.velocity.x, velocityY);
		}

		private void SetJumpGravityMultiplier(float gravityMultiplier)
		{
			var data = m_RuntimeData.PhysicsContextMainRuntimeData;
			var rigidbody = m_RuntimeData.PlayerUnityComponentsRuntimeData.rigidBody;

			float maxSpeed = data.jumpSettings.OverwriteMaxHorizontalSpeed ? data.jumpSettings.MaxHorizontalSpeed : data.mainSettings.MaxSpeed;
			float maxSpeedSqr = maxSpeed * maxSpeed;

			rigidbody.gravityScale = -(gravityMultiplier * maxSpeedSqr) / Physics2D.gravity.y;
		}

		private void SetJumpHorizontalVelocity(float verticalVelocityMultiplier, float sign)
		{
			var data = m_RuntimeData.PhysicsContextMainRuntimeData;
			var rigidbody = m_RuntimeData.PlayerUnityComponentsRuntimeData.rigidBody;

			float maxSpeed = data.jumpSettings.OverwriteMaxHorizontalSpeed ? data.jumpSettings.MaxHorizontalSpeed : data.mainSettings.MaxSpeed;
			float velocityX = verticalVelocityMultiplier * maxSpeed;

			rigidbody.velocity = new Vector2(sign * Mathf.Max(Mathf.Abs(rigidbody.velocity.x), velocityX), rigidbody.velocity.y);
		}

		#endregion
	}
}