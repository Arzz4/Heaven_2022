using AudioSystems;
using System.Collections;
using UnityEngine;

public class DelayedDestroy : MonoBehaviour
{
    public float destroyAfterTime = 0.5f;
    public void Start()
    {
        StartCoroutine(killMe());

        AudioManager manager = AudioManager.Instance;
        if (manager != null)
        {
            AudioDatabase db = manager.GetAudioDatabase();
            manager.PlayOnShotAudioOnVFXAudioSource(db.GetWetSplatVFX(Random.Range(0, db.GetNumberOfWetSplatVFX())));
        }
    }

    private IEnumerator killMe()
    {
        yield return new WaitForSeconds(destroyAfterTime);
        Destroy(gameObject);
    }
}