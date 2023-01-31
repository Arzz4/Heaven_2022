using PlayerPlatformer2D;
using UnityEngine;

public class NPC_Cat : MonoBehaviour
{
	[SerializeField]
	private Rigidbody2D m_Rigidbody = default;

	[SerializeField]
	private BoxCollider2D m_BoxCollider = default;

	private Transform m_AttachedPlayer = null;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
			return;

		GameObject playerVisuals = collision.GetComponentInChildren<PlayerVisuals>().gameObject;
		AttachToTransform(playerVisuals.transform);
	}

	private void AttachToTransform(Transform aParent)
	{
		m_AttachedPlayer = aParent;
		m_Rigidbody.simulated = false;
		m_BoxCollider.enabled = false;
		
		transform.SetParent(m_AttachedPlayer);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}
}
