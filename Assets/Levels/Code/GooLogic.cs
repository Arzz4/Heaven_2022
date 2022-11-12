using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;
using Tile = UnityEngine.Tilemaps.Tile;

public class GooLogic : MonoBehaviour
{

    private Tilemap tiles;
    public Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        tiles = GetComponent<Tilemap>();
    }

    public void createGoo(Vector3 worldPosition)
    {
        Vector3Int local = tiles.WorldToCell(worldPosition);
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        tiles.SetTile(local, tile);
        tiles.RefreshTile(local);
    }
}
