using UnityEngine.Tilemaps;
using UnityEngine;
using PlayerPlatformer2D;
using GameplayUtility;
using System.Collections.Generic;
using System.Collections;

public class HazardLogic : MonoBehaviour
{
    private Tilemap tiles;

    public float crumbleDelay = 1.0f;
    private Vector3 prefabOffset = new Vector3(0.5f, 0.5f, 0.5f);
    public GameObject crumblePrefab;

    public GameObject hazardPrefab;
    private HashSet<Vector3> crumbling;
    private TileLogic tileLogic;
    void Start()
    {
        tiles = GetComponent<Tilemap>();
        crumbling = new HashSet<Vector3>();
        tileLogic = GameObject.FindObjectOfType<TileLogic>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hazard(collision.gameObject);
        var obj = collision.gameObject;
        if (obj.GetComponent<PlayerCharacter>() != null)
        {
            hazard(collision.gameObject);
        }
    }

    private void hazard(GameObject obj)
    {
        if (obj.activeSelf)
        {
            obj.SetActive(false);
            ObjectPoolSystem.Instance.InstantiatePrefabWith(hazardPrefab, obj.transform.position, Quaternion.identity);
        }
    }

    private void crumble(Vector3 worldPosition)
    {
        Vector3Int pos = tiles.WorldToCell(worldPosition);
        if (crumbling.Add(pos))
        {
            StartCoroutine(doCrumble(pos));
        }
    }

    private IEnumerator doCrumble(Vector3 worldPosition)
    {
        yield return new WaitForSeconds(crumbleDelay);
        tileLogic.removeTilesAt(worldPosition);
        Vector3Int pos = tiles.WorldToCell(worldPosition);
        ObjectPoolSystem.Instance.InstantiatePrefabWith(crumblePrefab, tiles.CellToWorld(pos) + prefabOffset, Quaternion.identity);
        crumbling.Remove(pos);
    }
}
