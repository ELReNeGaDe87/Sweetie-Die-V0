using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation monsterConversation;

    public GameObject heartMonitor;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject heart4;
    public GameObject heart5;

    Image heart1Image;
    Image heart2Image;
    Image heart3Image;
    Image heart4Image;
    Image heart5Image;

    public Sprite filledHeart;
    public Sprite unfilledHeart;

    public static bool ConversationIsActive = false;

    private void Start()
    {
        heart1Image = heart1.GetComponent<Image>();
        heart2Image = heart2.GetComponent<Image>();
        heart3Image = heart3.GetComponent<Image>();
        heart4Image = heart4.GetComponent<Image>();
        heart5Image = heart5.GetComponent<Image>();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ConversationManager.Instance.StartConversation(monsterConversation);
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
        fillHearts(4);
        Cursor.lockState = CursorLockMode.Confined;

    }
    private void ConversationEnd()
    {
        ConversationIsActive = false;
        heartMonitor.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Debug.Log("A conversation has ended.");
    }

    private void fillHearts(int numHearts)
    {
        if (numHearts >= 1)
        {
            heart1Image.sprite = filledHeart;
        }
        if (numHearts >= 2)
        {
            heart2Image.sprite = filledHeart;
        }
        if (numHearts >= 3)
        {
            heart3Image.sprite = filledHeart;
        }
        if (numHearts >= 4)
        {
            heart4Image.sprite = filledHeart;
        }
        if (numHearts >= 5)
        {
            heart5Image.sprite = filledHeart;
        }
    }

    //private void Update()
    //{
    //    if (ConversationManager.Instance != null)
    //    {
    //        if (ConversationManager.Instance.IsConversationActive)
    //        {
    //            if (Input.GetKeyDown(KeyCode.UpArrow)) ConversationManager.Instance.SelectPreviousOption();
    //            else if (Input.GetKeyDown(KeyCode.DownArrow)) ConversationManager.Instance.SelectNextOption();
    //            else if (Input.GetKeyDown(KeyCode.Return)) ConversationManager.Instance.PressSelectedOption();
    //        }
    //    }
    //}
}
