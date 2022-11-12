using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public bool triggered;

    private void Start()
    {
        triggered = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggered = true;
    }
}
