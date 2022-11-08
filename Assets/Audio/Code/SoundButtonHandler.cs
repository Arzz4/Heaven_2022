using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AudioSystems
{
	public class SoundButtonHandler : MonoBehaviour
	{
		public Image buttonSoundImage;
		public TMPro.TextMeshProUGUI audioText;
		public Color mutedColor;
		public Color unMutedColor;

		public void Start()
		{
			UpdateState();
		}

		public void ToggleSound()
		{
			AudioManager.Instance.ToggleSoundState();
			UpdateState();
		}

		private void UpdateState()
		{
			if (AudioManager.Instance.IsSoundEnabled())
			{
				buttonSoundImage.color = unMutedColor;
				audioText.color = unMutedColor;
			}
			else
			{
				buttonSoundImage.color = mutedColor;
				audioText.color = mutedColor;
			}
		}
	}
}