using System.Collections;
using System.Collections.Generic;
using TelemetrySystems;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuNavigation : MonoBehaviour
{	
	[SerializeField]
	private EventSystem m_UIEventSystem = default;

	[SerializeField]
	private Transform m_buttonsContainer = default;

	[SerializeField]
	private UITrophyUnlocked[] m_Trophies = default;

	public void OnEnable()
	{
		int selectionIndex = SceneManager.GetActiveScene().buildIndex - 1;
		if (selectionIndex < 0 || selectionIndex >= m_buttonsContainer.childCount)
			selectionIndex = 0;

		m_UIEventSystem.SetSelectedGameObject(m_buttonsContainer.GetChild(selectionIndex).gameObject);

		for(int i = 0; i < m_Trophies.Length; i++) 
		{
			m_Trophies[i].UpdateUITrophy();
		}

	}
}
