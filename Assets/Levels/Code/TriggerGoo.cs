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
        Destroy(gameObject);
    }
}
