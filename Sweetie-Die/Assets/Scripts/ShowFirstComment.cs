using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowFirstComment : MonoBehaviour
{
    public float fadeInDuration = 1.5f;   // Set the duration for fade-in
    public float stayVisibleDuration = 3f; // Set the duration to stay visible
    public float fadeOutDuration = 1.5f;  // Set the duration for fade-out
    public float startDelay = 1f;          // Set the delay before the fading starts

    private TextMeshProUGUI textMeshProComponent;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        textMeshProComponent = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Update the timer only after the start delay has passed
        if (startDelay > 0f)
        {
            startDelay -= Time.deltaTime;
            return;
        }

        // Update the timer
        timer += Time.deltaTime;

        if (timer < fadeInDuration)
        {
            // Fade in
            float alpha = Mathf.Clamp01(timer / fadeInDuration);
            SetTextAlpha(alpha);
        }
        else if (timer < fadeInDuration + stayVisibleDuration)
        {
            // Stay visible
            SetTextAlpha(1f);
        }
        else if (timer < fadeInDuration + stayVisibleDuration + fadeOutDuration)
        {
            // Fade out
            float alpha = Mathf.Clamp01(1f - (timer - fadeInDuration - stayVisibleDuration) / fadeOutDuration);
            SetTextAlpha(alpha);
        }
        else
        {
            // Destroy the GameObject after the entire sequence is complete
            Destroy(gameObject);
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

    public void HideText()
    {
        Destroy(gameObject);
    }
}