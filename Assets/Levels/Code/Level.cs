using PlayerPlatformer2D;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Start is called before the first frame update

    private Grid grid;
    private PlayerCharacter player;
    private EndTrigger end;
    private TileLogic psxTiles;

    void Start()
    {
        grid = GetComponent<Grid>();
        psxTiles = GameObject.FindObjectOfType<TileLogic>();
        player = GameObject.FindObjectOfType<PlayerCharacter>();
        end = GameObject.FindObjectOfType<EndTrigger>();
    }
}
