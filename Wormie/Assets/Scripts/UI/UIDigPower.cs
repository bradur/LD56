using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDigPower : MonoBehaviour
{
    private List<UIDigPowerIcon> icons = new();

    [SerializeField]
    private Transform iconContainer;
    [SerializeField]
    private Image imgFill;

    void Start()
    {
        foreach (Transform child in iconContainer)
        {
            icons.Add(child.GetComponent<UIDigPowerIcon>());
        }
    }

    public void UpdateDisplay(int digPower, float percentage)
    {
        imgFill.fillAmount = percentage;
        foreach (UIDigPowerIcon icon in icons)
        {
            icon.UpdateDisplay(digPower);
        }
    }
}
