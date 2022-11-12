using GameplayUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

public class TileLogic : MonoBehaviour
{
    private Tilemap tiles;
    public GameObject explosionPrefab;
    private Vector3 prefabOffset = new Vector3(0.5f, 0.5f, 0.5f);
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
        var toDelete = new List<Tuple<float, Vector3Int>>();
        float rsq = r * r;
        for (int x = -check; x <= check; x++)
        {
            for (int y = -check; y <= check; y++)
            {
                float distSq = x * x + y * y;
                if (distSq < rsq)
                {
                    float delay = 0.25f * distSq / rsq;
                    toDelete.Add(new Tuple<float, Vector3Int>(delay, local + new Vector3Int(x, y, 0)));
                }
            }
        }

        StartCoroutine(removeTiles(toDelete));
    }

    private IEnumerator removeTiles(List<Tuple<float, Vector3Int>> toDelete)
    {
        toDelete.Sort((x, y) => x.Item1.CompareTo(y.Item1));
        float prevDelay = 0;
        foreach (var item in toDelete)
        {
            var delay = item.Item1;
            var pos = item.Item2;
            float wait = delay - prevDelay;
            yield return new WaitForSeconds(wait);
            var currTile = tiles.GetTile(pos);
            if ( currTile != null)
            {
                ObjectPoolSystem.Instance.InstantiatePrefabWith(explosionPrefab, tiles.CellToWorld(pos) + prefabOffset, Quaternion.identity);
                tiles.SetTile(pos, null);
            }
            prevDelay = delay;
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
