using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using AudioSystems;
using System.Collections.Generic;

public class EndTrigger : MonoBehaviour
{
	[SerializeField]
	private int m_NextSceneIndex;

	[SerializeField]
	private Vector3 m_TriggerSize = new Vector3(3, 3, 3);

	[SerializeField]
	private SpriteRenderer m_WinningSpriteRenderer = default;

	[SerializeField]
	private Sprite m_WinningTexture = default;

	public float levelSwitchDelay = 0.7f;
	public float levelResetDelay = 0.7f;

	private bool m_Loading = false;

	private List<LevelSelect> otherLevels;

	private void Start()
	{
		otherLevels = new List<LevelSelect>(FindObjectsOfType<LevelSelect>());
	}

	public void OnCharacterDead(GameObject character, int remainingCharacters)
	{
		if (m_Loading)
			return;


        if (IsInsideTriggerZone(character.transform.position))
		{
			TelemetrySystems.TelemetryManager.Instance.OnFinishedLevel(remainingCharacters);
			PlayGoalReachedAudio();
			SwapToWinningTexture();
			StartNextScene();
			return;
		}

        foreach (var level in otherLevels)
        {
            if (level.IsInsideTriggerZone(character.transform.position))
            {
                PlayGoalReachedAudio();
                level.Select();
                LoadOtherScene(level);
                return;
            }
        }

        if (remainingCharacters == 0)
		{
			ResetCurrentLevel();
		}

	}

	private void PlayGoalReachedAudio()
	{
		var manager = AudioManager.Instance;
		if (manager == null)
			return;

		var db = manager.GetAudioDatabase();
		if (db == null)
			return;

		manager.PlayOnShotAudioOnVFXAudioSource(db.PlayerHeavenlyGoal);
	}

	public void ResetCurrentLevel()
	{
		m_Loading = true;
		Debug.Log("Reset current level");
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex, levelResetDelay));
    }

	public void StartNextScene()
	{
		m_Loading = true;
		Debug.Log("Loading next level: " + m_NextSceneIndex);
		SaveLevelCompleted();
		StartCoroutine(loadLevel(m_NextSceneIndex,levelSwitchDelay));
	}

    public void LoadOtherScene(LevelSelect select)
    {
        m_Loading = true;
        Debug.Log("Loading other level: " + select.m_sceneIndex);
        StartCoroutine(loadLevel(select.m_sceneIndex, levelSwitchDelay));
    }

    public void LoadOtherScene(LevelSelectButton select)
    {
        m_Loading = true;
        Debug.Log("Loading other level: " + select.m_sceneIndex);
        StartCoroutine(loadLevel(select.m_sceneIndex, levelSwitchDelay));
    }

    private IEnumerator loadLevel(int index, float delay)
    {
		yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(index);
    }

    private bool IsInsideTriggerZone(Vector3 pos)
	{
		Vector3 point = transform.InverseTransformPoint(pos);
		float halfX = m_TriggerSize.x * 0.5f;
		float halfY = m_TriggerSize.y * 0.5f;
		float halfZ = m_TriggerSize.z * 0.5f;
		return (point.x < halfX && point.x > -halfX && point.y < halfY && point.y > -halfY && point.z < halfZ && point.z > -halfZ);
	}

	private void SwapToWinningTexture()
	{
		m_WinningSpriteRenderer.sprite = m_WinningTexture;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, m_TriggerSize);
		Gizmos.matrix = Matrix4x4.identity;
	}

	private void SaveLevelCompleted()
	{
		PlayerPrefs.SetInt(LevelCompletedKey(SceneManager.GetActiveScene().buildIndex), 1);
		PlayerPrefs.Save();
	}

	public static string LevelCompletedKey(int level)
	{
		return "level_completed_" + level;
	}
}
