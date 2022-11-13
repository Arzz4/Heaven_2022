using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public class PlayerDeath : MonoBehaviour
	{
		[SerializeField]
		private PlayerRuntimeData m_RuntimeData = default;

		[SerializeField]
		private GameObject m_DeathPrefab = default;

		public bool UpdateDeathBehaviour()
		{
			var frameInput = m_RuntimeData.PlayerInputRuntimeData.frameInput;
			if (!frameInput.buttonPress[(int)ButtonInputType.KillCharacter])
				return false;

			var obj = GameObject.Instantiate(m_DeathPrefab, transform.position, Quaternion.identity);
			var bounce = obj.GetComponent<Surface_Base>();
			if (bounce != null)
			{
				bounce.setSourceVelocity(m_RuntimeData.PlayerPhysicsRuntimeData.velocity);
			}
			return true;
		}
	}
}