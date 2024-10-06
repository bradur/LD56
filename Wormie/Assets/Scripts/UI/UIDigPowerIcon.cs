using UnityEngine;
using UnityEngine.UI;

public class UIDigPowerIcon : MonoBehaviour
{
    [SerializeField]
    private int minLevel;

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private Image imgIcon;
    [SerializeField]
    private Image imgBorder;
    [SerializeField]
    private Color highlightBorderColor;
    [SerializeField]
    private Color highlightIconColor;
    private RectTransform rt;
    private float width = 512f - 48f * 2;
    private float step;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //step = width / 10f;
        step = 40f;
        rt = GetComponent<RectTransform>();
        imgIcon.sprite = sprite;
        //transform.localPosition = new Vector2(minLevel, 0);
        rt.anchoredPosition = new Vector2(12 + (minLevel - 1) * step, 0);
    }

    public void UpdateDisplay(int level)
    {
        if (level >= minLevel)
        {
            imgBorder.color = highlightBorderColor;
            imgIcon.color = highlightIconColor;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
