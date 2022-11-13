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
			public bool[] buttonHoldRaw = null;
		}

		public FrameInput frameInput = new FrameInput();

		public bool inputEnabled = false;
		public float lastTimeJumpButtonPress = 0.0f;
	}

	public class PlayerInput : MonoBehaviour
	{
		[SerializeField]
		private PlayerRuntimeData m_RuntimeData = default;

		public void Initialize()
		{
			var data = m_RuntimeData.PlayerInputRuntimeData;

			// buttons
			int numberOfButtonInputs = System.Enum.GetValues(typeof(ButtonInputType)).Length;

			// initialize all frame button inputs 
			data.frameInput.buttonPress = new bool[numberOfButtonInputs];
			data.frameInput.buttonHoldRaw = new bool[numberOfButtonInputs];
			for (int i = 0; i < numberOfButtonInputs; ++i)
			{
				data.frameInput.buttonPress[i] = false;
				data.frameInput.buttonHoldRaw[i] = false;
			}

			EnableInput();
		}

		public void UpdateFrameInput()
		{
			InputSystem.Update();

			var data = m_RuntimeData.PlayerInputRuntimeData;
			var inputBindings = data.inputBindings;

			// axis
			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");
			data.frameInput.leftJoystickData.rawInput = new Vector2(horizontal, vertical);
			
			if(data.frameInput.leftJoystickData.rawInput.magnitude < Mathf.Epsilon)
				data.frameInput.leftJoystickData.rawInput = InputSystem.GetRawLeftJoystickInput();

			data.frameInput.leftJoystickData.magnitude = Mathf.Clamp01(data.frameInput.leftJoystickData.rawInput.magnitude);
			data.frameInput.leftJoystickData.normalizedInput = data.frameInput.leftJoystickData.rawInput.normalized;

			ApplyAngleDeadZones();

			// buttons
			data.frameInput.buttonPress[(int)ButtonInputType.Jump]				= Input.GetKeyDown(KeyCode.Space);
			data.frameInput.buttonPress[(int)ButtonInputType.KillCharacter]		= Input.GetKeyDown(KeyCode.LeftShift);

			if (data.frameInput.buttonPress[(int)ButtonInputType.Jump])
				data.lastTimeJumpButtonPress = Time.time;

			data.frameInput.buttonHoldRaw[(int)ButtonInputType.Jump]			= (Time.time - data.lastTimeJumpButtonPress < 5.0f) || Input.GetKey(KeyCode.Space);
			data.frameInput.buttonHoldRaw[(int)ButtonInputType.KillCharacter]	= Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift);

			if (!data.frameInput.buttonPress[(int)ButtonInputType.Jump])
			{
				data.frameInput.buttonPress[(int)ButtonInputType.Jump]				= InputSystem.CheckForInput(inputBindings.jump);
				data.frameInput.buttonHoldRaw[(int)ButtonInputType.Jump]			= InputSystem.CheckForInput(inputBindings.jump.buttonType, GamePadButtonInteractionType.Hold);
			}

			if(!data.frameInput.buttonPress[(int)ButtonInputType.KillCharacter])
			{
				data.frameInput.buttonPress[(int)ButtonInputType.KillCharacter]		= InputSystem.CheckForInput(inputBindings.killCharacter);
				data.frameInput.buttonHoldRaw[(int)ButtonInputType.KillCharacter]	= InputSystem.CheckForInput(inputBindings.killCharacter.buttonType, GamePadButtonInteractionType.Hold);
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

			int numberOfButtonInputs = System.Enum.GetValues(typeof(ButtonInputType)).Length;
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