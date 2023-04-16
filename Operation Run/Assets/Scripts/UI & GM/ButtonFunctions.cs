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
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.GameUnpaused();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RespawnPlayer()
    {
        GameManager.instance.GameUnpaused();
        GameManager.instance.playerController.SpawnPlayer();
    }

    public void NextScene()
    {
        GameManager.instance.GameUnpaused();
        int nextSceneIdx = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIdx < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIdx);
        }
        else
        {
            Debug.Log("There is no next level");
        }
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
        GameManager.instance.activeMenu.SetActive(false);
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
    public void ForwardsButton()
    { 
    
    }
    
    public void BackwardsButton()
    {

    }

    public void LeftButton()
    {

    }

    public void RightButton()
    {

    }

    public void JumpButton()
    {

    }

    public void AttackButton()
    {

    }

    public void HealButton()
    {

    }

    public void ApplyButton()
    {

    }
}

