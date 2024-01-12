using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BrokenDoorText : MonoBehaviour
{
    public float fadeInDuration = 1.5f;   // Set the duration for fade-in
    public float stayVisibleDuration = 3f; // Set the duration to stay visible
    public float fadeOutDuration = 1.5f;  // Set the duration for fade-out

    private float currentFadeInDuration;   // Set the duration for fade-in
    private float currentStayVisibleDuration; // Set the duration to stay visible
    private float currentFadeOutDuration;  // Set the duration for fade-out

    private TextMeshProUGUI textMeshProComponent;
    public float timer;

    private ShowFirstComment showFirstComment;
    private HidingText hidingText;

    // Start is called before the first frame update
    void Start()
    {
        textMeshProComponent = GetComponent<TextMeshProUGUI>();
        showFirstComment = FindObjectOfType<ShowFirstComment>();
        hidingText = FindObjectOfType<HidingText>();
    }

    public void Show()
    {
        if (showFirstComment != null) showFirstComment.HideText();
        if (hidingText.IsShowing()) hidingText.HideText();

        timer = 0;
        currentFadeInDuration = fadeInDuration;
        currentStayVisibleDuration = stayVisibleDuration;
        currentFadeOutDuration = fadeOutDuration;
    }

    void Update()
    {
        if (timer >= currentFadeInDuration + currentStayVisibleDuration + currentFadeOutDuration) return;

        // Update the timer
        timer += Time.deltaTime;

        if (timer < currentFadeInDuration)
        {
            // Fade in
            float alpha = Mathf.Clamp01(timer / currentFadeInDuration);
            SetTextAlpha(alpha);
        }
        else if (timer < currentFadeInDuration + currentStayVisibleDuration)
        {
            // Stay visible
            SetTextAlpha(1f);
        }
        else if (timer < currentFadeInDuration + currentStayVisibleDuration + currentFadeOutDuration)
        {
            // Fade out
            float alpha = Mathf.Clamp01(1f - (timer - currentFadeInDuration - currentStayVisibleDuration) / currentFadeOutDuration);
            SetTextAlpha(alpha);
        }
    }

    void SetTextAlpha(float alpha)
    {
        if (textMeshProComponent != null)
        {
            Color textColor = textMeshProComponent.color;
            textColor.a = alpha;
            textMeshProComponent.color = textColor;
        }
    }

    public bool IsShowing()
    {
        return (timer < currentFadeInDuration + currentStayVisibleDuration + currentFadeOutDuration);
    }

    public void HideText()
    {
        timer = currentFadeInDuration + currentStayVisibleDuration + currentFadeOutDuration + 1;
        SetTextAlpha(0);
    }
}
