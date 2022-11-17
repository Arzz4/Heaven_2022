using UnityEngine;

namespace PlayerPlatformer2D
{
	public class PlayerDeathRuntimeData
	{
		public enum State
		{
			Alive,
			Kill,
			KillAll
		}

		public State state = State.Alive;
	}

	public class PlayerDeath : MonoBehaviour
	{
		[SerializeField]
		private PlayerRuntimeData m_RuntimeData = default;

		[SerializeField]
		private GameObject m_DeathPrefab = default; // TODO: move to PlayerUnityComponents, rename that to PlayerComponentsAndAssetReferences

		public void Initialize()
		{
			m_RuntimeData.PlayerUnityComponentsRuntimeData.deathPrefab = m_DeathPrefab;
			m_RuntimeData.PlayerDeathRuntimeData.state = PlayerDeathRuntimeData.State.Alive;
		}

		public bool UpdateDeathBehaviour()
		{
			var data = m_RuntimeData.PlayerDeathRuntimeData;
			var frameInput = m_RuntimeData.PlayerInputRuntimeData.frameInput;
			if (frameInput.buttonPress[(int)GameActionSingleInputType.KillCharacter])
			{
				data.state = PlayerDeathRuntimeData.State.Kill;
			}

			if (frameInput.buttonPress[(int)GameActionMultipleInputType.KillAllCharacters])
			{
				data.state = PlayerDeathRuntimeData.State.KillAll;
			}

			return data.state != PlayerDeathRuntimeData.State.Alive;
		}
	}
}