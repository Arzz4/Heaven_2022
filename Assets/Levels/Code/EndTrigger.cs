using PlayerPlatformer2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public bool triggered;
    public int nextSceneIndex;
   
    private PlayerCharacter player;
    private bool loadingNext;

    private void Start()
    {
        triggered = false;
        player = null;
        loadingNext = false;
    }

    private void Update()
    {
        if (player != null && !player.isActiveAndEnabled)
        {
            StartNextScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            this.player = player;  
            triggered = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            if (!player.isActiveAndEnabled)
            {
                StartNextScene();
            }
            else
            {
                this.player = null;
                triggered = false;
            }
        }
    }

    public void resetCurrentLevel()
    {
        if (!loadingNext)
        {
            loadingNext =true;
            Debug.Log("Reset current level " + this.GetInstanceID());
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void StartNextScene()
    {
        if (!loadingNext)
        {
            loadingNext = true;
            Debug.Log("Loading next level: " +nextSceneIndex + "  " + this.GetInstanceID());
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
