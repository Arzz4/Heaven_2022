using UnityEngine;
using GameplayUtility;
using InControl;

namespace PlayerPlatformer2D
{
	public class PlayerInputRuntimeData
	{
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
			public bool[] buttonHoldRaw = null;
		}

		public PlayerInputBindings inputBindings = null;
		public InControlGameInputActions inControlGameInputActions = null;
		public FrameInput frameInput = new FrameInput();
		public int numberOfSingleButtonActions = 0;
		public int numberOfComboButtonActions = 0;
		public int numberOfButtonActions = 0;
		public bool inputEnabled = false;
	}

	public class PlayerInput : MonoBehaviour
	{
		[SerializeField]
		private PlayerRuntimeData m_RuntimeData = default;

		public void Initialize()
		{
			var data = m_RuntimeData.PlayerInputRuntimeData;

			// initialize in control and bindings
			data.inControlGameInputActions = new InControlGameInputActions();

			// axis bindings
			data.inControlGameInputActions.left.AddDefaultBinding(Key.LeftArrow);
			data.inControlGameInputActions.left.AddDefaultBinding(Key.A);
			data.inControlGameInputActions.left.AddDefaultBinding(InputControlType.DPadLeft);
			data.inControlGameInputActions.left.AddDefaultBinding(InputControlType.LeftStickLeft);

			data.inControlGameInputActions.right.AddDefaultBinding(Key.RightArrow);
			data.inControlGameInputActions.right.AddDefaultBinding(Key.D);
			data.inControlGameInputActions.right.AddDefaultBinding(InputControlType.DPadRight);
			data.inControlGameInputActions.right.AddDefaultBinding(InputControlType.LeftStickRight);

			data.inControlGameInputActions.up.AddDefaultBinding(Key.UpArrow);
			data.inControlGameInputActions.up.AddDefaultBinding(Key.W);
			data.inControlGameInputActions.up.AddDefaultBinding(InputControlType.DPadUp);
			data.inControlGameInputActions.up.AddDefaultBinding(InputControlType.LeftStickUp);

			data.inControlGameInputActions.down.AddDefaultBinding(Key.DownArrow);
			data.inControlGameInputActions.down.AddDefaultBinding(Key.S);
			data.inControlGameInputActions.down.AddDefaultBinding(InputControlType.DPadDown);
			data.inControlGameInputActions.down.AddDefaultBinding(InputControlType.LeftStickDown);

			// number of actions
			int numberOfSingleButtonActions = System.Enum.GetValues(typeof(GameActionSingleInputType)).Length;
			int numberOfComboButtonActions = System.Enum.GetValues(typeof(GameActionMultipleInputType)).Length;
			int numberOfButtonActions = numberOfSingleButtonActions + numberOfComboButtonActions;

			// single buttons bindings
			for (int i = 0; i < data.inputBindings.singleActions.Length; ++i)
			{
				var buttonAction = data.inputBindings.singleActions[i];
				int actionIndex = (int)buttonAction.action;

				data.inControlGameInputActions.singleButtonActions[actionIndex].AddDefaultBinding(buttonAction.gamepadKey);
				data.inControlGameInputActions.singleButtonActions[actionIndex].AddDefaultBinding(buttonAction.keyboardKey);
			}

			// combo button bindings
			for (int i = 0; i < data.inputBindings.comboActions.Length; ++i)
			{
				var buttonComboAction = data.inputBindings.comboActions[i];
				int actionIndex = (int)buttonComboAction.action - numberOfSingleButtonActions;

				data.inControlGameInputActions.comboButtonActions[actionIndex].comboActions[0].AddDefaultBinding(buttonComboAction.gamepadKey1);
				data.inControlGameInputActions.comboButtonActions[actionIndex].comboActions[0].AddDefaultBinding(buttonComboAction.keyboardKey1);

				data.inControlGameInputActions.comboButtonActions[actionIndex].comboActions[1].AddDefaultBinding(buttonComboAction.gamepadKey2);
				data.inControlGameInputActions.comboButtonActions[actionIndex].comboActions[1].AddDefaultBinding(buttonComboAction.keyboardKey2);
			}

			// buttons data
			data.numberOfSingleButtonActions = numberOfSingleButtonActions;
			data.numberOfComboButtonActions = numberOfComboButtonActions;
			data.numberOfButtonActions = numberOfButtonActions;

			data.frameInput.buttonPress = new bool[numberOfButtonActions];
			data.frameInput.buttonHoldRaw = new bool[numberOfButtonActions];
			for (int i = 0; i < numberOfButtonActions; ++i)
			{
				data.frameInput.buttonPress[i] = false;
				data.frameInput.buttonHoldRaw[i] = false;
			}
		}

		public void UpdateFrameInput()
		{
			var data = m_RuntimeData.PlayerInputRuntimeData;

			if (!data.inputEnabled)
				return;

			// axis
			float horizontal = data.inControlGameInputActions.horizontal.Value;
			float vertical = data.inControlGameInputActions.vertical.Value;
			data.frameInput.leftJoystickData.rawInput = new Vector2(horizontal, vertical);
			data.frameInput.leftJoystickData.magnitude = Mathf.Clamp01(data.frameInput.leftJoystickData.rawInput.magnitude);
			data.frameInput.leftJoystickData.normalizedInput = data.frameInput.leftJoystickData.rawInput.normalized;

			ApplyAngleDeadZones();

			// single buttons
			for (int i = 0; i < data.numberOfSingleButtonActions; ++i)
			{
				data.frameInput.buttonPress[i] = data.inControlGameInputActions.singleButtonActions[i].WasPressed;
				data.frameInput.buttonHoldRaw[i] = data.inControlGameInputActions.singleButtonActions[i].IsPressed;
			}

			// combo buttons
			for (int j = 0; j < data.numberOfComboButtonActions; ++j)
			{
				int i = data.numberOfSingleButtonActions + j;

				data.frameInput.buttonPress[i] = (data.inControlGameInputActions.comboButtonActions[j].comboActions[0].IsPressed && data.inControlGameInputActions.comboButtonActions[j].comboActions[1].WasPressed) ||
												 (data.inControlGameInputActions.comboButtonActions[j].comboActions[0].WasPressed && data.inControlGameInputActions.comboButtonActions[j].comboActions[1].IsPressed);

				data.frameInput.buttonHoldRaw[i] = (data.inControlGameInputActions.comboButtonActions[j].comboActions[0].IsPressed && data.inControlGameInputActions.comboButtonActions[j].comboActions[1].IsPressed);
			}
		}

		private void ApplyAngleDeadZones()
		{
			var leftJoystickData = m_RuntimeData.PlayerInputRuntimeData.frameInput.leftJoystickData;
			if (leftJoystickData.magnitude < Mathf.Epsilon)
				return;

			var inputBindings = m_RuntimeData.PlayerInputRuntimeData.inputBindings;
			if (Vector2.Angle(Vector2.down, leftJoystickData.normalizedInput) < inputBindings.downAngleDeadZone)
			{
				leftJoystickData.rawInput = Vector2.down;
				leftJoystickData.normalizedInput = Vector2.down;
				leftJoystickData.magnitude = 1.0f;
				return;
			}

			if (Vector2.Angle(Vector2.up, leftJoystickData.normalizedInput) < inputBindings.upAngleDeadZone)
			{
				leftJoystickData.rawInput = Vector2.up;
				leftJoystickData.normalizedInput = Vector2.up;
				leftJoystickData.magnitude = 1.0f;
				return;
			}
		}

		public void DisableInput()
		{
			var data = m_RuntimeData.PlayerInputRuntimeData;
			data.inputEnabled = false;

			data.frameInput.leftJoystickData.rawInput = Vector2.zero;
			data.frameInput.leftJoystickData.magnitude = 0.0f;
			data.frameInput.leftJoystickData.normalizedInput = Vector2.zero;

			int numberOfButtonInputs = System.Enum.GetValues(typeof(GameActionSingleInputType)).Length;
			for (int i = 0; i < numberOfButtonInputs; ++i)
			{
				data.frameInput.buttonPress[i] = false;
				data.frameInput.buttonHoldRaw[i] = false;
			}
		}

		public void EnableInput()
		{
			var data = m_RuntimeData.PlayerInputRuntimeData;
			data.inputEnabled = true;
		}
	}
}