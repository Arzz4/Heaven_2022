using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public class Surface_ModifyPlayerPhysicsSettings : MonoBehaviour
	{
		[SerializeField]
		private PhysicsContext_BaseSettings[] m_SettingsToApply = default;

		public void OnPlayerTouchesSurface(PlayerRuntimeData aPlayer)
		{
			for (int i = 0; i < m_SettingsToApply.Length; ++i)
			{
				var settings = m_SettingsToApply[i];
				switch (settings.GetSettingsType())
				{
					case PhysicsContextSettingsType.Motion:
						aPlayer.PhysicsContextMainRuntimeData.mainSettings = settings as PhysicsContext_MainMotionSettings;
						break;
					case PhysicsContextSettingsType.Jump:
						aPlayer.PhysicsContextMainRuntimeData.jumpSettings = settings as PhysicsContext_JumpMotionSettings;
						break;
				}
			}
		}
	}
}