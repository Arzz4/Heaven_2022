using AudioSystems;
using UnityEngine;

public class TriggerGoo : MonoBehaviour
{
    TileLogic tileLogic;
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

		Destroy(gameObject);
    }
}
