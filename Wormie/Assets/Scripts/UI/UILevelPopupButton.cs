using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelPopupButton : MonoBehaviour
{
    [SerializeField]
    private KeyCode key;
    public KeyCode Key { get { return key; } }

    [SerializeField]
    private TextMeshProUGUI txtKey;
    [SerializeField]
    private Image imgBg;
    [SerializeField]
    private Image borderBg;
    [SerializeField]
    private Color bgColor;
    [SerializeField]
    private Color activeColor;
    [SerializeField]
    private Color greyedOutColor;
    private Color originalTextColor;
    private Color originalBorderColor;


    [SerializeField]
    private UpgradeType type;
    public UpgradeType Type { get { return type; } }
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private Image iconRenderer;
    private bool greyedOut = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imgBg.color = bgColor;
        string keyName = key.ToString();
        if (keyName.Contains("Alpha"))
        {
            keyName = keyName.Replace("Alpha", "");
        }
        txtKey.text = keyName;
        originalTextColor = txtKey.color;
        originalBorderColor = borderBg.color;
        iconRenderer.sprite = icon;
    }

    public void GreyOut()
    {
        greyedOut = true;
        imgBg.color = greyedOutColor;
        imgBg.color = greyedOutColor;
    }

    public void Deactivate()
    {
        txtKey.color = originalTextColor;
        borderBg.color = originalBorderColor;
    }

    public bool Activate()
    {
        if (greyedOut)
        {
            return false;
        }
        txtKey.color = activeColor;
        borderBg.color = activeColor;
        return true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public enum UpgradeType
{
    VisionRadius,
    DigSpeed,
    MoveSpeed,
    DigPower
}