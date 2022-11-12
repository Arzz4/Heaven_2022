using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public class DebugRuntimeData
	{
		public int frameCounter = 0;
	}

	[System.Serializable]
	public class PlayerDefaultSettings
	{
		public PlayerInputBindings defaultInputBindings = default;
		public PlayerCollisionSettings defaultCollisionSettings = default;
		public PhysicsContext_MainMotionSettings defaultMainMotionSettings = default;
		public PhysicsContext_JumpMotionSettings defaultJumpSettings = default;
	}

	public class PlayerRuntimeData : MonoBehaviour
	{
		[SerializeField]
		private PlayerDefaultSettings m_DefaultSettings = default;

		public DebugRuntimeData DebugRuntimeData { get; } = new DebugRuntimeData();
		public PlayerInputRuntimeData PlayerInputRuntimeData { get; } = new PlayerInputRuntimeData();
		public PlayerPhysicsRuntimeData PlayerPhysicsRuntimeData { get; } = new PlayerPhysicsRuntimeData();
		public PlayerOrientationRuntimeData PlayerOrientationRuntimeData { get; } = new PlayerOrientationRuntimeData();
		public PlayerCollisionRuntimeData PlayerCollisionRuntimeData { get; } = new PlayerCollisionRuntimeData();
		public PlayerUnityComponentsRuntimeData PlayerUnityComponentsRuntimeData { get; } = new PlayerUnityComponentsRuntimeData();
		public PhysicsContextMainRuntimeData PhysicsContextMainRuntimeData { get; } = new PhysicsContextMainRuntimeData();

		public void Initialize()
		{
			DebugRuntimeData.frameCounter = 0;

			PlayerInputRuntimeData.inputBindings = m_DefaultSettings.defaultInputBindings;
			PlayerCollisionRuntimeData.settings = m_DefaultSettings.defaultCollisionSettings;
			PhysicsContextMainRuntimeData.mainSettings = m_DefaultSettings.defaultMainMotionSettings;
			PhysicsContextMainRuntimeData.jumpSettings = m_DefaultSettings.defaultJumpSettings;
		}

		public void OnFrameLateUpdate()
		{
			DebugRuntimeData.frameCounter++;
		}
	}
}