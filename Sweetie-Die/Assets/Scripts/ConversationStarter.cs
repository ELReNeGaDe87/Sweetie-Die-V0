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

    private Vector3 teleportPosition = new Vector3(141.38f, 1.2f, -79.43f);


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
                ConversationManager.Instance.StartConversation(BadEndingConversation);
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
        if (hasHadFirstConversation)
        {
            GameOver();
            return;
        }
        hasHadFirstConversation = true;
        ConversationIsActive = false;
        heartMonitor.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        TeleportPlayer();
        
        UnityEngine.Debug.Log("A conversation has ended.");
    }

    private void TeleportPlayer()
    {
        GameObject player = GameObject.Find("Player");

        // Check if the player GameObject is found
        if (player != null)
        {
            // Get the TeleportScript component attached to the player GameObject
            PlayerController playerController = player.GetComponent<PlayerController>();

            // Check if the TeleportScript component is found
            if (playerController != null)
            {
                // Call the Teleport function
                playerController.Teleport(teleportPosition);
            }
            else
            {
                UnityEngine.Debug.LogError("TeleportScript component not found on the player GameObject.");
            }
        }
        else
        {
            UnityEngine.Debug.LogError("Player GameObject not found.");
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    private void fillHearts(int numHearts)
    {
        if (numHearts >= 1)
        {
            heartImage.sprite = filledHeart;
        }
    }
}
