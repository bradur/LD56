using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITheEndPopup : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private bool isShown = false;
    private bool isInitialized = false;
    private float goodieTimer = 0f;
    private float goodieInterval = 0.25f;

    private List<UIGoodie> goodies;
    [SerializeField]
    private UIGoodie uiGoodiePrefab;
    [SerializeField]
    private Transform goodieContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void Show(List<UIGoodie> goodies)
    {
        this.goodies = new(goodies);
        if (isShown)
        {
            return;
        }
        goodieTimer = goodieInterval;
        Time.timeScale = 0f;
        animator.Play("uiTheEndPopupShow");
    }
    public void Hide()
    {
        animator.Play("uiTheEndPopupHide");
    }

    public void ShowAnimationFinished()
    {
        SoundManager.main.PlaySound(GameSoundType.Levelup);
        isShown = true;
    }
    public void HideAnimationFinished()
    {
        Time.timeScale = 1f;
        isShown = false;
    }

    private void ShowGoodie(UIGoodie goodie) {
        UIGoodie newGoodie = Instantiate(uiGoodiePrefab, goodieContainer);
        newGoodie.Initialize(goodie.Wtile);
        newGoodie.Consume();
        newGoodie.Wiggle();
        goodies.Remove(goodie);
    }


    // Update is called once per frame
    void Update()
    {
        if (isShown && !isInitialized) {
            goodieTimer += Time.unscaledDeltaTime;
            if (goodieTimer >= goodieInterval) {
                if (goodies.Count > 0) {
                    ShowGoodie(goodies[0]);
                    goodieTimer = 0f;
                } else {
                    isInitialized = true;
                }
            }
        }
        if (isShown && isInitialized)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Hide();
            }
        }
    }
}
