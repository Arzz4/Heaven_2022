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

    private void Start()
    {
        endTrigger = FindObjectOfType<EndTrigger>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        endTrigger.LoadOtherScene(this);
    }
}
