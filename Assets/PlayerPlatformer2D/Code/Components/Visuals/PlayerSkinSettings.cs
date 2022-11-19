using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	[System.Serializable]
	public class Skins
	{
		public Sprite[] sprites;
	}

	[CreateAssetMenu(fileName = "Skin Settings", menuName = "Player Platformer 2D/Visuals/Skin Settings")]
	public class PlayerSkinSettings : ScriptableObject
	{
		public string referenceSpriteName = "Character_Matter_Sheet_";
		public Skins[] skins;
	}
}