using UnityEngine;
using static PlayerPlatformer2D.PlayerInputRuntimeData;

namespace PlayerPlatformer2D
{
	public class PlayerPhysicsRuntimeData
	{
		public PhysicsContextType controllerContextType = PhysicsContextType.Main;
		public Vector2 velocity;
		public float speed;
		public bool isMovingSideways = false;
	}

	public class PlayerPhysics : MonoBehaviour
	{
		[SerializeField]
		private PlayerRuntimeData m_RuntimeData = default;

		private PhysicsContextBase[] m_ControllerContexts = default;
		private PhysicsContextType m_CurrentContext = default;

		public void Initialize()
		{
			var contextsRoot = m_RuntimeData.PlayerUnityComponentsRuntimeData.physicsContextsRoot;
			int nContexts = System.Enum.GetValues(typeof(PhysicsContextType)).Length;

			m_ControllerContexts = new PhysicsContextBase[nContexts];
			m_ControllerContexts[(int)PhysicsContextType.Main] = contextsRoot.GetComponent<PhysicsContextMain>();
			m_ControllerContexts[(int)PhysicsContextType.Static] = contextsRoot.GetComponent<PhysicsContextStatic>();

			for (int i = 0; i < m_ControllerContexts.Length; ++i)
				m_ControllerContexts[i].Initialize();

			InitOnContext(PhysicsContextType.Main);
		}

		public void SetContextTo(PhysicsContextType aNewContext)
		{
			if (m_CurrentContext == aNewContext)
				return;

			m_ControllerContexts[(int)m_CurrentContext].EndContext();
			m_CurrentContext = aNewContext;
			m_ControllerContexts[(int)m_CurrentContext].StartContext();

			var data = m_RuntimeData.PlayerPhysicsRuntimeData;
			data.controllerContextType = aNewContext;
		}
	
		public void PreUpdateController()
		{
			for (int i = 0; i < m_ControllerContexts.Length; ++i)
			{
				m_ControllerContexts[i].ContextLayerPreUpdate();
			}
		}

		public void UpdateController()
		{
			m_ControllerContexts[(int)m_CurrentContext].UpdateContext();
		}

		public void PostUpdateController()
		{
			m_ControllerContexts[(int)m_CurrentContext].PostUpdateContext();
		}

		public void UpdatePhysicsController()
		{
			m_ControllerContexts[(int)m_CurrentContext].FixedUpdateContext();

			UpdatePhysicsRuntimeData();
		}

		private void InitOnContext(PhysicsContextType anInitContext)
		{
			m_CurrentContext = anInitContext;
			m_ControllerContexts[(int)m_CurrentContext].StartContext();

			var data = m_RuntimeData.PlayerPhysicsRuntimeData;
			data.controllerContextType = anInitContext;
		}

		private void UpdatePhysicsRuntimeData()
		{
			var data = m_RuntimeData.PlayerPhysicsRuntimeData;
			var rigidbody = m_RuntimeData.PlayerUnityComponentsRuntimeData.rigidBody;
			var collisionData = m_RuntimeData.PlayerCollisionRuntimeData;
			var frameInput = m_RuntimeData.PlayerInputRuntimeData.frameInput;
		
			data.velocity = rigidbody.velocity;
			data.speed = data.velocity.magnitude;
			data.isMovingSideways = data.controllerContextType == PhysicsContextType.Main && Mathf.Abs(data.velocity.x) > 0.01f;

			if (collisionData.onRightWall && frameInput.leftJoystickData.normalizedInput.x > 0)
				data.isMovingSideways = false;

			if (collisionData.onLeftWall && frameInput.leftJoystickData.normalizedInput.x < 0)
				data.isMovingSideways = false;
		}
	}

}