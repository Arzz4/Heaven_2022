using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerPlatformer2D
{
	public enum AxisInputType
	{
		Movement,
	}

	public enum ButtonInputType
	{
		Jump = 0,
	}

	[System.Serializable]
	public class AxisInputAction
	{
		public AxisInputType inputType;
		public InputAction inputAction;
	}

	[System.Serializable]
	public class ButtonInputAction
	{
		public ButtonInputType inputType;
		public InputAction inputAction;
	}

	[CreateAssetMenu(fileName = "Input Bindings", menuName = "Player Platformer 2D/Input/Input Bindings", order = 1)]
	public class PlayerInputBindings : ScriptableObject
	{
		[Header(" --- Axis --- ")]
		public AxisInputAction[] axisActions = default;

		[Header(" --- Buttons--- ")]
		public ButtonInputAction[] buttonActions = default;

		public float buttonHoldThreshold = 0.2f;
	}
}