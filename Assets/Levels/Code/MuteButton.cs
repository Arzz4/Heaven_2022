using AudioSystems;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class MuteButton : MonoBehaviour
{
    private Button button;
    private AudioManager audio;
    private string key = "GameSoundState";

    private void Start()
    {
        button = GetComponent<Button>();
        audio = GameObject.FindObjectOfType<AudioManager>();
        button.onClick.AddListener(OnButtonClick);
        setState(PlayerPrefs.GetInt(key, 1) == 0);
    }

    public void OnButtonClick()
    {
        setState(!audio.IsMuted());
    }

    private void setState(bool muted)
    {
        audio.SetSoundMuted(muted);
        GetComponentInChildren<TextMeshProUGUI>().text = muted ? "Audio OFF" : "Audio ON";
        PlayerPrefs.SetInt(key, muted ? 0 : 1);
        PlayerPrefs.Save();
    }
}
