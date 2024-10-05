using UnityEngine;

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
    private Color defaultPopTextColor;
    [SerializeField]
    private Transform messageContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void GainXp(int value)
    {
        uIXPBar.AddXP(value);
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
