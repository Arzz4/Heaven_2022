using AudioSystems;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class TriggerExplosion : MonoBehaviour
{
	// Start is called before the first frame update
	TileLogic tileLogic;
    public float destroyAfterTime = 0.2f;
    void Start()
	{
		// level destruction
		tileLogic = GameObject.FindObjectOfType<TileLogic>();
		if (tileLogic != null)
		{
			tileLogic.RemoveTiles(transform.position);
		}

		// audio
		AudioManager manager = AudioManager.Instance;
		if (manager != null)
		{
			AudioDatabase db = manager.GetAudioDatabase();
			manager.PlayOnShotAudioOnVFXAudioSource(db.PlayerExplodeVFX);
		}

		// camera vfx
		Camera cam = Camera.main;
		if (cam != null)
		{
			Transform camTransform = cam.transform;
			
			// cam shake
			camTransform.DOComplete();
			camTransform.DOShakePosition(.2f, .5f, 14, 90, false, true);
			
			// vfx
			var rippleEffect = camTransform.GetComponent<RipplePostProcessor>();
			if (rippleEffect != null)
				rippleEffect.Ripple(cam.WorldToScreenPoint(transform.position));
		}

		// make sure to destroy this object afterwards
		StartCoroutine(killMe());
	}

	private IEnumerator killMe()
	{
		yield return new WaitForSeconds(destroyAfterTime);
		Destroy(gameObject);
	}
}
