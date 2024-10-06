using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILevelPopup : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private bool isShown = false;

    private UIXPBar uiXPBar;

    [SerializeField]
    private List<UILevelPopupButton> uiLevelPopupButtons = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void Show(UIXPBar uIXPBar)
    {
        uiXPBar = uIXPBar;
        if (isShown)
        {
            return;
        }
        Time.timeScale = 0f;
        uiXPBar.ShowLevel();
        animator.Play("uiLevelPopupShow");
    }
    public void Hide()
    {
        animator.Play("uiLevelPopupHide");
    }

    public void ShowAnimationFinished()
    {
        foreach (UILevelPopupButton button in uiLevelPopupButtons)
        {
            if (PlayerLevel.main.IsMaxLevel(button.Type))
            {
                button.GreyOut();
            }
        }
        isShown = true;
    }
    public void HideAnimationFinished()
    {
        Time.timeScale = 1f;
        foreach (UILevelPopupButton button in uiLevelPopupButtons)
        {
            button.Deactivate();
        }
        uiXPBar.Resume();
        isShown = false;
    }



    // Update is called once per frame
    void Update()
    {
        if (isShown)
        {
            foreach (UILevelPopupButton button in uiLevelPopupButtons)
            {
                if (Input.GetKeyDown(button.Key))
                {
                    bool allowed = button.Activate();
                    if (allowed)
                    {
                        PlayerLevel.main.Upgrade(button.Type);
                        Hide();
                        break;
                    }
                    else
                    {
                        Debug.Log("cant upgrade, it's max!");
                    }
                }
            }
        }
    }
}
