using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public class Surface_Base : MonoBehaviour
	{
		[SerializeField]
		private bool m_ApplyOnlyWhenPlayerStandsOnThisSurface = false;

		protected bool ValidateSurfaceAbilityActivation(PlayerRuntimeData aPlayer)
		{
			if (m_ApplyOnlyWhenPlayerStandsOnThisSurface)
			{
				if (!aPlayer.PlayerCollisionRuntimeData.onGround || aPlayer.PlayerCollisionRuntimeData.groundObject.GetInstanceID() != gameObject.GetInstanceID())
					return false;
			}

			return true;
		}
	}
}