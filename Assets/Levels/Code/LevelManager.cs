using NaughtyAttributes.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update

    public int NumberOfLevels;

    private Level level;
    private int curretLevelIndex = 0;
    private bool loading;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(loadNextLevel(curretLevelIndex + 1));
    }

    // Update is called once per frame
    void Update()
    {
        if (level == null) return;

        if (level.isFinished() && !loading)
        {
            loading = true;
            StartCoroutine(loadNextLevel(curretLevelIndex + 1));
        }
    }

    private IEnumerator loadNextLevel(int index)
    {
        if (index <= NumberOfLevels)
        {
            Debug.Log("Loading: " + index);
            yield return SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
            level = GameObject.FindObjectOfType<Level>();
            curretLevelIndex = index;
            Debug.Log("Loaded");
            loading = false;
        }
        else
        {
            Debug.Log("Game done");
        }
    }
}
