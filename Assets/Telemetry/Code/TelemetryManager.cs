using GameplayUtility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TelemetrySystems
{
	public class TelemetryData
	{
		public int numberOfDeaths = 0;
		public int highestLevelReached = 0;
	}

	public class TelemetryManager : MonoBehaviour
	{
		private static TelemetryManager m_Instance;
		public static TelemetryManager Instance { get { return m_Instance; } }

		[SerializeField]
		private GameObject[] m_AllAchievementPrefabs = default;

		[SerializeField]
		private int m_Achievement_TargetNumberOfDeaths = 20;

		[SerializeField]
		private int m_Achievement_FirstHiddenLevelIndex = 12;

		// Internals 
		private TelemetryData m_TelemetryData;
		private bool[] m_AchievementsCheckList;
		private int[] m_AchievementsPrefabIndexes;
		private Queue<AchievementType> m_AchievementsQueue;
		private float m_NextNotificationTimestamp;

		private void Awake()
		{
			if (m_Instance != null)
			{
				DestroyImmediate(gameObject);
				return;
			}

			m_Instance = this;
			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			InitializeTelemetry();
			InitializeAchievementsCheckList();
			InitializePrefabsIndices();
			InitializeQueue();

			SceneManager.sceneLoaded += OnSceneLoaded;
		}
		private void InitializeTelemetry()
		{
			m_TelemetryData = new TelemetryData();
		}

		private void InitializeAchievementsCheckList()
		{
			int nAchievements = (int)AchievementType.Count;
			m_AchievementsCheckList = new bool[nAchievements];
			for (int i = 0; i < nAchievements; ++i)
				m_AchievementsCheckList[i] = false;
		}

		private void InitializePrefabsIndices()
		{
			int nAchievements = (int)AchievementType.Count;
			m_AchievementsPrefabIndexes = new int[nAchievements];
			for (int i = 0, e = m_AllAchievementPrefabs.Length; i < e; ++i)
			{
				AchievementType t = m_AllAchievementPrefabs[i].GetComponent<AchievementDefinition>().AchievementType;
				m_AchievementsPrefabIndexes[(int)t] = i;
			}
		}

		private void InitializeQueue()
		{
			m_AchievementsQueue = new Queue<AchievementType>();
			m_NextNotificationTimestamp = Time.time;
		}

		public void OnCharacterDeath()
		{
			m_TelemetryData.numberOfDeaths++;
		}

		public TelemetryData GetData()
		{
			return m_TelemetryData;
		}
		
		public void OnFinishedLevel(int remainingCharacters, bool hasCatWhenFinishing = false)
		{
			if(!m_AchievementsCheckList[(int)AchievementType.PlusOneSurvivorFinish] && remainingCharacters > 0)
			{
				QueueTrophyNotification(AchievementType.PlusOneSurvivorFinish);
			}

			if (!m_AchievementsCheckList[(int)AchievementType.CatPersonFinish] && hasCatWhenFinishing)
			{
				QueueTrophyNotification(AchievementType.CatPersonFinish);
			}
		}

		private void Update()
		{
			if (m_AchievementsQueue.Count == 0)
			{
				this.enabled = false;
				return;
			}

			if(Time.time > m_NextNotificationTimestamp) 
			{
				TriggerTrophyNotification(m_AchievementsQueue.Dequeue());
			}
		}

		private void OnSceneLoaded(Scene anScene, LoadSceneMode aMode)
		{
			m_TelemetryData.highestLevelReached = Mathf.Max(anScene.buildIndex, m_TelemetryData.highestLevelReached);

			if (!m_AchievementsCheckList[(int)AchievementType.NumberOfDeaths] && m_TelemetryData.numberOfDeaths >= m_Achievement_TargetNumberOfDeaths)
			{
				QueueTrophyNotification(AchievementType.NumberOfDeaths);
			}

			if(!m_AchievementsCheckList[(int)AchievementType.ReachedHiddenLevels] && m_TelemetryData.highestLevelReached >= m_Achievement_FirstHiddenLevelIndex)
			{
				QueueTrophyNotification(AchievementType.ReachedHiddenLevels);
			}
		}

		private void QueueTrophyNotification(AchievementType aType)
		{
			m_AchievementsQueue.Enqueue(aType);
			this.enabled = true;
		}

		private void TriggerTrophyNotification(AchievementType aType)
		{
			m_AchievementsCheckList[(int)aType] = true;

			int prefabIndex = m_AchievementsPrefabIndexes[(int)aType];

			var poolSystem = ObjectPoolSystem.Instance;
			if (poolSystem)
			{
				GameObject throphy = poolSystem.InstantiatePrefabWith(m_AllAchievementPrefabs[prefabIndex], Vector3.zero, Quaternion.identity);
				DontDestroyOnLoad(throphy);
				m_NextNotificationTimestamp = Time.time + m_AllAchievementPrefabs[prefabIndex].GetComponent<ObjectLifetime>().LifeTime;
			}
		}

		public bool HasUnlockedAchievement(AchievementType aType) 
		{
			return m_AchievementsCheckList != null ? m_AchievementsCheckList[(int)aType] : false;
		}
	}
}