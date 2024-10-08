using System;

using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [SerializeField]
    private Loot loot;
    [SerializeField]
    private Animator animator;
    private bool isSpawned = false;
    private bool isAnimated = false;

    private float minDistance = 0.25f;
    private float checkInterval = 0.2f;
    private float checkTimer = 0f;

    private float spawnTimer = 0f;
    private float spawnDuration = 0.5f;

    private Vector2 targetPosition;
    private Vector2 startPosition;
    public Loot Loot { get { return loot; } }

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private WorldTile wTile;
    void Start()
    {

    }

    private void SpawnLoot(Vector2 pos, SpawnType spawnType)
    {
        WorldTileType tileType = spawnType == SpawnType.RandomDirt ? WorldTileType.Dirt : (
            spawnType == SpawnType.RandomPackedDirt ? WorldTileType.PackedDirt : (

                spawnType == SpawnType.RandomMoreDirt ? WorldTileType.MoreDirt : (
                    spawnType == SpawnType.RandomExtraDirt ? WorldTileType.ExtraDirt : (
                        WorldTileType.Stone
                    )
                )
            )
        );
        if (tileType == WorldTileType.Stone)
        {
            Debug.Log("something went wrong");
            startPosition = pos;
            targetPosition = Vector2.zero;
            return;
        }
        bool found = false;
        var tiles = WorldGrid.main.GetTiles(pos, loot.SpawnModifier);
        while (tiles.Count > 0)
        {
            Vector2Int tilePos = tiles[UnityEngine.Random.Range(0, tiles.Count)];
            WorldTile tile = WorldGrid.main.GetTile(tilePos);
            if (tile != null && tile.Type == tileType && tile.Diggable(PlayerLevel.main.DigPower))
            {
                found = true;
                targetPosition = tilePos;
                startPosition = pos;
                break;
            }
            tiles.Remove(tilePos);
        }
        if (!found)
        {
            startPosition = pos;
            targetPosition = Vector2.zero;
        }
    }

    public void Initialize(Vector2 pos, WorldTile tile)
    {
        wTile = tile;
        spriteRenderer.sprite = loot.Sprite;
        SoundManager.main.PlaySound(GameSoundType.Found);
        SpawnLoot(pos, loot.SpawnType);

        transform.position = pos;
        Show();
    }

    public void Show()
    {
        animator.Play("lootDropShow");
    }

    public void AnimationFinished()
    {
        isAnimated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAnimated)
        {
            return;
        }
        if (!isSpawned)
        {
            spawnTimer += Time.deltaTime;
            transform.position = Vector2.Lerp(startPosition, targetPosition, spawnTimer / spawnDuration);
            if (spawnTimer >= spawnDuration)
            {
                isSpawned = true;
                transform.position = targetPosition;
            }
            return;
        }
        checkTimer += Time.deltaTime;
        if (checkTimer < checkInterval)
        {
            return;
        }
        else
        {
            checkTimer = 0f;
        }
        if (Vector2.Distance(PlayerCharacter.main.GridPosition, transform.position) <= minDistance)
        {
            WorldGrid.main.ConsumeGoodie(wTile.Prefix);
            PlayerLevel.main.GainLoot(loot);
            Destroy(gameObject);
        }

    }


}

[System.Serializable]
public class Loot
{
    public LootType Type;
    public SpawnType SpawnType;
    public int SpawnModifier;
    public Sprite Sprite;
    public int Value;
}

public enum SpawnType
{
    RandomDirt,
    RandomPackedDirt,
    RandomMoreDirt,
    RandomExtraDirt
}

public enum LootType
{
    Xp,
    DigPower,
    Radius
}
