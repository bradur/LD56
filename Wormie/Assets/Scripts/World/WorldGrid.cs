using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGrid : MonoBehaviour
{
    public static WorldGrid main;

    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private List<WorldTile> worldTiles = new();

    private bool flipDir = false;

    public DigResult Dig(Vector2Int pos)
    {
        TileBase tile = GetTileAt(pos);
        WorldTile wTile = GetTile(pos);

        if (wTile == null)
        {
            Debug.Log("Should not be null!");
            return new() { Finished = false, Success = false };
        }
        if (wTile.Diggable(PlayerLevel.main.DigPower))
        {
            TileBase newTile = wTile.Dig(tile);
            DrawTile(pos, newTile);
            return new() { Finished = newTile == null, AfterDigPrefab = wTile.AfterDigPrefab, Success = true };
        }
        return new() { Finished = false, Success = false };
    }
    public WorldTile GetTile(Vector2 pos)
    {
        Vector2Int normalized = new(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
        Debug.Log($"normalized: {normalized}");
        return GetTile(normalized);
    }
    public WorldTile GetTile(Vector2Int pos)
    {
        TileBase tile = GetTileAt(pos);
        if (tile == null)
        {
            return null;
        }
        return worldTiles.Find(worldTile => tile.name.StartsWith(worldTile.Prefix));
    }

    public List<Vector2Int> GetTiles(Vector2 pos, int radius)
    {
        Vector2Int normalized = new(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
        List<Vector2Int> tiles = new();
        for (int xPos = normalized.x - radius; xPos <= (pos.x + radius); xPos += 1)
        {
            for (int yPos = normalized.y - radius; yPos <= (pos.y + radius); yPos += 1)
            {
                Vector2Int newPos = new(xPos, yPos);
                if (Mathf.Abs(Vector2Int.Distance(newPos, normalized)) > radius - 1)
                {
                    tiles.Add(newPos);
                }
            }
        }
        return tiles;
    }

    public MoveResult MoveAttempt(MoveAttempt attempt)
    {
        Vector2Int target = attempt.Origin + attempt.Direction;
        WorldTile tile = GetTile(target);
        bool isDiagonal = attempt.Direction.y != 0 && attempt.Direction.x != 0;
        MoveResult defaultSuccess = new(attempt, target, tile, true);
        MoveResult defaultFail = new(attempt, target, tile, false);
        if (!isDiagonal)
        {
            if (tile == null)
            {
                return defaultSuccess;
            }
            return defaultFail;
        }

        Vector2Int xNeighborPos = new(attempt.Origin.x + attempt.Direction.x, attempt.Origin.y);
        WorldTile xNeighborTile = GetTile(xNeighborPos);
        bool xNeighborExists = xNeighborTile != null;
        MoveResult moveToXNeighbor = new(attempt, xNeighborPos, xNeighborTile, true);

        Vector2Int yNeighborPos = new(attempt.Origin.x, attempt.Origin.y + attempt.Direction.y);
        WorldTile yNeighborTile = GetTile(yNeighborPos);
        MoveResult moveToYNeighbor = new(attempt, yNeighborPos, yNeighborTile, true);
        bool yNeighborExists = yNeighborTile != null;
        if (tile == null)
        {
            if (!xNeighborExists && yNeighborExists)
            {
                return new(attempt, xNeighborPos, tile, true);
            }
            else if (xNeighborExists && !yNeighborExists)
            {
                return new(attempt, yNeighborPos, tile, true);
            }
            else if (xNeighborExists && yNeighborExists)
            {
                return new(attempt, xNeighborPos, xNeighborTile, false);
            }
            return defaultSuccess;
        }

        if (!xNeighborExists && yNeighborExists)
        {
            return moveToXNeighbor;
        }
        else if (xNeighborExists && !yNeighborExists)
        {
            return moveToYNeighbor;
        }
        else if (xNeighborExists && yNeighborExists)
        {
            bool xDiggable = xNeighborTile.Diggable(PlayerLevel.main.DigPower);
            bool yDiggable = xNeighborTile.Diggable(PlayerLevel.main.DigPower);
            WorldTile diggableTile = xDiggable ? xNeighborTile : (yDiggable ? yNeighborTile : null);
            Vector2Int diggablePos = xDiggable ? xNeighborPos : (yDiggable ? yNeighborPos : Vector2Int.zero);
            if (flipDir)
            {
                diggableTile = yDiggable ? yNeighborTile : (xDiggable ? xNeighborTile : null);
                diggablePos = yDiggable ? yNeighborPos : (xDiggable ? xNeighborPos : Vector2Int.zero);
            }
            flipDir = !flipDir;
            return new(attempt, diggablePos, diggableTile, false);
        }
        return defaultFail;
    }



    private TileBase GetTileAt(Vector2 pos)
    {
        Vector3Int normalized = new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), 0);
        return GetTileAt(normalized);
    }

    private TileBase GetTileAt(Vector2Int pos)
    {
        Vector3Int normalized = new Vector3Int(pos.x, pos.y, 0);
        return GetTileAt(normalized);
    }

    private TileBase GetTileAt(Vector3Int pos)
    {
        Vector3Int normalized = pos;
        normalized.x -= 1;
        normalized.y -= 1;
        return tilemap.GetTile(normalized);
    }

    private void DrawTile(Vector2Int pos, TileBase tile)
    {
        Vector3Int normalized = new(pos.x, pos.y, 0);
        normalized.x -= 1;
        normalized.y -= 1;
        tilemap.SetTile(normalized, tile);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 vpPos = Camera.main.ScreenToWorldPoint(mousePos);
            TileBase tile = GetTileAt(vpPos);
            UIDebugText.main.ShowMessage($"m[{mousePos}]\nvp[{vpPos}]\ntile[{tile}]");
        }
    }
}

[System.Serializable]
public class WorldTile
{
    public string Name = "";
    public string Prefix = "";
    public WorldTileType Type;

    public bool IsWall;
    public int DigPowerRequired = 0;

    public bool Diggable(int power)
    {
        return !IsWall && power >= DigPowerRequired;
    }

    public int Xp = 10;
    public int XpFinish = 100;

    public GameObject AfterDigPrefab;

    public List<TileBase> tiles = new();

    public TileBase Dig(TileBase targetTile)
    {
        for (int index = 0; index < tiles.Count; index += 1)
        {
            TileBase tile = tiles[index];
            if (tile.name == targetTile.name)
            {
                if (index >= tiles.Count - 1)
                {
                    return null;
                }
                return tiles[index + 1];
            }
        }
        return null;
    }

}

public struct DigResult
{
    public GameObject AfterDigPrefab;
    public bool Finished;
    public bool Success;
}

public enum WorldTileType
{
    Dirt,
    PackedDirt,
    Stone,
    Goodie,
    MoreDirt,
    ExtraDirt
}
