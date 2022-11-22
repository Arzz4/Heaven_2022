using GameplayUtility;
using PlayerPlatformer2D;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class TileLogic : MonoBehaviour
{
    private Tilemap tiles;
    private GooLogic gooLogic;
    private SpeedLogic speedLogic;
    public GameObject explosionPrefab;
    private Vector3 prefabOffset = new Vector3(0.5f, 0.5f, 0.5f);

    public float explosionRadius = 2.5f;
    public List<int> destructableLayers = new List<int>();

    private Tilemap scorchTiles;
    public Sprite scorchSprite;

    public float gooRadius = 2.5f;

    public int numTilesToCreate = 3;
    public Sprite tileCreationSprite;

    public float speedyRadius = 2.5f;

    public float crumbleDelay = 1.0f;
    public GameObject crumblePrefab;

    public GameObject hazardPrefab;

    private List<Tilemap> mapsToCleanOnExplosion;
    private HashSet<Vector3> crumbling;
    // Start is called before the first frame update
    void Start()
    {
        tiles = GetComponent<Tilemap>();
        gooLogic = GameObject.FindObjectOfType<GooLogic>();
        speedLogic = GameObject.FindObjectOfType<SpeedLogic>();
        crumbling = new HashSet<Vector3>();

        mapsToCleanOnExplosion = new List<Tilemap>();
        int foreGround = -1;
        foreach (var tilemap in GameObject.FindObjectsOfType<Tilemap>())
        {
            int sortOrder = tilemap.GetComponent<TilemapRenderer>().sortingOrder;
            if (destructableLayers.Contains(sortOrder))
            {
                mapsToCleanOnExplosion.Add(tilemap);
                if (sortOrder > foreGround)
                {
                    foreGround = sortOrder;
                    scorchTiles = tilemap;
                }
            }
        }
    }
    // Update is called once per frame

    public void CreateTiles(Vector3 pos)
    {
        StartCoroutine(createTiles(pos));
    }

    public void RemoveTiles(Vector3 pos)
    {
        StartCoroutine(removeTiles(getTiles(explosionRadius, pos, 0.25f), pos));
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

    public void SpeedyTiles(Vector3 pos)
    {
        var possibleTiles = getTiles(speedyRadius, pos, 0.4f);
        Vector3Int local = this.tiles.WorldToCell(pos);
        var toSpeed = new List<Tuple<float, Vector3Int>>();


        foreach (var tile in possibleTiles)
        {
            var tilePos = tile.Item2;
            if (tiles.HasTile(tilePos) && hasFacingAirContact(tilePos, local))
            {
                toSpeed.Add(tile);
            }
        }
        StartCoroutine(speedTiles(toSpeed, pos));
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

    private IEnumerator removeTiles(List<Tuple<float, Vector3Int>> toDelete, Vector3 origin)
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
            removeTilesAt(tiles.CellToWorld(pos));

            scorchNearby(pos, origin);
            prevDelay = delay;
        }
    }

    private void removeTilesAt(Vector3 worldPos)
    {
        foreach (var tileMapToClean in mapsToCleanOnExplosion)
        {
            Vector3Int local = tileMapToClean.WorldToCell(worldPos);
            if (tileMapToClean.HasTile(local))
            {
                tileMapToClean.SetTile(local, null);
            }
        }
    }

    private void scorchNearby(Vector3Int pos, Vector3 origin)
    {
        scorchTile(pos, origin);
        scorchTile(pos + Vector3Int.right, origin);
        scorchTile(pos + Vector3Int.left, origin);
        scorchTile(pos + Vector3Int.up, origin);
        scorchTile(pos + Vector3Int.down, origin);
    }
    private void scorchTile(Vector3Int pos, Vector3 origin)
    {
        if (tiles.HasTile(pos))
        {
            Vector3 world = tiles.LocalToWorld(pos);
            float r = getRotation(pos, Vector3.Normalize(world - origin));
            if (r > -0.1f)
            {
                Vector3Int local = scorchTiles.WorldToCell(world);
                Tile tile = ScriptableObject.CreateInstance<Tile>();
                tile.sprite = scorchSprite;
                tile.transform = Matrix4x4.Rotate(Quaternion.Euler(new Vector3(0, 0, r)));
                scorchTiles.SetTile(local, tile);
                scorchTiles.RefreshTile(local);
            }
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
                float r = getRotation(pos, Vector3.Normalize(world - origin));
                if (r > -0.1f)
                {
                    gooLogic.createTiles(world, r);
                    speedLogic.clean(world);
                }
            }
            prevDelay = delay;
        }
    }

    private IEnumerator speedTiles(List<Tuple<float, Vector3Int>> toTint, Vector3 origin)
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
                float r = getRotation(pos, world - origin);
                if (r > -0.1f)
                {
                    speedLogic.createTiles(world, r);
                    gooLogic.clean(world);
                }
            }
            prevDelay = delay;
        }
    }

    private float getRotation(Vector3Int pos, Vector3 direction)
    {
        bool isFlor = !tiles.HasTile(pos + Vector3Int.up);
        bool isRoof = !tiles.HasTile(pos + Vector3Int.down);
        bool isLeftWall = !tiles.HasTile(pos + Vector3Int.right);
        bool isRightWall = !tiles.HasTile(pos + Vector3Int.left);

        bool sprayRight = direction.x > 0;
        bool sprayUp = direction.y > 0;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (isRightWall && sprayRight)
            {
                return 90;
            }
            else if (isLeftWall && !sprayRight)
            {
                return 270;
            }
            else if (isFlor && !sprayUp)
            {
                return 0;
            }
            else if (isRoof && sprayUp)
            {
                return 180;
            }
        }
        else
        {
            if (isFlor && !sprayUp)
            {
                return 0;
            }
            else if (isRoof && sprayUp)
            {
                return 180;
            }
            else if (isRightWall && sprayRight)
            {
                return 90;
            }
            else if (isLeftWall && !sprayRight)
            {
                return 270;
            }
        }
        return -1;
    }

    private IEnumerator createTiles(Vector3 worldPosition)
    {
        Vector3Int local = tiles.WorldToCell(worldPosition);
        createTileAt(local);
        yield return new WaitForSeconds(0.05f);
        for (int i = 1; i <= numTilesToCreate / 2; i++)
        {
            createTileAt(local + new Vector3Int(-i, 0, 0));
            createTileAt(local + new Vector3Int(i, 0, 0));
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void createTileAt(Vector3Int local)
    {
        if (!tiles.HasTile(local))
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = tileCreationSprite;
            tiles.SetTile(local, tile);
            tiles.RefreshTile(local);
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        var obj = collision.gameObject;
        if (obj.GetComponent<PlayerCharacter>() != null)
        {
            foreach (var contact in collision.contacts)
            {
                Vector3 worldPos = contact.point + 0.2f * contact.normal;
                Debug.DrawLine(contact.point, worldPos, Color.red, 1.0f);

                Vector3Int pos = tiles.WorldToCell(worldPos);
                if (tiles.HasTile(pos))
                {
                    Sprite sprite = tiles.GetSprite(pos);
                    if (sprite.name.Contains("Crumble"))
                    {
                        if(contact.normal.y < -0.5)
                        {
                            crumble(worldPos);
                        }
                    }
                    else if (sprite.name.Contains("Hazard"))
                    {
                        hazard(collision.gameObject);
                    }
                }
            }
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
        removeTilesAt(worldPosition);
        Vector3Int pos = tiles.WorldToCell(worldPosition);
        ObjectPoolSystem.Instance.InstantiatePrefabWith(crumblePrefab, tiles.CellToWorld(pos) + prefabOffset, Quaternion.identity);
        crumbling.Remove(pos);
    }
}
