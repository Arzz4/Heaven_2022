using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace PlayerPlatformer2D
{
	public class PlayerCharacter : MonoBehaviour
	{
		[SerializeField]
		private PlayerRuntimeData m_RuntimeData = default;

		[SerializeField]
		private PlayerUnityComponents m_UnityComponents = default;

		[SerializeField]
		private PlayerInput m_Input = default;

		[SerializeField]
		private PlayerCollision m_Collision = default;

		[SerializeField]
		private PlayerPhysics m_Physics = default;

		[SerializeField]
		private PlayerOrientation m_Orientation = default;

		#region UNITY METHODS

		private void Awake()
		{
			Initialize();
		}

		private void Update()
		{
			FramePreUpdate();
			FrameUpdate();
			FramePostUpdate();
		}
	
		private void LateUpdate()
		{
			FrameLateUpdate();
		}

		private void FixedUpdate()
		{
			PhysicsUpdate();
		}

		#endregion

		#region GAMELOOP

		private void Initialize()
		{
			m_RuntimeData.Initialize();
			m_UnityComponents.Initialize();
			m_Input.Initialize();
			m_Physics.Initialize();
		}

		private void FramePreUpdate()
		{
			m_Input.UpdateFrameInput();
			m_Collision.UpdateCollisions();
			m_Physics.PreUpdateController();
		}

		private void FrameUpdate()
		{
			m_Physics.UpdateController();
		}

		private void FramePostUpdate()
		{
			m_Physics.PostUpdateController();
			m_Orientation.PostUpdateCharacterOrientation();
		}

		private void FrameLateUpdate()
		{
			m_RuntimeData.OnFrameLateUpdate();
		}

		private void PhysicsUpdate()
		{
			m_Physics.UpdatePhysicsController();
		}

		#endregion
	}
}