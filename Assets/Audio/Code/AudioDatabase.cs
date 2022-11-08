using UnityEngine;

namespace AudioSystems
{
	[CreateAssetMenu(fileName = "Audio Clips Database", menuName = "Audio/New Audio Database")]
	public class AudioDatabase : ScriptableObject
	{
		[SerializeField]
		private AudioClip m_DefaultMainMenuAudio = default;

		[SerializeField]
		private AudioClip m_DefaultLevelsAudio = default;

		[SerializeField]
		private AudioClip[] m_ScenesAudioClips = default;

		[SerializeField]
		private AudioClip m_OnTileClicked = default;
		public AudioClip OnTileClicked { get { return m_OnTileClicked; } }

		[SerializeField]
		private AudioClip m_OnLevelCompleted = default;
		public AudioClip OnLevelCompleted { get { return m_OnLevelCompleted; } }

		public AudioClip GetBackgroundAudioForScene(int aSceneBuildIndex)
		{
			if (aSceneBuildIndex >= 0 && aSceneBuildIndex < m_ScenesAudioClips.Length && m_ScenesAudioClips[aSceneBuildIndex] != null)
				return m_ScenesAudioClips[aSceneBuildIndex];

			return aSceneBuildIndex == 0 ? m_DefaultMainMenuAudio : m_DefaultLevelsAudio;
		}
	}
}