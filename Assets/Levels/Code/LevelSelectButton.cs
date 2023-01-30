using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using AudioSystems;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using InterfaceMovement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using TMPro;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField]
    public int m_sceneIndex;

    private EndTrigger endTrigger;
    private Button button;

    public Color locked = new Color(154.0f / 255.0f, 79.0f / 255.0f, 80.0f / 255.0f);
    public Color unlocked = new Color(110.0f/255.0f, 170.0f / 255.0f, 120.0f / 255.0f);

    private void Start()
    {
        endTrigger = FindObjectOfType<EndTrigger>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        transform.GetChild(0).GetComponent<Image>().color = PlayerPrefs.HasKey(EndTrigger.LevelCompletedKey(m_sceneIndex)) ? unlocked : locked;
    }

    public void OnButtonClick()
    {
        endTrigger.LoadOtherScene(this);
    }
}
