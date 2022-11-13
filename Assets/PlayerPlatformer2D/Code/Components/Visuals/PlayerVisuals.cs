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
		private string m_ReferenceSpriteName = "Character_Matter_Sheet_";

		void LateUpdate()
		{
			SelectSkin();
		}

		void SelectSkin()
		{
			if (m_SpriteRenderer.sprite.name.Contains(m_ReferenceSpriteName))
			{
				string spriteName = m_SpriteRenderer.sprite.name;
				spriteName = spriteName.Replace(m_ReferenceSpriteName, "");
				int spriteIndex = int.Parse(spriteName);

				m_SpriteRenderer.sprite = m_Skins[m_SkinIndex].sprites[spriteIndex];
			}
		}
	}
}