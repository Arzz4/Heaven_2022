using UnityEngine;
using UnityEngine.SceneManagement;

namespace TelemetrySystems
{

	public class TelemetryData
	{
		public int numberOfDeaths = 0;
	}

	public class TelemetryManager : MonoBehaviour
	{
		private static TelemetryManager m_Instance;
		public static TelemetryManager Instance { get { return m_Instance; } }

		[SerializeField]
		private int m_Achivement_TargetNumberOfDeaths = 20;

		[SerializeField]
		private GameObject m_Achievement_TargetNumberOfDeathsPrefab = default;

		private bool m_GotAchievement_TargetNumberOfDeaths = false;

		private TelemetryData m_TelemetryData;

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

			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private void InitializeTelemetry()
		{
			m_TelemetryData = new TelemetryData();
		}

		public void OnCharacterDeath()
		{
			m_TelemetryData.numberOfDeaths++;
		}

		public TelemetryData GetData()
		{
			return m_TelemetryData;
		}

		private void OnSceneLoaded(Scene anScene, LoadSceneMode aMode)
		{
			if (!m_GotAchievement_TargetNumberOfDeaths && m_TelemetryData.numberOfDeaths >= m_Achivement_TargetNumberOfDeaths)
			{
				m_GotAchievement_TargetNumberOfDeaths = true;

				var poolSystem = GameplayUtility.ObjectPoolSystem.Instance;
				if (poolSystem)
					poolSystem.InstantiatePrefabWith(m_Achievement_TargetNumberOfDeathsPrefab, Vector3.zero, Quaternion.identity);
			}
		}
	}
}