
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

// later on the contents in this file are probably going to be replaced by some other input system 

namespace GameplayUtility
{
	public enum InputInteractionState
	{
		JustPressed,
		JustReleased,
		Released,
		Hold,
	}

	public class InputActionProcessor
	{
		private struct InputActionStateData
		{
			public InputInteractionState currentState;
			public float timestamp;
			public bool isTriggered;
		}

		// one per entry on the GamePadButtonType, increase accordingly
		private int m_ActionsCount = 0;
		private InputAction[] m_UnityInputActions;
		private InputActionStateData[] m_ActionStates;

		public void Initialize(int actionsCount)
		{
			m_ActionsCount		= actionsCount;
			m_UnityInputActions = new InputAction[m_ActionsCount];
			m_ActionStates		= new InputActionStateData[m_ActionsCount];

			for(int i = 0; i < m_ActionsCount; i++)
			{
				m_UnityInputActions[i]			= null;
				m_ActionStates[i].currentState	= InputInteractionState.Released;
				m_ActionStates[i].timestamp		= 0.0f;
				m_ActionStates[i].isTriggered	= false;
			}
		}

		public void BindInputAction(int anActionIndex, InputAction anUnityInputAction)
		{
			m_UnityInputActions[anActionIndex] = anUnityInputAction;
		}

		public void Update()
		{
			for(int i = 0; i < m_ActionsCount; ++i)
				UpdateInputActionState(i);
		}
	
		public bool CheckForInput(int anActionTypeIndex, InputInteractionState anStateType)
		{
			return m_ActionStates[anActionTypeIndex].currentState == anStateType;
		}

		public bool CheckForInput(int anActionTypeIndex, InputInteractionState anStateType, float aTresholdTime)
		{
			return m_ActionStates[anActionTypeIndex].currentState == anStateType && Time.time - m_ActionStates[anActionTypeIndex].timestamp > aTresholdTime;
		}

		public bool CheckInputIsTriggered(int anActionTypeIndex)
		{
			return m_ActionStates[anActionTypeIndex].isTriggered;
		}

		private void UpdateInputActionState(int anActionIndex)
		{
			bool isActionPressed   = m_UnityInputActions[anActionIndex].IsPressed();
			bool isActionTriggered = m_UnityInputActions[anActionIndex].triggered;
			UpdateActionStateChangeLogic(anActionIndex, isActionPressed, isActionTriggered);
		}

		private void UpdateActionStateChangeLogic(int anActionIndex, bool isActionPressed, bool isActionTriggered)
		{
			m_ActionStates[anActionIndex].isTriggered = isActionTriggered;

			if (!isActionPressed)
			{
				if (m_ActionStates[anActionIndex].currentState == InputInteractionState.Hold || m_ActionStates[anActionIndex].currentState == InputInteractionState.JustPressed)
				{
					m_ActionStates[anActionIndex].currentState = InputInteractionState.JustReleased;
					m_ActionStates[anActionIndex].timestamp = Time.time;
				}
				else if (m_ActionStates[anActionIndex].currentState != InputInteractionState.Released)
				{
					m_ActionStates[anActionIndex].currentState = InputInteractionState.Released;
					m_ActionStates[anActionIndex].timestamp = Time.time;
				}
			}
			else
			{
				if (m_ActionStates[anActionIndex].currentState == InputInteractionState.Released)
				{
					m_ActionStates[anActionIndex].currentState = InputInteractionState.JustPressed;
					m_ActionStates[anActionIndex].timestamp = Time.time;
				}
				else if (m_ActionStates[anActionIndex].currentState == InputInteractionState.JustPressed)
				{
					m_ActionStates[anActionIndex].currentState = InputInteractionState.Hold;
					m_ActionStates[anActionIndex].timestamp = Time.time;
				}
			}
		}

		public void Reset()
		{
			for (int i = 0; i < m_ActionsCount; ++i)
			{
				m_ActionStates[i].currentState = InputInteractionState.Released;
				m_ActionStates[i].timestamp = 0.0f;
				m_ActionStates[i].isTriggered = false;
			}
		}

		public void Disable()
		{
			for (int i = 0; i < m_ActionsCount; ++i)
				m_UnityInputActions[i].Disable();
		}

		public void Enable()
		{
			for (int i = 0; i < m_ActionsCount; ++i)
				m_UnityInputActions[i].Enable();
		}
	}

	public class InputActionAxisProcessor
	{
		private int m_ActionsCount = 0;
		private InputAction[] m_UnityInputActions;
		private Vector2[] m_ActionValues;

		public void Initialize(int anActionsCount)
		{
			m_ActionsCount = anActionsCount;
			m_UnityInputActions = new InputAction[m_ActionsCount];
			m_ActionValues = new Vector2[m_ActionsCount];

			for (int i = 0; i < m_ActionsCount; i++)
			{
				m_UnityInputActions[i] = null;
				m_ActionValues[i] = Vector2.zero;
			}
		}

		public void BindInputAction(int anActionIndex, InputAction anUnityInputAction)
		{
			m_UnityInputActions[anActionIndex] = anUnityInputAction;
		}

		public void Update()
		{
			for (int i = 0; i < m_ActionsCount; ++i)
				m_ActionValues[i] = m_UnityInputActions[i].ReadValue<Vector2>();
		}

		public Vector2 GetAxisValue(int anActionIndex)
		{
			return m_ActionValues[anActionIndex];
		}

		public void Reset()
		{
			for (int i = 0; i < m_ActionsCount; ++i)
				m_ActionValues[i] = Vector2.zero;
		}

		public void Disable()
		{
			for (int i = 0; i < m_ActionsCount; ++i)
				m_UnityInputActions[i].Disable();
		}

		public void Enable()
		{
			for (int i = 0; i < m_ActionsCount; ++i)
				m_UnityInputActions[i].Enable();
		}
	}
}
