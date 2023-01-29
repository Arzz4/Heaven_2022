using AudioSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class MuteButton : MonoBehaviour
{
    private Button button;
    private AudioManager audioManager;
    private string key = "GameSoundState";
    private Image img;
    public Sprite spriteMuted;
    public Sprite spriteUnmuted;

    private void Start()
    {
        button = GetComponent<Button>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        img = GetComponent<Image>();
        button.onClick.AddListener(OnButtonClick);
        setState(PlayerPrefs.GetInt(key, 1) == 0);
    }

    public void OnButtonClick()
    {
        setState(!audioManager.IsMuted());
    }

    private void setState(bool muted)
    {
        img.sprite = muted ? spriteMuted : spriteUnmuted;
        audioManager.SetSoundMuted(muted);
        PlayerPrefs.SetInt(key, muted ? 0 : 1);
        PlayerPrefs.Save();
    }
}
