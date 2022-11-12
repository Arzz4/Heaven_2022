using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    TileLogic tileLogic;
    void Start()
    {
        tileLogic = GameObject.FindObjectOfType<TileLogic>();
        if (tileLogic != null)
        {
            tileLogic.RemoveTiles(transform.position);
        }

        StartCoroutine(killMe());
    }

    private IEnumerator killMe()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
