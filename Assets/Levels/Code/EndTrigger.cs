using PlayerPlatformer2D;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
	[SerializeField]
	private int m_NextSceneIndex;

	[SerializeField]
	private Vector3 m_TriggerSize = new Vector3(3, 3, 3);

	private bool m_Loading = false;

	public void OnCharacterDead(GameObject character, int remainingCharacters)
	{
		if (m_Loading)
			return;

		if (IsInsideTriggerZone(character.transform.position))
			StartNextScene();

		else if (remainingCharacters == 0)
			ResetCurrentLevel();
	}

	public void ResetCurrentLevel()
	{
		m_Loading = true;
		Debug.Log("Reset current level " + this.GetInstanceID());
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void StartNextScene()
	{
		m_Loading = true;
		Debug.Log("Loading next level: " + m_NextSceneIndex + "  " + this.GetInstanceID());
		SceneManager.LoadScene(m_NextSceneIndex);
	}

	private bool IsInsideTriggerZone(Vector3 pos)
	{
		Vector3 point = transform.InverseTransformPoint(pos);
		float halfX = m_TriggerSize.x * 0.5f;
		float halfY = m_TriggerSize.y * 0.5f;
		float halfZ = m_TriggerSize.z * 0.5f;
		return (point.x < halfX && point.x > -halfX && point.y < halfY && point.y > -halfY && point.z < halfZ && point.z > -halfZ);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, m_TriggerSize);
		Gizmos.matrix = Matrix4x4.identity;
	}
}
