
using GameplayUtility;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public enum AxisInputType
	{
		Movement,
	}

	public enum ButtonInputType
	{
		Jump = 0,
		KillCharacter,
		KillAllCharacters
	}

	[CreateAssetMenu(fileName = "Input Bindings", menuName = "Player Platformer 2D/Input/Input Bindings", order = 1)]
	public class PlayerInputBindings : ScriptableObject
	{
		[Header(" --- Base Actions --- ")]
		public InputAction jump = default;
		public InputAction killCharacter = default;
		public InputActionTuple KillAllCharacters = default;

		[Header(" --- DeadZones --- ")]

		[Range(0.0f, 359.0f)]
		public float downAngleDeadZone = 10.0f;

		[Range(0.0f, 359.0f)]
		public float upAngleDeadZone = 10.0f;
	}
}