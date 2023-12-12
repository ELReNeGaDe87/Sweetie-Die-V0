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
    [SerializeField] private NPCConversation MiddleConversation;
    [SerializeField] private NPCConversation GoodEndingConversation;
    [SerializeField] private NPCConversation BadEndingConversation;

    private bool hasHadFirstConversation = false;

    public GameObject heartMonitor;
    public GameObject heart;

    Image heartImage;

    public Sprite filledHeart;
    public Sprite unfilledHeart;

    public static bool ConversationIsActive = false;

    public Transform teleportPosition;


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
                if (RecogerObjeto.HoldingObject())
                {
                    if (RecogerObjeto.heldObject.CompareTag("GiftCone"))
                    {
                        ConversationManager.Instance.StartConversation(GoodEndingConversation);
                    }
                    else
                    {
                        ConversationManager.Instance.StartConversation(BadEndingConversation);
                    }
                }
                else
                {
                    ConversationManager.Instance.StartConversation(MiddleConversation);
                }
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
        if (Cursor.lockState != CursorLockMode.Confined)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

    }

    private void ConversationEnd()
    {
        ConversationIsActive = false;
        heartMonitor.SetActive(false);
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        UnityEngine.Debug.Log("A conversation has ended.");
    }

    public void TeleportPlayer()
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

    public void SetHasHadFirstConversation()
    {
        hasHadFirstConversation = true;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("BadEnding");
    }

    public void GameWon()
    {
        SceneManager.LoadScene("GameWon");
    }

    public void FillHeart()
    {
        heartImage.sprite = filledHeart;
    }
}
