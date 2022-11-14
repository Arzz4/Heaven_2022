using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = UnityEngine.Tilemaps.Tile;

public class SprayLogic : MonoBehaviour
{
    private Tilemap tiles;
    public Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        tiles = GetComponent<Tilemap>();
    }

    public void createTiles(Vector3 worldPosition, float rot)
    {
        Vector3Int local = tiles.WorldToCell(worldPosition);
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        tile.transform = Matrix4x4.Rotate(Quaternion.Euler(new Vector3(0,0,rot)));
        tiles.SetTile(local, tile);
        tiles.RefreshTile(local);
    }

    public void clean(Vector3 worldPosition)
    {
        Vector3Int local = tiles.WorldToCell(worldPosition);
        tiles.SetTile(local, null);
    }
}
