
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// later on the contents in this file are probably going to be replaced by Unity's new Input System or some other input system 

namespace GameplayUtility
{
	public enum GamePadButtonType
	{
		A_X,
		B_CIRCLE,
		X_SQUARE,
		Y_TRIANGLE,
		R_BUMPER_R1,
		R_TRIGGER_R2,
		L_BUMPER_L1,
		L_TRIGGER_L2,
		START_MENU,
		BACK,
		L_JOYSTICK_BUTTON,
		R_JOYSTICK_BUTTON
	}

	public enum GamePadButtonInteractionType
	{
		ButtonDown,
		ButtonUp,
		Released,
		Hold,
		AxisButtonPressed,
		AxisButtonHold,
		AxisButtonReleased
	}

	[System.Serializable]
	public class InputAction
	{
		public GamePadButtonType buttonType;
		public GamePadButtonInteractionType interactionType;
	}

	[System.Serializable]
	public class InputActionTuple
	{
		public InputAction action1;
		public InputAction action2;
	}

	public class InputSystem
	{
		private enum AxisButtonState
		{
			JustPressed,
			Hold,
			Released
		}

		// TODO: optimize by using an array with elements of a tuple (axis name, state)
		private static Dictionary<string, AxisButtonState> m_AxisButtons = new Dictionary<string, AxisButtonState>()
		{
			{ "L_TRIGGER_L2", AxisButtonState.Released },
			{ "R_TRIGGER_R2", AxisButtonState.Released },
		};

		public static void Update()
		{
			// NOTE: if adding more axis buttons, use a foreach on the dictionary, otherwise that would be overkill
			UpdateAxisButtonState(GamePadButtonType.L_TRIGGER_L2.ToString());
			UpdateAxisButtonState(GamePadButtonType.R_TRIGGER_R2.ToString());
		}

		private static void UpdateAxisButtonState(string aButtonString)
		{
			float axisValue = Input.GetAxis(aButtonString);

			if (axisValue <= 0.0f)
				m_AxisButtons[aButtonString] = AxisButtonState.Released;
			else
			{
				if (m_AxisButtons[aButtonString] == AxisButtonState.Released)
					m_AxisButtons[aButtonString] = AxisButtonState.JustPressed;
				else
					m_AxisButtons[aButtonString] = AxisButtonState.Hold;
			}
		}

		public static bool CheckForInput(GamePadButtonType aButtonType, GamePadButtonInteractionType anInteractionType)
		{
			// TODO: cache this to avoid creation of strings at runtime
			string buttonString = aButtonType.ToString();

			switch (anInteractionType)
			{
				case GamePadButtonInteractionType.ButtonDown:
					return Input.GetButtonDown(buttonString);
				case GamePadButtonInteractionType.ButtonUp:
					return Input.GetButtonUp(buttonString);
				case GamePadButtonInteractionType.Released:
					return !Input.GetButton(buttonString);
				case GamePadButtonInteractionType.Hold:
					return Input.GetButton(buttonString);
				case GamePadButtonInteractionType.AxisButtonPressed:
					return m_AxisButtons[buttonString] == AxisButtonState.JustPressed;
				case GamePadButtonInteractionType.AxisButtonHold:
					return m_AxisButtons[buttonString] == AxisButtonState.Hold;
				case GamePadButtonInteractionType.AxisButtonReleased:
					return m_AxisButtons[buttonString] == AxisButtonState.Released;
			}

			return false;
		}

		public static bool CheckForInput(InputAction anInputAction)
		{
			return CheckForInput(anInputAction.buttonType, anInputAction.interactionType);
		}

		public static bool CheckForInput(InputActionTuple anInputActionTuple)
		{
			return CheckForInput(anInputActionTuple.action1) && CheckForInput(anInputActionTuple.action2);
		}

		public static Vector2 GetRawLeftJoystickInput()
		{
			return new Vector2(Input.GetAxis("L_JOYSTICK_HORIZONTAL"), Input.GetAxis("L_JOYSTICK_VERTICAL"));
		}

		public static Vector2 GetRawRightJoystickInput()
		{
			return new Vector2(Input.GetAxis("R_JOYSTICK_HORIZONTAL"), Input.GetAxis("R_JOYSTICK_VERTICAL"));
		}

		public static Vector2 GetRawDPADInput()
		{
			return new Vector2(Input.GetAxis("DPAD_HORIZONTAL"), Input.GetAxis("DPAD_VERTICAL"));
		}
	}
}
