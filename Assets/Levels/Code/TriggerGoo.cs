using AudioSystems;
using System.Collections;
using UnityEngine;

public class TriggerGoo : MonoBehaviour
{
    TileLogic tileLogic;
    public float destroyAfterTime = 0.5f;
    void Start()
    {
        tileLogic = GameObject.FindObjectOfType<TileLogic>();
        if (tileLogic != null)
        {
            tileLogic.TintTiles(transform.position);
		}

		AudioManager manager = AudioManager.Instance;
		if (manager != null)
		{
			AudioDatabase db = manager.GetAudioDatabase();
			manager.PlayOnShotAudioOnVFXAudioSource(db.GetWetSplatVFX(Random.Range(0, db.GetNumberOfWetSplatVFX())));
		}

        StartCoroutine(killMe());
    }
    private IEnumerator killMe()
    {
        yield return new WaitForSeconds(destroyAfterTime);
        Destroy(gameObject);
    }
}
