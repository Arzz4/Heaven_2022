using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TelemetrySystems
{
	public class UITrophyUnlocked : MonoBehaviour
	{
		[SerializeField]
		private AchievementType m_TrophyAchievement = default;

		public void UpdateUITrophy()
		{
			bool trophyEnabled = TelemetryManager.Instance.HasUnlockedAchievement(m_TrophyAchievement);
			gameObject.SetActive(trophyEnabled);
		}
	}
}

