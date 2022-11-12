using PlayerPlatformer2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    // Start is called before the first frame update
    public float explosionRadius = 2.5f;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            psxTiles.RemoveTiles(explosionRadius,pos);
        }
    }


    public bool isFinished()
    {
        return end.triggered;
    }
}
