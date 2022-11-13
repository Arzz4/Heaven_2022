using GameplayUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public class PlayerDeadZone : MonoBehaviour
	{
		[SerializeField]
		private GameObject m_GhostPrefab = default;

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
			Gizmos.matrix = Matrix4x4.identity;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			var obj = collision.gameObject;
			if (obj.GetComponent<PlayerCharacter>() != null)
			{
				obj.SetActive(false);
				ObjectPoolSystem.Instance.InstantiatePrefabWith(m_GhostPrefab, collision.transform.position, Quaternion.identity);
			}
		}

		private bool IsPointInsideDeadZone(Vector3 p)
		{
			Vector3 point = transform.InverseTransformPoint(p);
			float halfX = 0.5f;
			float halfY = 0.5f;
			float halfZ = 0.5f;
			return (point.x < halfX && point.x > -halfX && point.y < halfY && point.y > -halfY && point.z < halfZ && point.z > -halfZ);
		}

	}
}