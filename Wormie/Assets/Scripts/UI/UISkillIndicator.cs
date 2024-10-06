using UnityEngine;
using UnityEngine.UI;

public class UISkillIndicator : MonoBehaviour
{
    [SerializeField]
    private Image imgIndicator;
    [SerializeField]
    private Color turnedOnColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void TurnOn()
    {
        imgIndicator.color = turnedOnColor;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
