using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkill : MonoBehaviour
{
    [SerializeField]
    private UpgradeType upgradeType;

    public UpgradeType Type { get { return upgradeType; } }


    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private Image imgIcon;

    [SerializeField]
    private Transform indicatorContainer;

    private List<UISkillIndicator> indicators = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform child in indicatorContainer)
        {
            UISkillIndicator indicator = child.GetComponent<UISkillIndicator>();
            indicators.Add(indicator);
        }
        imgIcon.sprite = icon;
        UpdateDisplay(PlayerLevel.main.GetSkill(Type).Level);
    }

    public void UpdateDisplay(int level)
    {
        for (int index = 0; index < level; index += 1)
        {
            if (indicators.Count > index)
            {
                indicators[index].TurnOn();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
