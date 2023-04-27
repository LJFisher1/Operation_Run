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
    public GameObject creditsSubmenu;
    public TextMeshProUGUI tutorialScore;
    public TextMeshProUGUI leve1Score;
    public TextMeshProUGUI level2Score;
    public TextMeshProUGUI level3Score;
    public TextMeshProUGUI level4Score;
    public TextMeshProUGUI level5Score;
    public TextMeshProUGUI level6Score;
    public TextMeshProUGUI level7Score;
    public TextMeshProUGUI level8Score;
    public Slider SensitivitySlider;

    int lastCompletedLevel;
    GameData loadedData;
    public Button PrimaryButton;
    public Button StartButton;
    public Button ContinueButton;
    public Button TutorialButton;
    public Button ApplyButton;
    public Button QuitButton;
    public Button CreditsBackButton;



    public void OpenNewGameSubMenu()
    {
        activeMenu = newGameSubmenu;
        activeMenu.SetActive(true);
        menuBlocker.SetActive(true);
        StartButton.Select();
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
        if(loadedData.levels[1].highScore != 0)
        {
            tutorialScore.text = loadedData.levels[1].highScore.ToString();
        }
        if (loadedData.levels[2].highScore != 0)
        {
            leve1Score.text = loadedData.levels[2].highScore.ToString();
        }
        if (loadedData.levels[3].highScore != 0)
        {
            level2Score.text = loadedData.levels[3].highScore.ToString();
        }
        if (loadedData.levels[4].highScore != 0)
        {
            level3Score.text = loadedData.levels[4].highScore.ToString();
        }
        if (loadedData.levels[5].highScore != 0)
        {
            level4Score.text = loadedData.levels[5].highScore.ToString();
        }
        if (loadedData.levels[6].highScore != 0)
        {
            level6Score.text = loadedData.levels[6].highScore.ToString();
        }
        if (loadedData.levels[7].highScore != 0)
        {
            level7Score.text = loadedData.levels[7].highScore.ToString();
        }
        if (loadedData.levels[8].highScore != 0)
        {
            level5Score.text = loadedData.levels[8].highScore.ToString();
        }
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

    public void OpenCreditsSubMenu()
    {
        activeMenu = creditsSubmenu;
        activeMenu.SetActive(true);
        menuBlocker.SetActive(true);
    }

    public void Cancel()
    {
        activeMenu.SetActive(false);
        activeMenu = null;
        menuBlocker.SetActive(false);
        UpdatePlayerSensitivity();

    }

    public void Apply()
    {
        UpdatePlayerSensitivity();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        StartCoroutine(NewGameDelay(0)); // making this 0 fixed a bug where if you returned to the main menu after being in a scene, you couldnt hit new game again. not sure why. might need some kind of debounce?
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
        return;
    }

    public void SaveSettings(ref GameData data)
    {
        return;
    }

    public IEnumerator NewGameDelay(float f)
     {
         yield return new WaitForSeconds(f);
         SceneManager.LoadScene(1);
     }

     public void UpdatePlayerSensitivity()
     {
        //GameManager.instance.UpdateSensitivity(SensitivitySlider.value);
     }

    
}
