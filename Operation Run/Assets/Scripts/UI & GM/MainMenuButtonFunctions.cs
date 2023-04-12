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
    public GameObject levelLocked;
    public GameObject activeMenu;
    public GameObject menuBlocker;

    public static bool tutorial = false;
    public static bool level1 = false;
    public static bool level2 = false;
    public static bool level3 = false;
    public static bool level4 = false;
    public static bool level5 = false;

    public void OpenNewGameSubMenu()
    {
        activeMenu = newGameSubmenu;
        activeMenu.SetActive(true);
        menuBlocker.SetActive(true);
    }
    public void OpenContinueGameSubMenu()
    {
        //if save !exists
        //activeMenu = noSaveMenu;
        //else
        activeMenu = continueGameSubmenu;
        activeMenu.SetActive(true);
        menuBlocker.SetActive(true);
    }
    public void OpenLoadGameSubMenu()
    {
        //if save !exists
        //activeMenu = noSaveMenu;
        //else
        activeMenu = loadGameSubmenu;
        activeMenu.SetActive(true);
        menuBlocker.SetActive(true);
    }
    public void OpenLevelSelectSubMenu()
    {
        //if levels !are unlocked
        //activeMenu = noLevelsMenu;
        //else
        activeMenu = levelSelectSubmenu;
        activeMenu.SetActive(true);
        menuBlocker.SetActive(true);
    }
    public void OpenSettingsSubMenu()
    {
        activeMenu = settingsSubmenu;
        activeMenu.SetActive(true);
        menuBlocker.SetActive(true);
    }
    public void OpenQuitSubMenu()
    {
        activeMenu = quitSubmenu;
        activeMenu.SetActive(true);
        menuBlocker.SetActive(true);
    }

    public void Cancel()
    {
        activeMenu.SetActive(false);
        activeMenu = null;
        menuBlocker.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        SceneManager.LoadScene(0);
    }

    public void SelectLevelTutorial()
    {
        if (level5 == true)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(LevelLockedFlash());
        }
    }

    public void Continue()
    {
        DataPersistence.instance.LoadGame();
        if(level2==true)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void SelectLevel1()
    {
        SceneManager.LoadScene(2);
    }
    public void SelectLevel2()
    {
        if (level2 == true)
        {
            SceneManager.LoadScene(3);
        }
        else
        {
            StartCoroutine(LevelLockedFlash());
        }
    }
    public void SelectLevel3()
    {
        if (level3 == true)
        {
            SceneManager.LoadScene(4);
        }
        else
        {
            StartCoroutine(LevelLockedFlash());
        }
    }
    public void SelectLevel4()
    {
        if (level4 == true)
        {
            SceneManager.LoadScene(5);
        }
        else
        {
            StartCoroutine(LevelLockedFlash());
        }
    }
    public void SelectLevel5()
    {
        if (level5 == true)
        {
            SceneManager.LoadScene(6);
        }
        else
        {
            StartCoroutine(LevelLockedFlash());
        }
    }

    public IEnumerator LevelLockedFlash()
    {
        levelLocked.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        levelLocked.SetActive(false);
    }
}
