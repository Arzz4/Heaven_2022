using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public class Surface_Bounce : Surface_Base
	{
		public void OnPlayerTouchesSurface(PlayerRuntimeData aPlayer)
		{
			if (!ValidateSurfaceAbilityActivation(aPlayer))
				return;

			aPlayer.PhysicsContextMainRuntimeData.queuedJump = true;
		}
	}
}