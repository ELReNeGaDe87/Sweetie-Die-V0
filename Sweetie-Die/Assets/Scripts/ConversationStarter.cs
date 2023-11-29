using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation FirstConversation;
    [SerializeField] private NPCConversation GoodEndingConversation;
    [SerializeField] private NPCConversation BadEndingConversation;

    private bool hasHadFirstConversation = false;

    public GameObject heartMonitor;
    public GameObject heart;

    Image heartImage;

    public Sprite filledHeart;
    public Sprite unfilledHeart;

    public static bool ConversationIsActive = false;

    private void Start()
    {
        heartImage = heart.GetComponent<Image>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasHadFirstConversation)
            {
                ConversationManager.Instance.StartConversation(FirstConversation);
            }
            else
            {
                ConversationManager.Instance.StartConversation(GoodEndingConversation);
            }
        }
    }

    private void OnEnable()
    {
        ConversationManager.OnConversationStarted += ConversationStart;
        ConversationManager.OnConversationEnded += ConversationEnd;
    }

    private void OnDisable()
    {
        ConversationManager.OnConversationStarted -= ConversationStart;
        ConversationManager.OnConversationEnded -= ConversationEnd;
    }

    private void ConversationStart()
    {
        ConversationIsActive = true;
        UnityEngine.Debug.Log("A conversation has begun.");
        heartMonitor.SetActive(true);
        fillHearts(0);
        Cursor.lockState = CursorLockMode.Confined;

    }
    private void ConversationEnd()
    {
        hasHadFirstConversation = true;
        ConversationIsActive = false;
        heartMonitor.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Debug.Log("A conversation has ended.");
    }

    private void fillHearts(int numHearts)
    {
        if (numHearts >= 1)
        {
            heartImage.sprite = filledHeart;
        }
    }
}
