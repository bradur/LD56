using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWar : MonoBehaviour
{
    public static FogOfWar main;
    void Awake()
    {
        main = this;
    }
    [SerializeField]
    private Tilemap tilemap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void DrawCircle(int radius, Vector2Int origin, TileBase tile)
    {
        int originX = origin.x - 1;
        int originY = origin.y - 1;

        for (int xPos = originX - radius; xPos <= (originX + radius); xPos += 1)
        {
            for (int yPos = originY - radius; yPos <= (originY + radius); yPos += 1)
            {
                if ((xPos - originX) * (xPos - originX) + (yPos - originY) * (yPos - originY) <= radius * radius)
                {
                    tilemap.SetTile(new Vector3Int(xPos, yPos, 0), tile);
                }
            }
        }
    }

    public void ClearFog(Vector2Int pos)
    {
        DrawCircle(PlayerLevel.main.VisionRadius, pos, null);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
