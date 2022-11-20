using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using AudioSystems;

public class LevelSelect : MonoBehaviour
{
	[SerializeField]
	public int m_sceneIndex;

	[SerializeField]
	private Vector3 m_TriggerSize = new Vector3(3, 3, 3);

	[SerializeField]
	private SpriteRenderer m_WinningSpriteRenderer = default;

	[SerializeField]
	private Sprite m_WinningTexture = default;

	private void Start()
	{
        if (PlayerPrefs.HasKey(EndTrigger.LevelCompletedKey(m_sceneIndex)))
		{
			SwapToWinningTexture();
		}
    }

	public void Select()
	{
		SwapToWinningTexture();
	}

    public bool IsInsideTriggerZone(Vector3 pos)
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
}
