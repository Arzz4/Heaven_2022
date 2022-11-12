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

	public class PlayerVisuals : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer m_SpriteRenderer;

		[SerializeField]
		private Skins[] m_Skins;

		[SerializeField]
		private int m_SkinIndex;

		[SerializeField]
		private int m_SpriteIndex;

		void Awake()
		{
			SelectSkin();
		}

		void SelectSkin()
		{
			if (m_SpriteRenderer.sprite.name.Contains("HamsterMain"))
			{
				string spriteName = m_SpriteRenderer.sprite.name;
				spriteName = spriteName.Replace("HamsterMain_", "");
				m_SpriteIndex = int.Parse(spriteName);

				m_SpriteRenderer.sprite = m_Skins[m_SkinIndex].sprites[m_SpriteIndex];
			}
		}
	}
}