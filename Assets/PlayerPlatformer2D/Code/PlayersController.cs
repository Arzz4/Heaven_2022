using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerPlatformer2D
{
	public class PlayersController : MonoBehaviour
	{
		[SerializeField]
		private PlayerCharacter[] m_Players = default;

		[SerializeField]
		private float m_DelayBetweenCharacters = 1.0f;

		private int m_CurrentPlayerIndex = 0;

		[SerializeField]
		private UnityEvent<GameObject, int> m_OnCharacterDead = default;

		private void Start()
		{
			for (int i = 0; i < m_Players.Length; ++i)
				m_Players[i].StopPlayingWithCharacter();

			StartPlayingWithPlayer(m_CurrentPlayerIndex);
		}

		private void LateUpdate()
		{
			if (!m_Players[m_CurrentPlayerIndex].gameObject.activeSelf)
			{
				int remainingCharacters = m_Players.Length - m_CurrentPlayerIndex - 1;
				m_OnCharacterDead?.Invoke(m_Players[m_CurrentPlayerIndex].gameObject, remainingCharacters);

				int nextPlayerIndex = -1;
				for(int i = m_CurrentPlayerIndex + 1; i < m_Players.Length; ++i)
				{
					if (m_Players[i].gameObject.activeInHierarchy)
					{
						nextPlayerIndex = i;
						break;
					}
				}

				// check if we finished playing all characters 
				if (m_CurrentPlayerIndex == m_Players.Length || nextPlayerIndex < 0)
				{
					this.enabled = false;
					return;
				}

				m_CurrentPlayerIndex = nextPlayerIndex;

				// otherwise start playing with next character
				StartPlayingWithPlayer(m_CurrentPlayerIndex);
				return;
			}

			//TODO: hack to make the non-playable characters to fall fast
			for (int i = m_CurrentPlayerIndex + 1; i < m_Players.Length; ++i)
			{
				m_Players[i].UpdateWhileNotPlaying();
			}
		}

		private void StartPlayingWithPlayer(int playerIndex)
		{
			m_Players[playerIndex].StartPlayingWithCharacter(m_DelayBetweenCharacters);
		}
	}
}