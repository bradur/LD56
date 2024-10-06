using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public static PlayerLevel main;
    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private List<LevelProgress> levels = new();

    private LevelProgress currentLevel;
    private LevelProgress previousLevel;

    private int xp = 0;
    private int xpComing = 0;
    private int digPower = 0;
    public int DigPower { get { return digPower; } }
    private int visionRadius = 1;
    public int VisionRadius { get { return visionRadius; } }

    private int level = 0;
    public int Level { get { return level; } }
    private float moveSpeed = 0;
    public float MoveSpeed { get { return moveSpeed; } }
    private float digSpeed = 0;
    public float DigSpeed { get { return digSpeed; } }

    public int NextLevelXP { get { return currentLevel.XpRequired; } }
    public int PreviousLevelXp { get { return previousLevel == null ? 0 : previousLevel.XpRequired; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLevel = levels[level];
        UIManager.main.GainXp(0, delegate () { });
    }

    public void Upgrade(UpgradeType Type)
    {
        if (Type == UpgradeType.VisionRadius)
        {
            visionRadius += 1;
        }
        if (Type == UpgradeType.MoveSpeed)
        {
            moveSpeed += 0.05f;
        }
        if (Type == UpgradeType.DigSpeed)
        {
            digSpeed += 0.1f;
        }
    }

    public void GainLoot(Loot loot)
    {
        Debug.Log($"Caught loot '{loot.Type}' with value {loot.Value}");
        if (loot.Type == LootType.Xp)
        {
            GainXp(loot.Value, transform.position);
        }
    }

    public void GainXp(int value, Vector2 pos)
    {
        if (value <= 0)
        {
            return;
        }
        xpComing += value;
        Vector2 offset = new Vector2(-1f, 0f);
        UIManager.main.ShowMessage($"+ {value}xp", pos + offset, Color.cyan);
        UIManager.main.GainXp(value, delegate ()
        {
            xp += xpComing;
            xpComing = 0;
            if (xp > currentLevel.XpRequired)
            {
                level += 1;
                if (level >= levels.Count)
                {
                    Debug.Log("Max level reached!");
                }
                else
                {
                    previousLevel = currentLevel;
                    currentLevel = levels[level];
                    UIManager.main.ShowLevelPopup();
                }
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}


[System.Serializable]
public class LevelProgress
{
    public int XpRequired;
}