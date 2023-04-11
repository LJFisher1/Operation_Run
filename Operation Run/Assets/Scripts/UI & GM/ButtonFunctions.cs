using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
        GameManager.instance.playerController.SpawnPlayer();
    }

    public void NextScene()
    {
        GameManager.instance.GameUnpaused();
        SceneManager.LoadScene(sceneBuildIndex: SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
    public void OpenSettingsSubMenu()
    {
        GameManager.instance.activeMenu = GameManager.instance.settingsMenu;
        GameManager.instance.activeMenu.SetActive(true);
    }

    public void OpenGuide()
    {
        GameManager.instance.activeMenu = GameManager.instance.guideMenu;
        GameManager.instance.activeMenu.SetActive(true);
    }

    public void Cancel()
    {
        GameManager.instance.activeMenu.SetActive(false);
        GameManager.instance.activeMenu = GameManager.instance.pauseMenu;
        GameManager.instance.activeMenu.SetActive(true);
    }

    public void MakeActiveTipGem()
    {
        GameManager.instance.guideTips.text = GameManager.instance.gemsTip.text;
    }

    public void MakeActiveTipGold()
    {
        GameManager.instance.guideTips.text = GameManager.instance.goldTip.text;
    }
    public void MakeActiveTipKeys()
    {
        GameManager.instance.guideTips.text = GameManager.instance.keysTip.text;
    }

    public void MakeActiveTipDoors()
    {
        GameManager.instance.guideTips.text = GameManager.instance.doorsTip.text;
    }
    public void MakeActiveTipHealth()
    {
        GameManager.instance.guideTips.text = GameManager.instance.healthTip.text;
    }

    public void MakeActiveTipMana()
    {
        GameManager.instance.guideTips.text = GameManager.instance.manaTip.text;
    }
    public void MakeActiveTipWalls()
    {
        GameManager.instance.guideTips.text = GameManager.instance.wallsTip.text;
    }

    public void MakeActiveTipEnemies()
    {
        GameManager.instance.guideTips.text = GameManager.instance.enemiesTip.text;
    }
    public void MakeActiveTipStaffs()
    {
        GameManager.instance.guideTips.text = GameManager.instance.staffsTip.text;
    }
    public void MakeActiveTipHourglasses()
    {
        GameManager.instance.guideTips.text = GameManager.instance.hourglassesTip.text;
    }
    public void MakeActiveTipWind()
    {
        GameManager.instance.guideTips.text = GameManager.instance.windTip.text;
    }
}

