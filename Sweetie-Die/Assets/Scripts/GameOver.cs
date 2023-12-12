using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    public Image blackScreen; // Asegúrate de tener una imagen negra en tu escena y arrástrala aquí en el inspector

    public void GameOver()
    {
        StartCoroutine(FadeOutAndChangeScene());
    }

    IEnumerator FadeOutAndChangeScene()
    {
        // Fade to black
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // Ajusta la alfa de la imagen negra para crear el efecto de desvanecimiento
            blackScreen.color = new Color(0, 0, 0, i);
            yield return null;
        }

        // Cambia a la escena de GameOver
        SceneManager.LoadScene("GameOver");

        // Fade from black
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // Ajusta la alfa de la imagen negra para crear el efecto de desvanecimiento
            blackScreen.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }
}