using System.Collections.Generic;
using NUnit.Framework;
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

    public bool Dig(Vector2Int pos)
    {
        TileBase tile = GetTileAt(pos);
        WorldTile wTile = GetTile(pos);
        Debug.Log($"Dig tile at pos: {pos} ({wTile})");
        if (wTile == null)
        {
            Debug.Log("Should not be null!");
            return false;
        }
        if (wTile.Type == WorldTileType.Dirt)
        {
            TileBase newTile = wTile.Dig(tile);
            DrawTile(pos, newTile);
            return newTile == null;
        }
        return false;
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
    public MoveResult MoveAttempt(MoveAttempt attempt)
    {
        Vector2Int target = attempt.Origin + attempt.Direction;
        WorldTile tile = GetTile(target);
        bool isDiagonal = attempt.Direction.y != 0 && attempt.Direction.x != 0;
        if (!isDiagonal)
        {
            if (tile == null)
            {
                return new() { Success = true, Position = target };
            }
            return new() { Success = false, Position = target, Tile = tile };
        }

        Vector2Int xNeighborPos = new(attempt.Origin.x + attempt.Direction.x, attempt.Origin.y);
        WorldTile xNeighborTile = GetTile(xNeighborPos);
        bool xNeighborExists = xNeighborTile != null;

        Vector2Int yNeighborPos = new(attempt.Origin.x, attempt.Origin.y + attempt.Direction.y);
        WorldTile yNeighborTile = GetTile(yNeighborPos);
        bool yNeighborExists = yNeighborTile != null;
        if (tile == null)
        {
            if (!xNeighborExists && yNeighborExists)
            {
                return new() { Success = true, Position = xNeighborPos };
            }
            if (xNeighborExists && !yNeighborExists)
            {
                return new() { Success = true, Position = yNeighborPos };
            }
            if (xNeighborExists && yNeighborExists)
            {
                return new() { Success = false };
            }
            return new() { Success = true, Position = target };
        }

        if (!xNeighborExists && yNeighborExists)
        {
            return new() { Success = true, Position = xNeighborPos };
        }
        if (xNeighborExists && !yNeighborExists)
        {
            return new() { Success = true, Position = yNeighborPos };
        }
        if (xNeighborExists && yNeighborExists)
        {
            return new() { Success = false };
        }
        return new() { Success = false, Position = target, Tile = tile };
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

public enum WorldTileType
{
    Dirt,
    PackedDirt,
    Stone
}
