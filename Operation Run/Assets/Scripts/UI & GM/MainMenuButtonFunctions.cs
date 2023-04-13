using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static GameData;

public class MainMenuButtonFunctions : MonoBehaviour, iDataPersistence
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

    int lastCompletedLevel;
    GameData loadedData;

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
        SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        SceneManager.LoadScene(lastCompletedLevel);
    }
    public void SelectLevel(int buildIndex)
    {
        if (loadedData.levels[buildIndex].unlocked)
        {
            SceneManager.LoadScene(buildIndex);
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

    public void LoadData(GameData data)
    {
        lastCompletedLevel = data.GetLastCompletedBuildIndex();
        loadedData = data;
    }

    public void SaveData(ref GameData data)
    {
        throw new System.NotImplementedException();
    }
}
