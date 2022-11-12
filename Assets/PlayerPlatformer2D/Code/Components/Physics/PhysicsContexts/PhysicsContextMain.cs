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

		public struct AirAccelerationValues
		{
			public float airAccelerationForInput;
			public float airAccelerationNoInput;
			public float maxSpeedMultiplier;
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
			SetGravityMultiplier(data.jumpSettings.HighJump.GravityMultiplierDown);

			// air acceleration
			data.currentAirAccelerationValues.airAccelerationForInput = data.mainSettings.AirAcelerationMultiplierForInput;
			data.currentAirAccelerationValues.airAccelerationNoInput = data.mainSettings.AirAcelerationMultiplierNoInput;
			data.currentAirAccelerationValues.maxSpeedMultiplier = 1.0f;
		}

		public override void EndContext()
		{
			var data = m_RuntimeData.PhysicsContextMainRuntimeData;
			data.jumpState = PhysicsContextMainRuntimeData.JumpState.None;
		}

		public override void UpdateContext()
		{
			var frameInput = m_RuntimeData.PlayerInputRuntimeData.frameInput;
			var collisionData = m_RuntimeData.PlayerCollisionRuntimeData;

			// start jump
			if (collisionData.onGround && frameInput.buttonPress[(int)ButtonInputType.Jump])
				Jump();

			// update jump state
			UpdateJumpState();
		}

		public override void PostUpdateContext()
		{
			var orientationData = m_RuntimeData.PlayerOrientationRuntimeData;
			var rigidbody = m_RuntimeData.PlayerUnityComponentsRuntimeData.rigidBody;

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

			// 2d movement
			Vector2 aJoystickInput = frameInput.leftJoystickData.rawInput;
			float joystickX_Remapped = System.Math.Sign(aJoystickInput.x) * mainMotion.HorizontalMotionInputRemapping.Evaluate(Mathf.Abs(aJoystickInput.x));
			float currentSpeedX = rigidbody.velocity.x;
			float targetSpeedX = joystickX_Remapped * mainMotion.MaxSpeed * data.currentAirAccelerationValues.maxSpeedMultiplier;
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

			// cap to max falling speed 
			if (!collisionData.onGround)
				rigidbody.velocity = new Vector2(rigidbody.velocity.x, Mathf.Max(-mainMotion.MaxFallSpeed, rigidbody.velocity.y));
		}

		#region JUMP

		private void Jump()
		{
			var data = m_RuntimeData.PhysicsContextMainRuntimeData;
			var jumpSettings = data.jumpSettings;
			var rigidbody = m_RuntimeData.PlayerUnityComponentsRuntimeData.rigidBody;

			float maxSpeed = data.mainSettings.MaxSpeed;

			rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSettings.HighJump.YVelocityMultiplierUp * maxSpeed);
			SetGravityMultiplier(jumpSettings.HighJump.GravityMultiplierUp);


			data.jumpState = PhysicsContextMainRuntimeData.JumpState.TakingOff;
			data.lowJump = false;
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

			// update gravity multiplier according to hold jump (high/low)
			if (data.jumpState == PhysicsContextMainRuntimeData.JumpState.OnAir)
			{
				if (rigidbody.velocity.y > 0)
				{
					var frameInput = m_RuntimeData.PlayerInputRuntimeData.frameInput;
					if (!frameInput.buttonHoldRaw[(int)ButtonInputType.Jump] && !data.lowJump)
					{
						data.lowJump = true;
						rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSettings.LowJump.YVelocityMultiplierUp * data.mainSettings.MaxSpeed);
						SetGravityMultiplier(jumpSettings.LowJump.GravityMultiplierUp);
					}
				}
				else if (rigidbody.velocity.y < 0)
				{
					float gravityMultiplierDown = data.lowJump ? jumpSettings.LowJump.GravityMultiplierDown : jumpSettings.HighJump.GravityMultiplierDown;
					SetGravityMultiplier(gravityMultiplierDown);
				}
			}
		}

		private void SetGravityMultiplier(float gravityMultiplier)
		{
			var data = m_RuntimeData.PhysicsContextMainRuntimeData;
			var rigidbody = m_RuntimeData.PlayerUnityComponentsRuntimeData.rigidBody;

			float maxSpeed = data.mainSettings.MaxSpeed;
			float maxSpeedSqr = maxSpeed * maxSpeed;

			rigidbody.gravityScale = -(gravityMultiplier * maxSpeedSqr) / Physics2D.gravity.y;
		}

		#endregion
	}
}