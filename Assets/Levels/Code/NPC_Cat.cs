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

		AttachToTransform(collision.transform);
	}

	private void Update()
	{
		if (m_AttachedPlayer == null)
		{
			return;
		}

		if(!m_AttachedPlayer.gameObject.activeSelf)
		{
			Detach();
			return;
		}
	}

	private void AttachToTransform(Transform aParent)
	{
		m_AttachedPlayer = aParent;
		m_Rigidbody.simulated = false;

		Transform playerVisuals = aParent.GetComponentInChildren<Animator>().transform;
		transform.SetParent(playerVisuals);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}

	private void Detach()
	{
		transform.SetParent(null, true);

		m_AttachedPlayer = null;
		m_Rigidbody.simulated = true;
	}
}
