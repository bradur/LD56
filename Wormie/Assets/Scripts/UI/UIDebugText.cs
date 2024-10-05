using TMPro;
using UnityEngine;

public class UIDebugText : MonoBehaviour
{
    public static UIDebugText main;
    void Awake()
    {
        main = this;
    }
    [SerializeField]
    private TextMeshProUGUI txtDebug;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ShowMessage(string text)
    {
        txtDebug.text = text;
    }
}
