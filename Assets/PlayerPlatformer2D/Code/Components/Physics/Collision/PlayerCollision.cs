﻿using System;
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
		public bool onStickySurface = false;
		public bool onSpeedySurface = false;
		public bool onBouncySurface = false;
		public int wallSide = 0;
		public bool onMovingPlatform = false;
		public float onGroundTimestamp = 0.0f;
		public float leftGroundTimestamp = 0.0f;
		public Transform movingPlatform = null;
		public GameObject groundObject = null;
		public bool justLanded = false;
		public bool justLeftGround = false;

		public const int maxHits = 1;
		public Collider[] groundHits = new Collider[maxHits];
		public RaycastHit[] frontWallHits = new RaycastHit[maxHits];
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
			Collider2D groundCollider = Physics2D.OverlapCircle((Vector2)transform.position + collisionSettings.BottomOffset, collisionSettings.GroundCollisionRadius, collisionSettings.GroundLayer);
			collisionData.onGround = groundCollider != null;
			collisionData.groundObject = groundCollider != null ? groundCollider.gameObject : null;
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

			// update surface physics modification
			UpdatePhysicsChangesBasedOnSurfaces();

			// walls
			bool wasOnWall = collisionData.onWall;
			Collider2D rightWallCollider = Physics2D.OverlapCircle((Vector2)transform.position + collisionSettings.RightOffset, collisionSettings.WallCollisionRadius, collisionSettings.GroundLayer);
			Collider2D leftWallCollider = Physics2D.OverlapCircle((Vector2)transform.position + collisionSettings.LeftOffset, collisionSettings.WallCollisionRadius, collisionSettings.GroundLayer);

			collisionData.onRightWall = rightWallCollider != null;
			collisionData.onLeftWall = leftWallCollider != null;
			collisionData.onWall = collisionData.onRightWall || collisionData.onLeftWall;
			collisionData.wallSide = collisionData.onWall ? (collisionData.onRightWall ? -1 : 1) : 0;
			collisionData.onStickySurface = !collisionData.onWall ? collisionData.onStickySurface : (collisionData.onRightWall ? rightWallCollider.CompareTag("StickySurface") : leftWallCollider.CompareTag("StickySurface"));

			// platforms
			Collider2D platformCollider = groundCollider ? groundCollider : (rightWallCollider ? rightWallCollider : (leftWallCollider ? leftWallCollider : null));
			collisionData.movingPlatform = platformCollider && platformCollider.CompareTag("MovingPlatform") ? platformCollider.transform : null;
			transform.parent = collisionData.movingPlatform ? collisionData.movingPlatform : null;

			// WARNING! rotational platforms are changing the player's rotation!
			//if(m_Transform.parent == null)
				//m_Transform.eulerAngles = new Vector3(m_Transform.eulerAngles.x, m_Transform.eulerAngles.y, 0.0f);
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

			Gizmos.color = collisionData.onGround ? Color.green : Color.red;
			Gizmos.DrawWireSphere((Vector2)transform.position + collisionSettings.BottomOffset, collisionSettings.GroundCollisionRadius);

			Gizmos.color = collisionData.onRightWall ? Color.green : Color.red;
			Gizmos.DrawWireSphere((Vector2)transform.position + collisionSettings.RightOffset, collisionSettings.WallCollisionRadius);

			Gizmos.color = collisionData.onLeftWall ? Color.green : Color.red;
			Gizmos.DrawWireSphere((Vector2)transform.position + collisionSettings.LeftOffset, collisionSettings.WallCollisionRadius);

			if (collisionData.movingPlatform)
			{
				Gizmos.color = Color.magenta;
				Gizmos.DrawWireCube(collisionData.movingPlatform.position, Vector3.one * 0.5f);
			}
		}
	}
}