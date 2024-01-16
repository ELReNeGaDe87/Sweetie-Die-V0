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

    public void ShowFourthComment()
    {
        activeCommentObject = comment4;
        activeCommentObject.SetActive(true);
        activeComment = comment4.GetComponent<TextMeshProUGUI>();
        StartCommentRoutine();
    }

    public void ShowLastComment()
    {
        activeCommentObject = lastComment;
        activeCommentObject.SetActive(true);
        activeComment = lastComment.GetComponent<TextMeshProUGUI>();
        StartCommentRoutine();
    }

    public void ShowRandomComment()
    {
        SetRandomComment();
        StartCommentRoutine();
    }

    void StartCommentRoutine()
    {
        blackBackground.SetActive(true);
        StartCoroutine(FadeText());
    }

    void SetRandomComment()
    {
        // Check if the list is not empty
        if (availableComments.Count == 0)
        {
            UnityEngine.Debug.LogWarning("The availableComments list is empty.");
        }

        // Generate a random index within the bounds of the list
        int randomIndex = UnityEngine.Random.Range(0, availableComments.Count);

        // Set the random comment as the active comment
        activeCommentObject = availableComments[randomIndex];
        activeCommentObject.SetActive(true);
        activeComment = activeCommentObject.GetComponent<TextMeshProUGUI>();
        availableComments.RemoveAt(randomIndex);
    }

    IEnumerator FadeText()
    {
        // Deactivate player movement while text is showing
        playerController.Deactivate();

        // Fade in
        yield return Fade(0f, 1f, 1f);

        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Fade out
        yield return Fade(1f, 0f, 1f);

        // Optionally, you can perform additional actions after fading, if needed
        UnityEngine.Debug.Log("Fading completed!");

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
