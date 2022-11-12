using GameplayUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileExplosionAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject explosionPrefab;
    private float start;
    public float lifetime = 0.2f;
    private void OnEnable()
    {
        start = Time.time;
    }

    private void Update()
    {
        if ((start + lifetime) < Time.time)
        {
            ObjectPoolSystem.Instance.RecycleInstance(explosionPrefab);
        }
    }
}
