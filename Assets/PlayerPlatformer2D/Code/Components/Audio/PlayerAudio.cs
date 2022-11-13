using AudioSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public class PlayerAudioRuntimeData
	{

	}

	public class PlayerAudio : MonoBehaviour
	{
		[SerializeField]
		private PlayerRuntimeData m_RuntimeData = default;

		public void PostUpdateAudio()
		{
			var manager = AudioManager.Instance;
			if (manager == null)
				return;

			var db = manager.GetAudioDatabase();
			if (db == null)
				return;

			var collisionData = m_RuntimeData.PlayerCollisionRuntimeData;

			// land
			if (collisionData.justLanded)
				manager.PlayOnShotAudioOnVFXAudioSource(db.PlayerLandSound);
		}
	}
}