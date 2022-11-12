using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public enum PhysicsContextType
	{
		Main = 0, // 2d motion + jump
		Static,
	}

	public abstract class PhysicsContextBase : MonoBehaviour
	{
		protected PhysicsContextType m_ContextType = default;
		public PhysicsContextType GetContextType() { return m_ContextType; }
		public virtual void Initialize() { } // runs when loading character (on Awake)
		public virtual void StartContext() { } // runs when this context starts to be the current context
		public virtual void EndContext() { } // runs when this context finishs to be the current context
		public virtual void ContextLayerPreUpdate() { } // runs every frame without mattering if this is the current context of the FSM.
		public virtual void UpdateContext() { } // runs only when this context is the current context of the FSM and after the PreUpdate
		public virtual void PostUpdateContext() { } // runs only when this context is the current context of the FSM and after Update
		public virtual void FixedUpdateContext() { } // runs only when this context is the current context of the FSM.
	}
}