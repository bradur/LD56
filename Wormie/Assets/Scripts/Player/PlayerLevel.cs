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
    private int digPower = 0;
    public int DigPower { get { return digPower; } }
    private int visionRadius = 1;
    public int VisionRadius { get { return visionRadius; } }

    private int level = 0;
    public int Level { get { return level; } }

    public int NextLevelXP { get { return currentLevel.XpRequired; } }
    public int PreviousLevelXp { get { return previousLevel == null ? 0 : previousLevel.XpRequired; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLevel = levels[level];
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
        xp += value;
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
            }
        }
        Vector2 offset = new Vector2(-1f, 0f);
        UIManager.main.ShowMessage($"+ {value}xp", pos + offset, Color.cyan);
        UIManager.main.GainXp(value);
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