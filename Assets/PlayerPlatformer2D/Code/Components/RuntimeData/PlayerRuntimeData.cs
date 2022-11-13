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

	[System.Serializable]
	public class PlayerSurfaceSettings
	{
		public PhysicsContext_BaseSettings[] settingsToApplyOnStickySurface = default;
		public PhysicsContext_BaseSettings[] settingsToApplyOnSpeedySurface = default;
	}

	public class PlayerRuntimeData : MonoBehaviour
	{
		[SerializeField]
		private PlayerDefaultSettings m_DefaultSettings = default;

		[SerializeField]
		private PlayerSurfaceSettings m_SurfaceSettings = default; 

		public DebugRuntimeData DebugRuntimeData { get; } = new DebugRuntimeData();
		public PlayerInputRuntimeData PlayerInputRuntimeData { get; } = new PlayerInputRuntimeData();
		public PlayerPhysicsRuntimeData PlayerPhysicsRuntimeData { get; } = new PlayerPhysicsRuntimeData();
		public PlayerOrientationRuntimeData PlayerOrientationRuntimeData { get; } = new PlayerOrientationRuntimeData();
		public PlayerCollisionRuntimeData PlayerCollisionRuntimeData { get; } = new PlayerCollisionRuntimeData();
		public PlayerUnityComponentsRuntimeData PlayerUnityComponentsRuntimeData { get; } = new PlayerUnityComponentsRuntimeData();
		public PhysicsContextMainRuntimeData PhysicsContextMainRuntimeData { get; } = new PhysicsContextMainRuntimeData();
		public PlayerAudioRuntimeData PlayerAudioRuntimeData { get; } = new PlayerAudioRuntimeData();

		public void Initialize()
		{
			DebugRuntimeData.frameCounter = 0;

			PlayerInputRuntimeData.inputBindings = m_DefaultSettings.defaultInputBindings;
			PlayerCollisionRuntimeData.settings = m_DefaultSettings.defaultCollisionSettings;

			ResetToDefaultPhysicsContextsSettings();
		}

		public void OnFrameLateUpdate()
		{
			DebugRuntimeData.frameCounter++;
		}

		public void ResetToDefaultPhysicsContextsSettings()
		{
			PhysicsContextMainRuntimeData.mainSettings = m_DefaultSettings.defaultMainMotionSettings;
			PhysicsContextMainRuntimeData.jumpSettings = m_DefaultSettings.defaultJumpSettings;
		}

		public void ApplyStickySurfaceSettings()
		{
			var settingsToApply = m_SurfaceSettings.settingsToApplyOnStickySurface;

			for (int i = 0; i < settingsToApply.Length; ++i)
			{
				var settings = settingsToApply[i];
				switch (settings.GetSettingsType())
				{
					case PhysicsContextSettingsType.Motion:
						PhysicsContextMainRuntimeData.mainSettings = settings as PhysicsContext_MainMotionSettings;
						break;
					case PhysicsContextSettingsType.Jump:
						PhysicsContextMainRuntimeData.jumpSettings = settings as PhysicsContext_JumpMotionSettings;
						break;
				}
			}
		}

		public void ApplySpeedySurfaceSettings()
		{
			var settingsToApply = m_SurfaceSettings.settingsToApplyOnStickySurface;

			for (int i = 0; i < settingsToApply.Length; ++i)
			{
				var settings = settingsToApply[i];
				switch (settings.GetSettingsType())
				{
					case PhysicsContextSettingsType.Motion:
						PhysicsContextMainRuntimeData.mainSettings = settings as PhysicsContext_MainMotionSettings;
						break;
					case PhysicsContextSettingsType.Jump:
						PhysicsContextMainRuntimeData.jumpSettings = settings as PhysicsContext_JumpMotionSettings;
						break;
				}
			}
		}
	}
}