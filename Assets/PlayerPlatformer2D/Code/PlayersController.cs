using GameplayUtility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XInputDotNetPure;

namespace PlayerPlatformer2D
{
	public class PlayersController : MonoBehaviour
	{
		[SerializeField]
		private PlayerCharacter[] m_Players = default;

		[SerializeField]
		private float m_DelayBetweenCharacters = 1.0f;

		private int m_CurrentPlayerIndex = 0;
		private List<PlayerRuntimeData> m_PlayersRuntimeData = new List<PlayerRuntimeData>(5);

		[SerializeField]
		private UnityEvent<GameObject, int> m_OnCharacterDead = default;

		private GameObject m_PauseMenuObj = null;

		private void Awake()
		{
			for(int i = 0; i < m_Players.Length; ++i)
				m_PlayersRuntimeData.Add(m_Players[i].GetComponent<PlayerRuntimeData>());

			m_PauseMenuObj = Camera.main.GetComponent<GameObjectHolder>().GameObjectRef;
		}

		private void Start()
		{
			for (int i = 0; i < m_Players.Length; ++i)
				m_Players[i].StopPlayingWithCharacter();

			StartPlayingWithPlayer(m_CurrentPlayerIndex);
		}

		private void LateUpdate()
		{
			// pause menu
			var frameInput = m_PlayersRuntimeData[m_CurrentPlayerIndex].PlayerInputRuntimeData.frameInput;
			if(frameInput.buttonPress[(int)GameActionSingleInputType.TogglePauseMenu])
			{
				m_PauseMenuObj.SetActive(!m_PauseMenuObj.activeSelf);
				if (m_PauseMenuObj.activeSelf)
				{
					m_Players[m_CurrentPlayerIndex].OnPauseMenuOpen();
				}
				else
				{
					m_Players[m_CurrentPlayerIndex].OnPauseMenuClosed();
				}
				return;
			}

			// death handling
			var currentPlayerDeathData = m_PlayersRuntimeData[m_CurrentPlayerIndex].PlayerDeathRuntimeData;
			if(currentPlayerDeathData.state == PlayerDeathRuntimeData.State.Kill)
			{
				KillCharacter(m_CurrentPlayerIndex);
			}
			else if(currentPlayerDeathData.state == PlayerDeathRuntimeData.State.KillAll)
			{
				for (int i = m_CurrentPlayerIndex; i < m_Players.Length; ++i)
					KillCharacter(i);
			}

			if (!m_Players[m_CurrentPlayerIndex].gameObject.activeSelf)
			{
				int nextPlayerIndex = -1;
				for(int i = m_CurrentPlayerIndex + 1; i < m_Players.Length; ++i)
				{
					if (m_Players[i].gameObject.activeInHierarchy)
					{
						nextPlayerIndex = i;
						break;
					}
				}

				int remainingCharacters = nextPlayerIndex < 0 ? 0 : m_Players.Length - nextPlayerIndex;
				m_OnCharacterDead?.Invoke(m_Players[m_CurrentPlayerIndex].gameObject, remainingCharacters);

				// check if we finished playing all characters 
				if (nextPlayerIndex < 0)
				{
					this.enabled = false;
					return;
				}

				m_CurrentPlayerIndex = nextPlayerIndex;

				// otherwise start playing with next character
				StartPlayingWithPlayer(m_CurrentPlayerIndex);
				return;
			}
		}

		private void StartPlayingWithPlayer(int playerIndex)
		{
			m_Players[playerIndex].StartPlayingWithCharacter(m_DelayBetweenCharacters);
		}

		private void KillCharacter(int playerIndex)
		{
			var player = m_Players[playerIndex];
			var playerRuntimeData = m_PlayersRuntimeData[playerIndex];

			// disable game object
			player.gameObject.SetActive(false);

			// disable input
			player.StopPlayingWithCharacter();

			// add to telemetry
			var telemetryManager = TelemetrySystems.TelemetryManager.Instance;
			if (telemetryManager)
				telemetryManager.OnCharacterDeath();

			// instantiate death prefab (handle bouncy velocity continuation)
			var obj = GameObject.Instantiate(playerRuntimeData.PlayerUnityComponentsRuntimeData.deathPrefab, player.transform.position, Quaternion.identity);
			var bounce = obj.GetComponent<Surface_Base>();
			if (bounce != null)
			{
				bounce.setSourceVelocity(playerRuntimeData.PlayerPhysicsRuntimeData.velocity);
			}

			// make sure all objects attached on visuals are detached
			Transform playerVisuals = player.GetComponentInChildren<Animator>().transform;
			for(int i = playerVisuals.childCount-1; i >= 0; --i) 
			{
				playerVisuals.GetChild(i).SetParent(null, true);
			}
		}
	}
}

