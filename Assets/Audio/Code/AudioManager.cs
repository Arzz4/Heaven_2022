using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AudioSystems
{
	[System.Serializable]
	public class AudioClipDef
	{
		[Tooltip("audio asset")]
		public AudioClip asset = default;

		[Tooltip("adjust for how long to wait before playing the asset above"), Min(0.0f)]
		public float delayToPlaySound = 0.0f;
	}

	public class AudioManager : MonoBehaviour
	{
		private static AudioManager m_Instance;
		public static AudioManager Instance { get { return m_Instance; } }

		[SerializeField]
		private AudioDatabase m_AudioDatabase = default;

		[SerializeField]
		private AudioSource m_MainAudioSource = default;

		[SerializeField]
		private AudioSource m_VFXAudioSource = default;

		[SerializeField]
		private bool m_SoundMuted = true;

		List<AudioSource> m_LevelSources = new List<AudioSource>();

		private void Awake()
		{
			if (m_Instance != null)
			{
				DestroyImmediate(gameObject);
				return;
			}

			m_Instance = this;
			DontDestroyOnLoad(gameObject);

			m_GlobalAudioVolume = 1.0f;
			m_BackgroundAudioVolume = m_MainAudioSource.volume;
			m_VFXAudioSource.volume = m_VFXAudioSource.volume;
		}

		private void OnEnable()
		{
			SceneManager.sceneLoaded += OnSceneLoadCallback;
		}

		private void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoadCallback;
		}

		private void Start()
		{
			OnSceneLoadCallback(SceneManager.GetActiveScene(), LoadSceneMode.Single);
		}

		private void OnSceneLoadCallback(Scene scene, LoadSceneMode sceneMode)
		{
			UpdateLevelSources();
			SetEverythingToMutedState(m_SoundMuted);
			SetGlobalSoundVolume(m_GlobalAudioVolume);
			UpdateLevelAudioSourcesVolumeLevel();

			var backgroundAudio = m_AudioDatabase.GetBackgroundAudioForScene(scene.buildIndex);
			if (m_MainAudioSource.clip != backgroundAudio)
				PlayLoopAudioOnMainAudioSource(backgroundAudio);

			if (!m_MainAudioSource.isPlaying)
				m_MainAudioSource.Play();
		}

		#region VOLUME

		private float m_GlobalAudioVolume = 1.0f; // used as a multiplier
		private float m_BackgroundAudioVolume = 1.0f;
		private float m_VFXAudioVolume = 1.0f;

		public void ToggleSoundState()
		{
			SetSoundMuted(!m_SoundMuted);
		}

		public void SetSoundMuted(bool aMutedFlag)
		{
			m_SoundMuted = aMutedFlag;
			SetEverythingToMutedState(m_SoundMuted);
		}

		public bool IsMuted()
		{
			return m_SoundMuted;
		}

		public void SetGlobalSoundVolume(float aVolumeValue)
		{
			m_GlobalAudioVolume = aVolumeValue;
			SetBackgroundVolume(m_BackgroundAudioVolume);
			SetVFXVolume(m_VFXAudioVolume);
		}

		public float GetGlobalAudioVolume()
		{
			return m_GlobalAudioVolume;
		}

		public void SetBackgroundVolume(float aValue)
		{
			m_BackgroundAudioVolume = aValue;
			m_MainAudioSource.volume = aValue * m_GlobalAudioVolume;
		}

		public float GetBackgroundVolume()
		{
			return m_BackgroundAudioVolume;
		}

		public void SetVFXVolume(float aValue)
		{
			m_VFXAudioVolume = aValue;
			m_VFXAudioSource.volume = aValue * m_GlobalAudioVolume;

			UpdateLevelAudioSourcesVolumeLevel();
		}

		public float GetVFXVolume()
		{
			return m_VFXAudioVolume;
		}

		public bool IsSoundEnabled()
		{
			return !m_SoundMuted;
		}

		private void SetEverythingToMutedState(bool aMutedFlag)
		{
			m_MainAudioSource.mute = aMutedFlag;
			m_VFXAudioSource.mute = aMutedFlag;

			for (int i = 0; i < m_LevelSources.Count; ++i)
				m_LevelSources[i].mute = aMutedFlag;
		}

		private void UpdateLevelAudioSourcesVolumeLevel()
		{
			for (int i = 0; i < m_LevelSources.Count; ++i)
				m_LevelSources[i].volume = m_VFXAudioVolume * m_GlobalAudioVolume;
		}

		#endregion

		private void UpdateLevelSources()
		{
			m_LevelSources.Clear();
			var audioSources = FindObjectsOfType<AudioSource>(true);
			foreach (var audioSource in audioSources)
			{
				if (audioSource == m_MainAudioSource || audioSource == m_VFXAudioSource)
					continue;

				m_LevelSources.Add(audioSource);
			}
		}

		public void PlayOnShotAudioOnVFXAudioSource(AudioClip aClip)
		{
			if (aClip == null)
				return;

			PlayAudio(m_VFXAudioSource, aClip);
		}

		public void PlayLoopAudioOnMainAudioSource(AudioClip aClip)
		{
			if (aClip == null)
				return;

			PlayAudioLoopInternal(m_MainAudioSource, aClip);
		}

		public void PlayOneShotAudio(AudioClip aClip, AudioSource anAudioSource)
		{
			if (anAudioSource == null || aClip == null)
				return;

			PlayAudio(anAudioSource, aClip);
		}

		public void PlayOneShotAudio(AudioClipDef aClip, AudioSource anAudioSource)
		{
			if (anAudioSource == null || aClip.asset == null)
				return;

			if (aClip.delayToPlaySound > 0.0f)
				StartCoroutine(PlayAudioDelayed(anAudioSource, aClip.asset, aClip.delayToPlaySound));
			else
				PlayAudio(anAudioSource, aClip.asset);
		}

		public void PlayAudioLoop(AudioClipDef aClip, AudioSource anAudioSource)
		{
			if (aClip.delayToPlaySound > 0.0f)
				StartCoroutine(PlayAudioLoopDelayed(anAudioSource, aClip.asset, aClip.delayToPlaySound));
			else
				PlayAudioLoopInternal(anAudioSource, aClip.asset);
		}

		public void PlayAudioLoop(AudioClip aClip, AudioSource anAudioSource)
		{
			PlayAudioLoopInternal(anAudioSource, aClip);
		}

		public AudioDatabase GetAudioDatabase()
		{
			return m_AudioDatabase;
		}

		private IEnumerator PlayAudioDelayed(AudioSource anAudioSource, AudioClip aClipAsset, float delay)
		{
			yield return new WaitForSeconds(delay);
			PlayAudio(anAudioSource, aClipAsset);
		}

		private void PlayAudio(AudioSource anAudioSource, AudioClip aClipAsset)
		{
			anAudioSource.PlayOneShot(aClipAsset);
		}

		private IEnumerator PlayAudioLoopDelayed(AudioSource anAudioSource, AudioClip aClipAsset, float delay)
		{
			yield return new WaitForSeconds(delay);
			PlayAudioLoopInternal(anAudioSource, aClipAsset);
		}

		private void PlayAudioLoopInternal(AudioSource anAudioSource, AudioClip aClipAsset)
		{
			anAudioSource.clip = aClipAsset;
			anAudioSource.loop = true;
			anAudioSource.Play();
		}

	}
}