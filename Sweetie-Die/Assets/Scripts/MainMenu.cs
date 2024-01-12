using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public GameObject logo;
    public GameObject background;
    public GameObject startButton;
    public GameObject optionsButton;
    public GameObject creditsButton;
    public GameObject exitButton;
    public GameObject backgroundOptCre;

    Image logoImage;
    Image backgroundImage;
    Image startButtonImage;
    Image optionsButtonImage;
    Image creditsButtonImage;
    Image exitButtonImage;
    Image backgroundOptCreImage;

    public Sprite logo_sweet;
    public Sprite background_sweet;
    public Sprite startButton_sweet;
    public Sprite optionsButton_sweet;
    public Sprite creditsButton_sweet;
    public Sprite exitButton_sweet;
    public Sprite backgroundOptCre_sweet;

    public Sprite logo_creepy;
    public Sprite background_creepy;
    public Sprite startButton_creepy;
    public Sprite optionsButton_creepy;
    public Sprite creditsButton_creepy;
    public Sprite exitButton_creepy;
    public Sprite backgroundOptCre_creepy;

    public float sweetInterval = 4f;
    public float creepyInterval = 1f;

    private void Start()
    {
        logoImage = logo.GetComponent<Image>();
        backgroundImage = background.GetComponent<Image>();
        startButtonImage = startButton.GetComponent<Image>();
        optionsButtonImage = optionsButton.GetComponent<Image>();
        creditsButtonImage = creditsButton.GetComponent<Image>();
        exitButtonImage = exitButton.GetComponent<Image>();
        backgroundOptCreImage = backgroundOptCre.GetComponent<Image>();

        StartCoroutine(SwitchMenus());   
    }
  
    public IEnumerator SwitchMenus()
    {
        while (true) 
        {

            yield return new WaitForSeconds(sweetInterval);

            logoImage.sprite = logo_creepy;
            backgroundImage.sprite = background_creepy;
            startButtonImage.sprite = startButton_creepy;
            optionsButtonImage.sprite = optionsButton_creepy;
            creditsButtonImage.sprite = creditsButton_creepy;
            exitButtonImage.sprite = exitButton_creepy;
            backgroundOptCreImage.sprite = backgroundOptCre_creepy;


            yield return new WaitForSeconds(creepyInterval);

            logoImage.sprite = logo_sweet;
            backgroundImage.sprite = background_sweet;
            startButtonImage.sprite = startButton_sweet;
            optionsButtonImage.sprite = optionsButton_sweet;
            creditsButtonImage.sprite = creditsButton_sweet;
            exitButtonImage.sprite = exitButton_sweet;
            backgroundOptCreImage.sprite = backgroundOptCre_sweet;
        }
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene("Hotel");
    }

    public void QuitGame ()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
