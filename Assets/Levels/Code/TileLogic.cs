using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileLogic : MonoBehaviour
{
    private Tilemap tiles;
    // Start is called before the first frame update
    void Start()
    {
        tiles = GetComponent<Tilemap>();
    }
        // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveTiles(float r, Vector3 pos)
    {
        float cellSizeWorld = tiles.CellToWorld(Vector3Int.right).x;
        int check = (int)Mathf.Ceil(r/ cellSizeWorld);
        Vector3Int local = tiles.WorldToCell(pos);

        for (int x = -check; x <= check; x++)
        {
            for (int y = -check; y <= check; y++)
            {
                float distSq = x * x + y * y;
                if (distSq < r * r)
                {
                    tiles.SetTile(local + new Vector3Int(x, y, 0), null);
                }
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //Vector3Int local = tiles.WorldToCell(collision.contacts[0].point - new Vector2(0,0.3f));
        //tiles.SetTileFlags(local, TileFlags.None);
        //tiles.SetColor(local, Color.red);
        //Debug.Log(local);
    }


}
