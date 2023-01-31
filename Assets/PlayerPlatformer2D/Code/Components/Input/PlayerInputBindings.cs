using InControl;
using InControl.UnityDeviceProfiles;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public enum GameActionSingleInputType
	{
		Jump = 0,
		KillCharacter,
		TogglePauseMenu
	}

	public enum GameActionMultipleInputType
	{
		KillAllCharacters = 3,
	}

	[System.Serializable]
	public class InControlActionInputBindingSingle
	{
		public GameActionSingleInputType action;
		public InControl.Key keyboardKey = InControl.Key.None;
		public InControl.InputControlType gamepadKey = InControl.InputControlType.None;
	}

	[System.Serializable]
	public class InControlActionInputBindingMultiple
	{
		public GameActionMultipleInputType action;
		public InControl.Key keyboardKey1 = InControl.Key.None;
		public InControl.Key keyboardKey2 = InControl.Key.None;
		public InControl.InputControlType gamepadKey1 = InControl.InputControlType.None;
		public InControl.InputControlType gamepadKey2 = InControl.InputControlType.None;
	}

	[System.Serializable]
	public class PlayerComboAction
	{
		public InControl.PlayerAction[] comboActions;
	}

	public class InControlGameInputActions : PlayerActionSet
	{
		// axis
		public InControl.PlayerAction left;
		public InControl.PlayerAction right;
		public InControl.PlayerAction up;
		public InControl.PlayerAction down;
		public InControl.PlayerOneAxisAction horizontal;
		public InControl.PlayerOneAxisAction vertical;

		// single buttons
		public InControl.PlayerAction[] singleButtonActions;

		// combo buttons
		public PlayerComboAction[] comboButtonActions;

		public InControlGameInputActions()
		{
			// axis
			left		= CreatePlayerAction("left");
			right		= CreatePlayerAction("right");
			up			= CreatePlayerAction("up");
			down		= CreatePlayerAction("down");
			horizontal	= CreateOneAxisPlayerAction(left, right);
			vertical	= CreateOneAxisPlayerAction(down, up);

			// single actions
			int numberOfSingleButtonActions = System.Enum.GetValues(typeof(GameActionSingleInputType)).Length;
			singleButtonActions = new PlayerAction[numberOfSingleButtonActions];
			for (int i = 0; i < numberOfSingleButtonActions; ++i)
				singleButtonActions[i] = CreatePlayerAction(((GameActionSingleInputType)i).ToString());

			// combo actions
			int numberOfComboButtonActions = System.Enum.GetValues(typeof(GameActionMultipleInputType)).Length;
			comboButtonActions = new PlayerComboAction[numberOfComboButtonActions];
			for(int i = 0; i < numberOfComboButtonActions; ++i)
			{
				comboButtonActions[i] = new PlayerComboAction();
				comboButtonActions[i].comboActions = new PlayerAction[2];
				for (int j = 0; j < comboButtonActions[i].comboActions.Length; ++j)
					comboButtonActions[i].comboActions[j] = CreatePlayerAction(((GameActionMultipleInputType)i).ToString() + "_" + j);
			}
		}
	}

	[CreateAssetMenu(fileName = "Input Bindings", menuName = "Player Platformer 2D/Input/Input Bindings", order = 1)]
	public class PlayerInputBindings : ScriptableObject
	{
		[Header(" --- Base Actions --- ")]
		public InControlActionInputBindingSingle[] singleActions = default;
		public InControlActionInputBindingMultiple[] comboActions = default;

		[Header(" --- DeadZones --- ")]

		[Range(0.0f, 359.0f)]
		public float downAngleDeadZone = 10.0f;

		[Range(0.0f, 359.0f)]
		public float upAngleDeadZone = 10.0f;
	}
}