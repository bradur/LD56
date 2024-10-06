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
    public int DigPower { get { return GetSkill(UpgradeType.DigPower).IntValue; } }
    public int VisionRadius { get { return Mathf.RoundToInt(GetSkill(UpgradeType.VisionRadius).IntValue); } }

    private int level = 0;
    public int Level { get { return level; } }

    public float MoveSpeed { get { return GetSkill(UpgradeType.MoveSpeed).Value; } }
    public float DigSpeed { get { return GetSkill(UpgradeType.DigSpeed).Value; } }


    public int NextLevelXP { get { return currentLevel.XpRequired; } }
    public int PreviousLevelXp { get { return previousLevel == null ? 0 : previousLevel.XpRequired; } }

    [SerializeField]
    private List<SkillLevel> skills = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLevel = levels[level];
        UIManager.main.GainXp(0, delegate () { });
    }

    public SkillLevel GetSkill(UpgradeType upgradeType)
    {
        SkillLevel skill = skills.Find(skill => skill.Type == upgradeType);
        return skill;
    }

    public bool IsMaxLevel(UpgradeType Type)
    {
        SkillLevel skillLevel = GetSkill(Type);
        if (skillLevel != null)
        {
            return skillLevel.Level == skillLevel.MaxLevel;
        }
        return false;
    }

    public void Upgrade(UpgradeType Type)
    {
        SkillLevel skill = GetSkill(Type);
        if (skill != null)
        {
            skill.Level += 1;
            UIManager.main.UpdateSkill(skill);
        }
        /*
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
        }*/
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

[System.Serializable]
public class SkillLevel
{
    public int Level = 1;
    public int MaxLevel = 7;
    public float Value { get { return Level * Modifier; } }
    public bool IsInt;
    public int IntValue { get { return Level * IntModifier; } }
    public float Modifier;
    public int IntModifier;
    public UpgradeType Type;
}