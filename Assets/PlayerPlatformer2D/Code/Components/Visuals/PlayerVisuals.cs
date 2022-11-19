using UnityEngine;

namespace PlayerPlatformer2D
{
	public class PlayerVisuals : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer m_SpriteRenderer;

		[SerializeField]
		private PlayerSkinSettings m_SkinSettings; 

		[SerializeField]
		private int m_SkinIndex;

		void LateUpdate()
		{
			SelectSkin();
		}

		void SelectSkin()
		{
			if (m_SpriteRenderer.sprite.name.Contains(m_SkinSettings.referenceSpriteName))
			{
				string spriteName = m_SpriteRenderer.sprite.name;
				spriteName = spriteName.Replace(m_SkinSettings.referenceSpriteName, "");
				int spriteIndex = int.Parse(spriteName);

				m_SpriteRenderer.sprite = m_SkinSettings.skins[m_SkinIndex].sprites[spriteIndex];
			}
		}
	}
}