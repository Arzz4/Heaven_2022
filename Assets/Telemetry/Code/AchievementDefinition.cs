using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TelemetrySystems
{
	public enum AchievementType
	{
		NumberOfDeaths = 0,
		ReachedHiddenLevels,
		PlusOneSurvivorFinish,
		CatPersonFinish,

		Count
	}

	public class AchievementDefinition : MonoBehaviour
	{
		[SerializeField]	
		private AchievementType m_AchievementType = default;
		public AchievementType AchievementType { get { return m_AchievementType; } }
	}
}