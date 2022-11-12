using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public class Surface_Bounce : MonoBehaviour
	{
		public void OnPlayerTouchesSurface(PlayerRuntimeData aPlayer)
		{
			aPlayer.PhysicsContextMainRuntimeData.queuedJump = true;
		}
	}
}