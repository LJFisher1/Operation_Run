using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player")]
    public GameObject player;
    public PlayerController playerController;
    public GameObject playerSpawnPosition;

    GameObject activeMenu;
    [Header("Game UI")]
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject checkPointMenu;
    public GameObject playerHitFlash;
    [SerializeField] GameObject sensitivitySlider;
    public Image playerHealthBar;
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI enemiesRemainingText;
    public GameObject playerKeyPopup;// Key
    public TextMeshProUGUI KeyCountText;// Key
    public GameObject hpPickup;

    [Header("Game Goals")]
    public int enemiesRemaining;

    public bool isPaused;

    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = PlayerController.FindObjectOfType<PlayerController>();
        playerSpawnPosition = GameObject.FindGameObjectWithTag("Player Spawn Position");
        objectiveText.text = ("Enemies Remaining:");
        KeyCountText.text = playerController.keysInPossession.ToString("F0");
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

    public void GameUpdateGoal(int amount)
    {
        enemiesRemaining += amount;
        enemiesRemainingText.text = enemiesRemaining.ToString("F0");

        if (enemiesRemaining < 1)
        {
            enemiesRemainingText.text = ("");
            objectiveText.text = ("Find The Escape");
            //remove enemies remaining text and add the "ESCAPE!!" Goal
        }
    }

    public IEnumerator FlashKeyPopup()// Key
    {
        playerKeyPopup.SetActive(true);
        yield return new WaitForSeconds(1f);
        playerKeyPopup.SetActive(false);
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
        UpdateSensitivity();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
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

    public void PlayerDead()
    {
        GamePaused();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }

    public void UpdateSensitivity()
    {
        Camera.main.GetComponent<CameraController>().UpdateSensitivity(sensitivitySlider.GetComponent<Slider>().value);
    }
    public IEnumerator hpFlash()
    {
        GameManager.instance.hpPickup.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameManager.instance.hpPickup.SetActive(false);
    }
}
