using AudioSystems;
using System.Collections;
using UnityEngine;
using GameplayUtility;
using DG.Tweening;

public class DelayedDestroy : MonoBehaviour
{
    public float destroyAfterTime = 0.5f;
    public void Start()
    {
        StartCoroutine(killMe());
    }

    private IEnumerator killMe()
    {
        yield return new WaitForSeconds(destroyAfterTime);
        Destroy(gameObject);
    }
}