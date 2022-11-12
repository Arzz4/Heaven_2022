using GameplayUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI.Table;

public class TileLogic : MonoBehaviour
{
    private Tilemap tiles;
    private GooLogic gooLogic;
    public GameObject explosionPrefab;
    private Vector3 prefabOffset = new Vector3(0.5f, 0.5f, 0.5f);

    public float explosionRadius = 2.5f;
    public List<int> destructableLayers = new List<int>();
    public float gooRadius = 2.5f;

    private List<Tilemap> mapsToCleanOnExplosion;
    // Start is called before the first frame update
    void Start()
    {
        tiles = GetComponent<Tilemap>();
        gooLogic = GameObject.FindObjectOfType<GooLogic>();

        mapsToCleanOnExplosion = new List<Tilemap>();
        foreach (var tilemap in GameObject.FindObjectsOfType<Tilemap>())
        {
            if (destructableLayers.Contains(tilemap.GetComponent<TilemapRenderer>().sortingOrder))
            {
                mapsToCleanOnExplosion.Add(tilemap);
            }
        }

    }
        // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveTiles(Vector3 pos)
    {
        StartCoroutine(removeTiles(getTiles(explosionRadius, pos, 0.25f)));
    }

    public void TintTiles(Vector3 pos)
    {
        var possibleTiles = getTiles(gooRadius, pos, 0.4f);
        Vector3Int local = this.tiles.WorldToCell(pos);
        var toColor = new List<Tuple<float, Vector3Int>>();


        foreach (var tile in possibleTiles)
        {
            var tilePos = tile.Item2;
            if (tiles.HasTile(tilePos) && hasFacingAirContact(tilePos, local))
            {
                toColor.Add(tile);
            }
        }
        StartCoroutine(tintTiles(toColor, pos));
    }

    private bool hasFacingAirContact(Vector3Int p, Vector3Int origin)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3Int dir = new Vector3Int(i, j);
                Vector3Int airPos = p + dir;
                
                if (!tiles.HasTile(airPos) && Vector3.Dot(Vector3.Normalize(dir), Vector3.Normalize(origin - p)) > 0.4)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private IEnumerator removeTiles(List<Tuple<float, Vector3Int>> toDelete)
    {
        float prevDelay = 0;
        foreach (var item in toDelete)
        {
            var delay = item.Item1;
            var pos = item.Item2;
            float wait = delay - prevDelay;
            yield return new WaitForSeconds(wait);
            if (tiles.HasTile(pos))
            {
                ObjectPoolSystem.Instance.InstantiatePrefabWith(explosionPrefab, tiles.CellToWorld(pos) + prefabOffset, Quaternion.identity);
            }
            Vector3 worldPos = tiles.CellToWorld(pos);
            foreach (var tileMapToClean in mapsToCleanOnExplosion)
            {
                Vector3Int local = tileMapToClean.WorldToCell(worldPos);
                if (tileMapToClean.HasTile(pos))
                {
                    tileMapToClean.SetTile(pos, null);
                }
            }
            prevDelay = delay;
        }
    }

    private IEnumerator tintTiles(List<Tuple<float, Vector3Int>> toTint, Vector3 origin)
    {
        float prevDelay = 0;
        foreach (var item in toTint)
        {
            var delay = item.Item1;
            var pos = item.Item2;
            float wait = delay - prevDelay;
            yield return new WaitForSeconds(wait);
            if (tiles.HasTile(pos))
            {
                Vector3 world = tiles.LocalToWorld(pos);
                gooLogic.createGoo(world, origin-world);
            }
            prevDelay = delay;
        }
    }

    List<Tuple<float, Vector3Int>> getTiles(float r, Vector3 pos, float delayMax)
    {
        float cellSizeWorld = this.tiles.CellToWorld(Vector3Int.right).x;
        int check = (int)Mathf.Ceil(r / cellSizeWorld);
        Vector3Int local = this.tiles.WorldToCell(pos);
        var tiles = new List<Tuple<float, Vector3Int>>();
        float rsq = r * r;
        for (int x = -check; x <= check; x++)
        {
            for (int y = -check; y <= check; y++)
            {
                float distSq = x * x + y * y;
                if (distSq < rsq)
                {
                    float delay = delayMax * distSq / rsq;
                    tiles.Add(new Tuple<float, Vector3Int>(delay, local + new Vector3Int(x, y, 0)));
                }
            }
        }
        tiles.Sort((x, y) => x.Item1.CompareTo(y.Item1));
        return tiles;
    }
}
