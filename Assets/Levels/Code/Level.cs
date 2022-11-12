using PlayerPlatformer2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    // Start is called before the first frame update

    private Grid grid;
    private PlayerCharacter player;
    private EndTrigger end;
    private Tilemap psxTiles;

    void Start()
    {
        grid = GetComponent<Grid>();
        player = GameObject.FindObjectOfType<PlayerCharacter>();
        end = GameObject.FindObjectOfType<EndTrigger>();

        foreach (var item in grid.GetComponentsInChildren<Tilemap>())
        {
            if (item.name.Contains("psx"))
            {
                psxTiles = item;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int local = psxTiles.WorldToCell(pos);
            psxTiles.SetTile(local, null);
        }
    }

    public bool isFinished()
    {
        return end.triggered;
    }
}
