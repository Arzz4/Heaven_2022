using AudioSystems;
using GameplayUtility;
using System;
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

				PlayGhostAudio();
			}
		}

		private void PlayGhostAudio()
		{
			var manager = AudioManager.Instance;
			if (manager == null)
				return;

			var db = manager.GetAudioDatabase();
			if (db == null)
				return;

			manager.PlayOnShotAudioOnVFXAudioSource(db.PlayerGhostDeath);
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