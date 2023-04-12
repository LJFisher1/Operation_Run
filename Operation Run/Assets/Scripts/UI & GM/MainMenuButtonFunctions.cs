using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class MainMenuButtonFunctions : MonoBehaviour
{
    public GameObject newGameMenu;
    public GameObject continueGameMenu;
    public GameObject loadGameMenu;
    public GameObject levelSelectMenu;
    public GameObject settingsMenu;
    public GameObject quitMenu;
    public GameObject newGameSubmenu;
    public GameObject continueGameSubmenu;
    public GameObject loadGameSubmenu;
    public GameObject levelSelectSubmenu;
    public GameObject settingsSubmenu;
    public GameObject quitSubmenu;

    public GameObject activeMenu;

    public void OpenNewGameSubMenu()
    {
        activeMenu = newGameSubmenu;
        activeMenu.SetActive(true);
    }
    public void OpenContinueGameSubMenu()
    {
        //if save !exists
        //activeMenu = noSaveMenu;
        //else
        activeMenu = continueGameSubmenu;
        activeMenu.SetActive(true);
    }
    public void OpenLoadGameSubMenu()
    {
        //if save !exists
        //activeMenu = noSaveMenu;
        //else
        activeMenu = loadGameSubmenu;
        activeMenu.SetActive(true);
    }
    public void OpenLevelSelectSubMenu()
    {
        //if levels !are unlocked
        //activeMenu = noLevelsMenu;
        //else
        activeMenu = levelSelectSubmenu;
        activeMenu.SetActive(true);
    }
    public void OpenSettingsSubMenu()
    {
        activeMenu = settingsSubmenu;
        activeMenu.SetActive(true);
    }
    public void OpenQuitSubMenu()
    {
        activeMenu = quitSubmenu;
        activeMenu.SetActive(true);
    }

    public void Cancel()
    {
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void SelectLevel1()
    {
        SceneManager.LoadScene(1);
    }
    public void SelectLevel2()
    {
        if (GameManager.instance.level2 == true)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            StartCoroutine(GameManager.instance.LevelLockedFlash());
        }
    }
    public void SelectLevel3()
    {
        if (GameManager.instance.level3 == true)
        {
            SceneManager.LoadScene(3);
        }
        else
        {
            StartCoroutine(GameManager.instance.LevelLockedFlash());
        }
    }
    public void SelectLevel4()
    {
        if (GameManager.instance.level4 == true)
        {
            SceneManager.LoadScene(4);
        }
        else
        {
            StartCoroutine(GameManager.instance.LevelLockedFlash());
        }
    }
    public void SelectLevel5()
    {
        if (GameManager.instance.level5 == true)
        {
            SceneManager.LoadScene(5);
        }
        else
        {
            StartCoroutine(GameManager.instance.LevelLockedFlash());
        }
    }
}
