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
		public int wallSide = 0;
		public bool onMovingPlatform = false;
		public float onGroundTimestamp = 0.0f;
		public Transform movingPlatform = null;
		public GameObject groundObject = null;
		public bool justLanded = false;

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
			collisionData.justLanded = false;

			if (!wasOnGround && collisionData.onGround)
			{
				OnTouchingSurface(groundCollider.gameObject);
				collisionData.justLanded = true;
			}

			// walls
			bool wasOnWall = collisionData.onWall;
			Collider2D rightWallCollider = Physics2D.OverlapCircle((Vector2)transform.position + collisionSettings.RightOffset, collisionSettings.WallCollisionRadius, collisionSettings.GroundLayer);
			Collider2D leftWallCollider = Physics2D.OverlapCircle((Vector2)transform.position + collisionSettings.LeftOffset, collisionSettings.WallCollisionRadius, collisionSettings.GroundLayer);

			collisionData.onRightWall = rightWallCollider != null;
			collisionData.onLeftWall = leftWallCollider != null;
			collisionData.onWall = collisionData.onRightWall || collisionData.onLeftWall;
			collisionData.wallSide = collisionData.onWall ? (collisionData.onRightWall ? -1 : 1) : 0;
			collisionData.onStickySurface = !collisionData.onWall ? collisionData.onStickySurface : (collisionData.onRightWall ? rightWallCollider.CompareTag("StickySurface") : leftWallCollider.CompareTag("StickySurface"));

			if (!wasOnWall && collisionData.onWall)
				OnTouchingSurface(collisionData.onRightWall ? rightWallCollider.gameObject : leftWallCollider.gameObject);

			// platforms
			Collider2D platformCollider = groundCollider ? groundCollider : (rightWallCollider ? rightWallCollider : (leftWallCollider ? leftWallCollider : null));
			collisionData.movingPlatform = platformCollider && platformCollider.CompareTag("MovingPlatform") ? platformCollider.transform : null;
			transform.parent = collisionData.movingPlatform ? collisionData.movingPlatform : null;

			// WARNING! rotational platforms are changing the player's rotation!
			//if(m_Transform.parent == null)
				//m_Transform.eulerAngles = new Vector3(m_Transform.eulerAngles.x, m_Transform.eulerAngles.y, 0.0f);
		}

		private void OnTouchingSurface(GameObject aGroundObj)
		{
			var collisionData = m_RuntimeData.PlayerCollisionRuntimeData;
			collisionData.onGroundTimestamp = Time.time;

			var modifyPlayerSettings = aGroundObj.GetComponent<Surface_ModifyPlayerPhysicsSettings>();
			if(modifyPlayerSettings != null)
			{
				modifyPlayerSettings.OnPlayerTouchesSurface(m_RuntimeData);
			}
			else if(collisionData.onStickySurface)
			{
				m_RuntimeData.ApplyStickySurfaceSettings();
			}
			else if(collisionData.onSpeedySurface)
			{
				m_RuntimeData.ApplySpeedySurfaceSettings();
			}
			else
			{
				m_RuntimeData.ResetToDefaultPhysicsContextsSettings();
			}

			var bounceSurface = aGroundObj.GetComponent<Surface_Bounce>();
			if(bounceSurface != null)
			{
				bounceSurface.OnPlayerTouchesSurface(m_RuntimeData);
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