using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void Resume()
    {
        GameManager.instance.GameUnpaused();
        GameManager.instance.isPaused = !GameManager.instance.isPaused;
    }
    public void Restart()
    {
        GameManager.instance.GameUnpaused();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RespawnPlayer()
    {
        GameManager.instance.GameUnpaused();
        GameManager.instance.PlayerController.SpawnPlayer();
    }
}

