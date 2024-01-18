using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System;

public class ShowFantasmaComments : MonoBehaviour
{
    [SerializeField]
    GameObject blackBackground;
    [SerializeField]
    GameObject comment1;
    [SerializeField]
    GameObject comment2;
    [SerializeField]
    GameObject comment3;
    [SerializeField]
    GameObject comment4;
    [SerializeField]
    GameObject lastComment;

    private GameObject activeCommentObject;
    private TextMeshProUGUI activeComment;
    List<GameObject> availableComments;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        availableComments = new List<GameObject> { comment1, comment2, comment3 };
        playerController = FindObjectOfType<PlayerController>();

        // Aet default active comment to first one
        activeCommentObject = comment1;
        activeComment = comment1.GetComponent<TextMeshProUGUI>();
    }

    public void ShowComment(int num)
    {
        GameObject name;
        switch(num){
            case 1:
                name = comment1;
                break;
            case 2:
                name = comment2;
                break;
            case 3:
                name = comment3;
                break;
            case 4:
                name = comment4;
                break;
            case 5:
                name = lastComment;
                break;
            default:
                name = comment1;
                break;
        }
        activeCommentObject = name;
        activeComment = name.GetComponent<TextMeshProUGUI>();
        StartCoroutine(FadeText());
    }

    IEnumerator FadeText()
    {
        // Deactivate player movement while text is showing
        playerController.Deactivate();

        // Activate the UI elements
        blackBackground.SetActive(true);
        activeCommentObject.SetActive(true);

        // Fade in
        yield return Fade(0f, 1f, 1f);

        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Fade out
        yield return Fade(1f, 0f, 1f);

        // Optionally, you can perform additional actions after fading, if needed
        UnityEngine.Debug.Log("Fading completed!");

        // Deactivate UI elements
        activeCommentObject.SetActive(false);
        blackBackground.SetActive(false);

        // Activate player movement when text has faded away
        playerController.Activate();
    }

    IEnumerator Fade(float startAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color startColor = activeComment.color;

        while (elapsedTime < duration)
        {
            activeComment.color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, targetAlpha), (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        activeComment.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
    }
}
