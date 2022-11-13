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