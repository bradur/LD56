using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager main;
    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private UIPopText uiPopTextPrefab;
    [SerializeField]
    private UIXPBar uIXPBar;
    [SerializeField]
    private UILevelPopup uiLevelPopup;
    [SerializeField]
    private UISkillsBar uiSkillsBar;
    [SerializeField]
    private UIDigPower uiDigPower;
    [SerializeField]
    private UIGoodieDisplay uiGoodieDisplay;

    [SerializeField]
    private Color defaultPopTextColor;
    [SerializeField]
    private Transform messageContainer;

    [SerializeField]
    private List<XpColor> xpColors = new();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void InitializeGoodieDisplay(List<WorldTile> goodies)
    {
        uiGoodieDisplay.Initialize(goodies);
    }
    public void ConsumeGoodie(WorldTile wTile)
    {
        uiGoodieDisplay.Consume(wTile);
    }
    public void UpdateDigPower(int level, float percentage)
    {
        uiDigPower.UpdateDisplay(level, percentage);
    }

    public void UpdateSkill(SkillLevel skill)
    {
        uiSkillsBar.UpdateSkill(skill.Type, skill.Level);
    }

    public void ShowLevelPopup()
    {
        uiLevelPopup.Show(uIXPBar);
    }

    public void GainXp(int value, UnityAction finishedCallback)
    {
        uIXPBar.AddXP(value, finishedCallback);
    }

    public void ShowXpDrop(int value, Vector2 position)
    {
        XpColor xpColor = xpColors.Find(xpColor => value >= xpColor.MinValue && xpColor.MaxValue >= value);
        if (xpColor == null)
        {
            ShowMessage($"+ {value}", position, defaultPopTextColor);
        }
        else
        {
            ShowMessage($"+ {value}", position, xpColor.Color);
        }
    }

    public void ShowMessage(string message, Vector2 position)
    {
        ShowMessage(message, position, defaultPopTextColor);
    }

    public void ShowMessage(string message, Vector2 position, Color color)
    {
        UIPopText popText = Instantiate(uiPopTextPrefab, messageContainer);
        popText.Show(message, position, color);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
[System.Serializable]
public class XpColor
{
    public int MinValue;
    public int MaxValue;
    public Color Color;
}