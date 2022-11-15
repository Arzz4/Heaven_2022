using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public class PlayerCollisionRuntimeData
	{
		// settings
		public PlayerCollisionSettings settings;

		// data
		public bool onGround = false;
		public bool onRightWall = false;
		public bool onLeftWall = false;
		public bool onWall = false;
		public bool onEdge = false;
		public bool onStickySurface = false;
		public bool onSpeedySurface = false;
		public bool onBouncySurface = false;
		public int wallSide = 0;
		public bool onMovingPlatform = false;
		public float onGroundTimestamp = 0.0f;
		public float leftGroundTimestamp = 0.0f;
		public GameObject groundObject = null;
		public bool justLanded = false;
		public bool justLeftGround = false;

		public const int maxHits = 1;
		public RaycastHit2D[] midGroundHits = new RaycastHit2D[maxHits];
		public RaycastHit2D[] leftGroundHits = new RaycastHit2D[maxHits];
		public RaycastHit2D[] rightGroundHits = new RaycastHit2D[maxHits];
		public int nFrameMidHits = 0;
		public int nFrameLeftHits = 0;
		public int nFrameRightHits = 0;
	}

	[RequireComponent(typeof(Collider2D))]
	public class PlayerCollision : MonoBehaviour
	{
		[SerializeField]
		private PlayerRuntimeData m_RuntimeData = default;

		public void UpdateCollisions()
		{
			var collisionData = m_RuntimeData.PlayerCollisionRuntimeData;
			var collisionSettings = collisionData.settings;

			// ground
			bool wasOnGround = collisionData.onGround;
			
			RaycastToGround();
			GameObject groundCollider = collisionData.groundObject;

			collisionData.onStickySurface = groundCollider == null ? false : groundCollider.CompareTag("StickySurface");
			collisionData.onSpeedySurface = groundCollider == null ? false : groundCollider.CompareTag("SpeedySurface");
			collisionData.onBouncySurface = groundCollider == null ? false : groundCollider.CompareTag("BouncySurface");
			collisionData.justLanded = false;
			collisionData.justLeftGround = false;

			if (!wasOnGround && collisionData.onGround)
			{
				collisionData.justLanded = true;
				collisionData.onGroundTimestamp = Time.time;
			}

			if(wasOnGround && !collisionData.onGround)
			{
				collisionData.justLeftGround = true;
				collisionData.leftGroundTimestamp = Time.time;
			}

			// walls
			Collider2D rightWallCollider = Physics2D.OverlapCircle((Vector2)transform.position + collisionSettings.RightOffset, collisionSettings.WallCollisionRadius, collisionSettings.GroundLayer);
			Collider2D leftWallCollider = Physics2D.OverlapCircle((Vector2)transform.position + collisionSettings.LeftOffset, collisionSettings.WallCollisionRadius, collisionSettings.GroundLayer);

			collisionData.onRightWall = rightWallCollider != null;
			collisionData.onLeftWall = leftWallCollider != null;
			collisionData.onWall = collisionData.onRightWall || collisionData.onLeftWall;
			collisionData.wallSide = collisionData.onWall ? (collisionData.onRightWall ? -1 : 1) : 0;
			collisionData.onStickySurface = !collisionData.onWall ? collisionData.onStickySurface : (collisionData.onRightWall ? rightWallCollider.CompareTag("StickySurface") : leftWallCollider.CompareTag("StickySurface"));

			// update surface physics modification
			UpdatePhysicsChangesBasedOnSurfaces();
		}

		private void RaycastToGround()
		{
			var data = m_RuntimeData.PlayerCollisionRuntimeData;
			var collisionSettings = data.settings;

			Vector3 midRayStartPos = transform.position + new Vector3(collisionSettings.GroundRaycastCenterOffset.x, collisionSettings.GroundRaycastCenterOffset.y, 0.0f);
			Vector3 leftRayStartPos = midRayStartPos + Vector3.left * collisionSettings.GroundPlanarSize * 0.5f;
			Vector3 rightRayStartPos = midRayStartPos + Vector3.right * collisionSettings.GroundPlanarSize * 0.5f;

			data.nFrameMidHits = Physics2D.RaycastNonAlloc(midRayStartPos, Vector2.down , data.midGroundHits, collisionSettings.GroundRaycastDistance, collisionSettings.GroundLayer);
			data.nFrameLeftHits = Physics2D.RaycastNonAlloc(leftRayStartPos, Vector2.down, data.leftGroundHits, collisionSettings.GroundRaycastDistance, collisionSettings.GroundLayer);
			data.nFrameRightHits = Physics2D.RaycastNonAlloc(rightRayStartPos, Vector2.down, data.rightGroundHits, collisionSettings.GroundRaycastDistance, collisionSettings.GroundLayer);

			if (data.nFrameMidHits > 0)
				data.groundObject = data.midGroundHits[0].collider.gameObject;

			else if (data.nFrameLeftHits > 0)
				data.groundObject = data.leftGroundHits[0].collider.gameObject;

			else if (data.nFrameRightHits > 0)
				data.groundObject = data.rightGroundHits[0].collider.gameObject;

			else
				data.groundObject = null;

			// update ground state
			data.onGround = data.groundObject != null;

			// update edge state
			if (data.onGround && data.nFrameRightHits == 0)
				data.onEdge = true;

			else if (data.onGround && data.nFrameLeftHits == 0)
				data.onEdge = true;

			else
				data.onEdge = false;
		}

		private void UpdatePhysicsChangesBasedOnSurfaces()
		{
			var collisionData = m_RuntimeData.PlayerCollisionRuntimeData;

			if (collisionData.onStickySurface)
			{
				m_RuntimeData.ApplyStickySurfaceSettings();
			}
			else if (collisionData.onSpeedySurface)
			{
				m_RuntimeData.ApplySpeedySurfaceSettings();
			}
			else if(collisionData.onBouncySurface)
			{
				var bounceSurface = collisionData.groundObject.GetComponent<Surface_Bounce>();
				if (bounceSurface != null)
				{
					bounceSurface.OnPlayerTouchesSurface(m_RuntimeData);
				}

				var modifyPlayerSettings = collisionData.groundObject.GetComponent<Surface_ModifyPlayerPhysicsSettings>();
				if (modifyPlayerSettings != null)
				{
					modifyPlayerSettings.OnPlayerTouchesSurface(m_RuntimeData);
				}
			}
			else if(collisionData.onGround)
			{
				m_RuntimeData.ResetToDefaultPhysicsContextsSettings();
			}
		}

		public void OnDrawGizmos()
		{
			if (m_RuntimeData == null || m_RuntimeData.PlayerCollisionRuntimeData == null || m_RuntimeData.PlayerCollisionRuntimeData.settings == null)
				return;

			var collisionData = m_RuntimeData.PlayerCollisionRuntimeData;
			var collisionSettings = collisionData.settings;

			Gizmos.color = collisionData.nFrameMidHits == 0 ? Color.green : Color.red;
			Vector3 midRayStartPos = transform.position + new Vector3(collisionSettings.GroundRaycastCenterOffset.x, collisionSettings.GroundRaycastCenterOffset.y, 0.0f);
			Gizmos.DrawLine(midRayStartPos, midRayStartPos + Vector3.down * collisionSettings.GroundRaycastDistance);

			Gizmos.color = collisionData.nFrameLeftHits == 0 ? Color.green : Color.red;
			Vector3 leftRayStartPos = midRayStartPos + Vector3.left * collisionSettings.GroundPlanarSize * 0.5f;
			Gizmos.DrawLine(leftRayStartPos, leftRayStartPos + Vector3.down * collisionSettings.GroundRaycastDistance);

			Gizmos.color = collisionData.nFrameRightHits == 0 ? Color.green : Color.red;
			Vector3 rightRayStartPos = midRayStartPos + Vector3.right * collisionSettings.GroundPlanarSize * 0.5f;
			Gizmos.DrawLine(rightRayStartPos, rightRayStartPos + Vector3.down * collisionSettings.GroundRaycastDistance);

			Gizmos.color = collisionData.onRightWall ? Color.green : Color.red;
			Gizmos.DrawWireSphere((Vector2)transform.position + collisionSettings.RightOffset, collisionSettings.WallCollisionRadius);

			Gizmos.color = collisionData.onLeftWall ? Color.green : Color.red;
			Gizmos.DrawWireSphere((Vector2)transform.position + collisionSettings.LeftOffset, collisionSettings.WallCollisionRadius);
		}
	}
}