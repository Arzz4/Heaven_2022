using UnityEngine;
using GameplayUtility;

namespace PlayerPlatformer2D
{
	public class PlayerInputRuntimeData
	{
		public PlayerInputBindings inputBindings = null;

		public class JoystickData
		{
			public Vector2 rawInput;
			public Vector2 normalizedInput;
			public float magnitude;
		}

		public class FrameInput
		{
			// axis/joystick data
			public JoystickData leftJoystickData = new JoystickData();

			// buttons
			public bool[] buttonPress = null;
			public bool[] buttonHoldWithThreshold = null;
			public bool[] buttonHoldRaw = null;
		}

		public FrameInput frameInput = new FrameInput();
	}

	public class PlayerInput : MonoBehaviour
	{
		[SerializeField]
		private PlayerRuntimeData m_RuntimeData = default;

		private InputActionAxisProcessor m_AxisProcessor;
		private InputActionProcessor m_ButtonsProcessor;

		public void Initialize()
		{
			var data = m_RuntimeData.PlayerInputRuntimeData;
			var inputBindings = data.inputBindings;

			#region AXIS

			// axis
			int numberOfAxisInputs = System.Enum.GetValues(typeof(AxisInputType)).Length;

			// processor
			m_AxisProcessor = new InputActionAxisProcessor();
			m_AxisProcessor.Initialize(numberOfAxisInputs);

			for (int i = 0, e = inputBindings.axisActions.Length; i < e; ++i)
			{
				// bind
				var actionDef = inputBindings.axisActions[i];
				m_AxisProcessor.BindInputAction((int)actionDef.inputType, actionDef.inputAction);
			}

			#endregion

			#region BUTTONS

			// buttons
			int numberOfButtonInputs = System.Enum.GetValues(typeof(ButtonInputType)).Length;

			// processor
			m_ButtonsProcessor = new InputActionProcessor();
			m_ButtonsProcessor.Initialize(numberOfButtonInputs);
			for (int i = 0, e = inputBindings.buttonActions.Length; i < e; ++i)
			{
				// bind
				var actionDef = inputBindings.buttonActions[i];
				m_ButtonsProcessor.BindInputAction((int)actionDef.inputType, actionDef.inputAction);
			}

			// initialize all frame button inputs 
			data.frameInput.buttonPress = new bool[numberOfButtonInputs];
			data.frameInput.buttonHoldWithThreshold = new bool[numberOfButtonInputs];
			data.frameInput.buttonHoldRaw = new bool[numberOfButtonInputs];
			for (int i = 0; i < numberOfButtonInputs; ++i)
			{
				data.frameInput.buttonPress[i] = false;
				data.frameInput.buttonHoldWithThreshold[i] = false;
				data.frameInput.buttonHoldRaw[i] = false;
			}

			#endregion

			EnableInput();
		}

		public void UpdateFrameInput()
		{
			// update processors
			m_AxisProcessor.Update();
			m_ButtonsProcessor.Update();

			// TODO: remove this copy to local, make processor accesible from the runtime data!
			var data = m_RuntimeData.PlayerInputRuntimeData;

			// axis
			data.frameInput.leftJoystickData.rawInput = m_AxisProcessor.GetAxisValue((int)AxisInputType.Movement);
			data.frameInput.leftJoystickData.magnitude = Mathf.Clamp01(data.frameInput.leftJoystickData.rawInput.magnitude);
			data.frameInput.leftJoystickData.normalizedInput = data.frameInput.leftJoystickData.rawInput.normalized;

			// buttons
			for (int i = 0, e = data.frameInput.buttonPress.Length; i < e; ++i)
			{
				data.frameInput.buttonPress[i] = m_ButtonsProcessor.CheckInputIsTriggered(i);
				data.frameInput.buttonHoldWithThreshold[i] = m_ButtonsProcessor.CheckForInput(i, InputInteractionState.Hold, data.inputBindings.buttonHoldThreshold);
				data.frameInput.buttonHoldRaw[i] = m_ButtonsProcessor.CheckForInput(i, InputInteractionState.Hold, 0.0f);
			}
		}

		public void DisableInput()
		{
			m_AxisProcessor.Reset();
			m_AxisProcessor.Disable();

			m_ButtonsProcessor.Reset();
			m_ButtonsProcessor.Disable();
		}

		public void EnableInput()
		{
			m_AxisProcessor.Enable();
			m_ButtonsProcessor.Enable();
		}
	}
}