using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public float timeBeforeSkip = 5f;
    private float timer;
    [SerializeField]
    private GameObject button;
    private float animationTime = 39f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeBeforeSkip && !button.activeSelf)
        {
            button.SetActive(true);
        }

        if (timer >= animationTime)
        {
            GoToMainMenu();
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
