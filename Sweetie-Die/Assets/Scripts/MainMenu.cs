using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public GameObject background;
    public GameObject startButton;
    public GameObject optionsButton;
    public GameObject creditsButton;
    public GameObject exitButton;

    Image backgroundImage;
    Image startButtonImage;
    Image optionsButtonImage;
    Image creditsButtonImage;
    Image exitButtonImage;

    public Sprite background_sweet;
    public Sprite startButton_sweet;
    public Sprite optionsButton_sweet;
    public Sprite creditsButton_sweet;
    public Sprite exitButton_sweet;

    public Sprite background_creepy;
    public Sprite startButton_creepy;
    public Sprite optionsButton_creepy;
    public Sprite creditsButton_creepy;
    public Sprite exitButton_creepy;

    private float sweetInterval = 3f;
    private float creepyInterval = 0.5f;

    private void Start()
    {
        backgroundImage = background.GetComponent<Image>();
        startButtonImage = startButton.GetComponent<Image>();
        optionsButtonImage = optionsButton.GetComponent<Image>();
        creditsButtonImage = creditsButton.GetComponent<Image>();
        exitButtonImage = exitButton.GetComponent<Image>();
        
        StartCoroutine(SwitchMenus());   
    }
  
    public IEnumerator SwitchMenus()
    {
        while (true) 
        {

            yield return new WaitForSeconds(sweetInterval);

            backgroundImage.sprite = background_creepy;
            startButtonImage.sprite = startButton_creepy;
            optionsButtonImage.sprite = optionsButton_creepy;
            creditsButtonImage.sprite = creditsButton_creepy;
            exitButtonImage.sprite = exitButton_creepy;

            yield return new WaitForSeconds(creepyInterval);

            backgroundImage.sprite = background_sweet;
            startButtonImage.sprite = startButton_sweet;
            optionsButtonImage.sprite = optionsButton_sweet;
            creditsButtonImage.sprite = creditsButton_sweet;
            exitButtonImage.sprite = exitButton_sweet;
        }
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame ()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
