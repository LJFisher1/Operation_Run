using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    GameObject activeMenu;
    [Header("*   Game UI")]
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject checkPointMenu;
    public GameObject playerHitFlash;
    public Image playerHealthBar;


    public bool isPaused;

    private void Awake()
    {
        instance = this;

    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);

            if(isPaused)
            {
                GamePaused();
            }
            else
            {
                GameUnpaused();
            }
        }
    }

    public void GamePaused()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void GameUnpaused()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void PlayerLose()
    {
        GamePaused();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }

    public void PlayerWin()
    {
        GamePaused();
        activeMenu = winMenu;
        activeMenu.SetActive(true);
    }

    public IEnumerator PlayerHitFlash()
    {
        playerHitFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerHitFlash.SetActive(false);
    }
}
