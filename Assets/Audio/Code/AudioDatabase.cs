using UnityEngine;

namespace AudioSystems
{
	[CreateAssetMenu(fileName = "Audio Clips Database", menuName = "Audio/New Audio Database")]
	public class AudioDatabase : ScriptableObject
	{
		[Header("Levels")]
		[SerializeField]
		private AudioClip m_DefaultMainMenuAudio = default;

		[SerializeField]
		private AudioClip m_DefaultLevelsAudio = default;

		[SerializeField]
		private AudioClip[] m_ScenesAudioClips = default;

		[Header("Player VFX")]
		[SerializeField]
		private AudioClip m_PlayerExplodeVFX = default;
		public AudioClip PlayerExplodeVFX { get { return m_PlayerExplodeVFX; } }

		[SerializeField]
		private AudioClip m_PlayerJumpSound = default;
		public AudioClip PlayerJumpSound { get { return m_PlayerJumpSound; } }

		[SerializeField]
		private AudioClip m_PlayerLandSound = default;
		public AudioClip PlayerLandSound { get { return m_PlayerLandSound; } }

		[SerializeField]
		private AudioClip m_PlayerGhostDeath = default;
		public AudioClip PlayerGhostDeath { get { return m_PlayerGhostDeath; } }

		[SerializeField]
		private AudioClip m_PlayerHeavenlyGoal = default;
		public AudioClip PlayerHeavenlyGoal { get { return m_PlayerHeavenlyGoal; } }

		[SerializeField]
		private AudioClip[] m_WetSplatVFX = default;
		public int GetNumberOfWetSplatVFX() { return m_WetSplatVFX.Length; }
		public AudioClip GetWetSplatVFX(int index) { return m_WetSplatVFX[index];}

		public AudioClip GetBackgroundAudioForScene(int aSceneBuildIndex)
		{
			if (aSceneBuildIndex >= 0 && aSceneBuildIndex < m_ScenesAudioClips.Length && m_ScenesAudioClips[aSceneBuildIndex] != null)
				return m_ScenesAudioClips[aSceneBuildIndex];

			return aSceneBuildIndex == 0 ? m_DefaultMainMenuAudio : m_DefaultLevelsAudio;
		}
	}
}