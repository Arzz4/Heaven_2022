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

				m_CurrentPlayerIndex++;

				// check if we finished playing all characters 
				if (m_CurrentPlayerIndex == m_Players.Length)
				{
					this.enabled = false;
					return;
				}

				// otherwise start playing with next character
				StartPlayingWithPlayer(m_CurrentPlayerIndex);
				return;
			}
		}

		private void StartPlayingWithPlayer(int playerIndex)
		{
			m_Players[playerIndex].StartPlayingWithCharacter();
		}
	}
}