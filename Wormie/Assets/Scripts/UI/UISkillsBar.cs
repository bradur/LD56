using System.Collections.Generic;
using UnityEngine;

public class UISkillsBar : MonoBehaviour
{
    [SerializeField]
    private Transform skillContainer;
    private List<UISkill> skills = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform child in skillContainer)
        {
            UISkill skill = child.GetComponent<UISkill>();
            skills.Add(skill);
        }
    }

    public void UpdateSkill(UpgradeType upgradeType, int level)
    {
        UISkill skill = skills.Find(skill => skill.Type == upgradeType);
        if (skill != null)
        {
            skill.UpdateDisplay(level);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
