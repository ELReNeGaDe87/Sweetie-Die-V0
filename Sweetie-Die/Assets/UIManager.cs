using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    // Fade-times
    public float fadeDuration = 1f;
    public float showTextDuration = 3f;

    // Aimdot and E
    [SerializeField]
    private GameObject aimDot;
    [SerializeField]
    private GameObject e;

    // Pickup object texts
    [SerializeField]
    private GameObject pickupObjectText;
    [SerializeField]
    private GameObject switchObjectText;

    // Fading texts
    [SerializeField]
    private GameObject readBookText;
    [SerializeField]
    private GameObject hidingText;
    [SerializeField]
    private GameObject brokenDoorText;
    [SerializeField]
    private GameObject firstCommentText;

    private List<GameObject> texts;

    // Fantasma comments
    [SerializeField]
    private GameObject blackBackground;
    [SerializeField]
    private GameObject commentText;

    void Awake()
    {
        texts = new List<GameObject> { pickupObjectText, switchObjectText, readBookText, hidingText, brokenDoorText, firstCommentText };
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowFirstCommentText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowE(bool value)
    {
        UnityEngine.Debug.Log("ShowE");
        aimDot.SetActive(!value);
        e.SetActive(value);
    }

    public void HideEAndAimDot()
    {
        e.SetActive(false);
        HideAimDot();
    }

    public void HideAimDot()
    {
        aimDot.SetActive(false);
    }

    public void ShowAimDot()
    {
        aimDot.SetActive(true);
    }

    public void ShowFirstCommentText()
    {
        ShowFadingText(firstCommentText);
    }

    public void ShowReadBookText()
    {
        ShowFadingText(readBookText);
    }

    public void ShowHidingText()
    {
        ShowFadingText(hidingText);
    }

    public void ShowBrokenDoorText()
    {
        ShowFadingText(brokenDoorText);
    }

    public void ShowPickupObjectText()
    {
        ShowRegularText(pickupObjectText);
    }

    public void ShowSwitchObjectText()
    {
        ShowRegularText(switchObjectText);
    }

    public void HideObjectTexts()
    {
        pickupObjectText.SetActive(false);
        switchObjectText.SetActive(false);
    }

    public void HideAllTexts()
    {
        foreach(GameObject o in texts)
        {
            o.SetActive(false);
        }
    }

    private void ShowFadingText(GameObject text)
    {
        if (!text.activeSelf)
        {
            StartCoroutine(FadeText(text));
        }
    }

    private void ShowRegularText(GameObject text)
    {
        if (!text.activeSelf)
        {
            HideAllTexts();
            text.SetActive(true);
        }
    }


    IEnumerator FadeText(GameObject textObject)
    {
        HideAllTexts();

        textObject.SetActive(true);

        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();

        // Fade in
        yield return Fade(text, 0f, 1f, fadeDuration);

        // Wait for 3 seconds
        yield return new WaitForSeconds(showTextDuration);

        // Fade out
        yield return Fade(text, 1f, 0f, fadeDuration);

        // Optionally, you can perform additional actions after fading, if needed
        UnityEngine.Debug.Log("Fading completed!");

        textObject.SetActive(false);
    }

    IEnumerator Fade(TextMeshProUGUI text, float startAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color startColor = text.color;

        while (elapsedTime < duration)
        {
            text.color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, targetAlpha), (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
    }
}
