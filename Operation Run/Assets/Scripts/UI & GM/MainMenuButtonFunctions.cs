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
        activeMenu = newGameMenu;
        activeMenu.SetActive(true);
    }
    public void OpenContinueGameSubMenu()
    {
        activeMenu = continueGameMenu;
        activeMenu.SetActive(true);
    }
    public void OpenLoadGameSubMenu()
    {
        activeMenu = loadGameMenu;
        activeMenu.SetActive(true);
    }
    public void OpenLevelSelectSubMenu()
    {
        activeMenu = levelSelectMenu;
        activeMenu.SetActive(true);
    }
    public void OpenSettingsSubMenu()
    {
        activeMenu = settingsMenu;
        activeMenu.SetActive(true);
    }
    public void OpenQuitSubMenu()
    {
        activeMenu = quitMenu;
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
}
