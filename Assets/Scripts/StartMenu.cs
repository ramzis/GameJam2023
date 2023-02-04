using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        //SceneManager.LoadScene("Screen Name From Builder");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
