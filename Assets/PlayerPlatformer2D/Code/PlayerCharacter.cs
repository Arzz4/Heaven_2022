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

		[SerializeField]
		private PlayerAnimator m_Animator = default;

		[SerializeField]
		private PlayerDeath m_DeathBehaviour = default;

		[SerializeField]
		private PlayerAudio m_Audio = default;

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
			m_DeathBehaviour.Initialize();
			m_Physics.Initialize();
			m_Input.Initialize();
		}

		private void FramePreUpdate()
		{
			m_Input.UpdateFrameInput();

			if (m_DeathBehaviour.UpdateDeathBehaviour())
				return;

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
			m_Animator.PostUpdateAnimations();
			m_Audio.PostUpdateAudio();
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

		public void StartPlayingWithCharacter(float delayBetweenCharacters)
		{
			StartCoroutine(DelayToStartPlayingWithCharacter(delayBetweenCharacters));
		}

		public void StopPlayingWithCharacter()
		{
			m_Input.DisableInput();
			m_Physics.SetContextTo(PhysicsContextType.Static);
		}

		private IEnumerator DelayToStartPlayingWithCharacter(float delay)
		{
			var arrowObj = m_RuntimeData.PlayerUnityComponentsRuntimeData.characterArrow;

			if(arrowObj != null) 
				arrowObj.gameObject.SetActive(true);

			yield return new WaitForSeconds(delay);

			if (arrowObj != null)
				arrowObj.gameObject.SetActive(false);

			m_Input.EnableInput();
			m_Physics.SetContextTo(PhysicsContextType.Main);
		}
	}
}