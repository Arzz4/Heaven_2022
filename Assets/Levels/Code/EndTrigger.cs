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

    private void Start()
    {
        triggered = false;
    }

    public void resetCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartNextScene(nextSceneIndex);
        triggered = true;
    }

    public void StartNextScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
