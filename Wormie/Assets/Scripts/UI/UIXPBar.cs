using System;
using TMPro;
using UnityEngine;
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


    [SerializeField]
    private TextMeshProUGUI txtPercentage;
    [SerializeField]
    private Image imgFill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animatedXp = xp;
        originalFontSize = txtPercentage.fontSize;
    }

    public void AddXP(int addition)
    {
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
        int contextXp = currentXp - PlayerLevel.main.PreviousLevelXp;
        int contextMaxXp = PlayerLevel.main.NextLevelXP - PlayerLevel.main.PreviousLevelXp;
        percentage = contextXp / (contextMaxXp * 1.0f);
        percentage = Mathf.Clamp(percentage, 0, 1);
        txtPercentage.fontSize = fontSize;
        //txtPercentage.text = $"{percentage * 100:0}%";
        //txtPercentage.text = $"current({currentXp}) {contextXp} / {PlayerLevel.main.NextLevelXP} xp({xp})";
        txtPercentage.text = $"{currentXp} / {PlayerLevel.main.NextLevelXP}";
        imgFill.fillAmount = percentage;
    }

    // Update is called once per frame
    void Update()
    {
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
            UpdateView(targetXp, originalFontSize);
            animateTimer = 0f;
            isAnimating = false;
        }
    }
}
