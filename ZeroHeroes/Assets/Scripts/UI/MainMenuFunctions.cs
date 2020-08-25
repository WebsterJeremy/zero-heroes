using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuFunctions : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Plot");
    }

    public void Options()
    {
        SceneManager.LoadScene("Options Menu");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
