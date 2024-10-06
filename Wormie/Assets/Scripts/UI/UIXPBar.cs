using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIXPBar : MonoBehaviour
{
    private int xp = 0;
    private int animatedXp = 0;
    private int targetXp = 0;
    private int startXp = 0;
    private float percentage;
    private float animateTimer = 0f;
    [SerializeField]
    private float animateDuration = 0.15f;
    private bool isAnimating = false;
    private float originalFontSize;
    private float startFontSize;
    private float targetFontSize;

    [SerializeField]
    private float fontSizeChange = 16;

    private UnityAction finishedCallback;

    [SerializeField]
    private TextMeshProUGUI txtPercentage;
    [SerializeField]
    private Transform levelContainer;
    [SerializeField]
    private TextMeshProUGUI txtLevel;
    [SerializeField]
    private Image imgFill;
    private bool isShowingLevel = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animatedXp = xp;
        originalFontSize = txtPercentage.fontSize;
    }

    public void AddXP(int addition, UnityAction finishedCallback)
    {
        this.finishedCallback = finishedCallback;
        startXp = animatedXp;
        targetXp = xp + addition;
        xp = targetXp;
        isAnimating = true;
        startFontSize = originalFontSize;
        targetFontSize = startFontSize + fontSizeChange;
        //Debug.Log($"Animating xp: xp{xp} target{targetXp} startXp{animatedXp}");
    }

    private void UpdateView(int currentXp, float fontSize)
    {
        if (isShowingLevel)
        {
            return;
        }
        int contextXp = currentXp - PlayerLevel.main.PreviousLevelXp;
        int contextMaxXp = PlayerLevel.main.NextLevelXP - PlayerLevel.main.PreviousLevelXp;
        percentage = contextXp / (contextMaxXp * 1.0f);
        percentage = Mathf.Clamp(percentage, 0, 1);
        txtPercentage.fontSize = fontSize;
        //txtPercentage.text = $"{percentage * 100:0}%";
        //txtPercentage.text = $"current({currentXp}) {contextXp} / {PlayerLevel.main.NextLevelXP} xp({xp})";
        txtPercentage.text = $"{contextXp} / {contextMaxXp}";
        imgFill.fillAmount = percentage;
    }

    public void ShowLevel()
    {
        isShowingLevel = true;
        txtPercentage.text = $".~< {PlayerLevel.main.Level + 1} >~.";
        txtPercentage.fontSize = originalFontSize + fontSizeChange;
        imgFill.fillAmount = 1;
        txtLevel.text = $"{PlayerLevel.main.Level + 1}";
        levelContainer.gameObject.SetActive(false);
    }

    public void Resume()
    {
        levelContainer.gameObject.SetActive(true);
        isShowingLevel = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShowingLevel)
        {
            return;
        }
        if (!isAnimating)
        {
            return;
        }
        animateTimer += Time.deltaTime;
        animatedXp = (int)Mathf.Lerp(startXp, targetXp, animateTimer / animateDuration);
        float fontSize = Mathf.Lerp(startFontSize, targetFontSize, (animateTimer * 1.5f) / animateDuration);
        if ((animateTimer * 1.5f) > animateDuration)
        {
            startFontSize = fontSize;
            targetFontSize = originalFontSize;
        }
        UpdateView(animatedXp, fontSize);
        if (animateTimer >= animateDuration)
        {
            finishedCallback();
            UpdateView(targetXp, originalFontSize);
            animateTimer = 0f;
            isAnimating = false;
        }
    }
}
