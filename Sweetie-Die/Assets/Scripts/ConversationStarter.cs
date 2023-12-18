using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

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

    public GameObject aimDot;

    public float minWaitBetweenPlays = 3f;
    public float maxWaitBetweenPlays = 15f;
    public float waitTimeCountdown = -1f;
    private List<string> laughters = new List<string> { "LaughterMonster1", "LaughterMonster2", "LaughterMonster3" };
    private string currentLaugh;

    private AudioManager audioManager;


    private void Start()
    {
        heartImage = heart.GetComponent<Image>();
        audioManager = FindObjectOfType<AudioManager>();
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
        aimDot.SetActive(false);
        heartMonitor.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        audioManager.Play("Music");
        audioManager.Pause("BackgroundNoise");
    }

    private void ConversationEnd()
    {
        ConversationIsActive = false;
        aimDot.SetActive(true);
        heartMonitor.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Debug.Log("A conversation has ended.");
        audioManager.Stop("Music");
        audioManager.Play("BackgroundNoise");
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
        SceneManager.LoadScene("GoodEnding");
    }

    public void FillHeart()
    {
        heartImage.sprite = filledHeart;
    }

    //public void Update()
    //{
    //    if (!ConversationIsActive) return;
    //    foreach (string laugh in laughters)
    //    {
    //        if (audioManager.IsPlaying(laugh)) return;
    //    }
    //    if (ConversationIsActive && waitTimeCountdown < 0f)
    //    {
    //        currentLaugh = laughters[UnityEngine.Random.Range(0, laughters.Count)];
    //        audioManager.Play(currentLaugh);
    //        waitTimeCountdown = UnityEngine.Random.Range(minWaitBetweenPlays, maxWaitBetweenPlays);
    //    }
    //    else
    //    {
    //        waitTimeCountdown -= Time.deltaTime;
    //    }
    //}
}
