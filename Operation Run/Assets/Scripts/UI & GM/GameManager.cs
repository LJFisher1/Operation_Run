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

    public GameObject activeMenu;
    [Header("Game UI")]
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject checkPointMenu;
    public GameObject startMenu;
    public GameObject playerHitFlash;
    [SerializeField] GameObject sensitivitySlider;
    public Image playerHealthBar;
    public Image playerManaBar;
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI GemsRemainingText;
    public GameObject playerKeyPopup;// Key
    public TextMeshProUGUI KeyCountText;// Key
    public GameObject noKeysPopup;//door
    public GameObject usedKeyPopup;//door
    public GameObject HealItemPopup;
    public GameObject hpPickup;
    public GameObject manaPickup;
    public TextMeshProUGUI HealCountText;
    public TextMeshProUGUI scoreCountText;
    public GameObject weaponChangePopup;
    public TextMeshProUGUI weaponChangeText;
    public GameObject needMoreGemsPopup;

    [Header("Game Goals")]
    public int GemsRemaining;
    public int scoreCount;
    public bool isPaused;
    private float timeScaleOriginal;
    private  float timeFixedOriginal;


    private void Awake()
    {
        
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = PlayerController.FindObjectOfType<PlayerController>();
        playerSpawnPosition = GameObject.FindGameObjectWithTag("Player Spawn Position");
        objectiveText.text = ("Remaining Gems:");
        KeyCountText.text = playerController.keysInPossession.ToString("F0");
        HealCountText.text = playerController.healItemCount.ToString("F0");
        SetScore(0);
        

        timeScaleOriginal = Time.timeScale;
        timeFixedOriginal = Time.fixedDeltaTime;
    }
    private void Start()
    {
        StartCoroutine(StartMenuFlash());
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);

            if (isPaused)
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
        GemsRemaining += amount;
        GemsRemainingText.text = GemsRemaining.ToString("F0");

        if (GemsRemaining < 1)
        {
            GemsRemainingText.text = ("");
            objectiveText.text = ("Find The Escape");
        }
    }
    public IEnumerator StartMenuFlash()
    {
        startMenu.SetActive(true);
        yield return new WaitForSeconds(5f);
        startMenu.SetActive(false);

    }

    public IEnumerator FlashKeyPopup()// Key
    {
        playerKeyPopup.SetActive(true);
        yield return new WaitForSeconds(1f);
        playerKeyPopup.SetActive(false);
    }
    public IEnumerator FlashHealItemPopup()// Key
    {
        HealItemPopup.SetActive(true);
        yield return new WaitForSeconds(1f);
        HealItemPopup.SetActive(false);
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
        SetScore(scoreCount - 50);
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
    public IEnumerator manaFlash()
    {
        GameManager.instance.manaPickup.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameManager.instance.manaPickup.SetActive(false);
    }

    public IEnumerator noKeysFlash()
    {
        GameManager.instance.noKeysPopup.SetActive(true);
        yield return new WaitForSeconds(2f);
        GameManager.instance.noKeysPopup.SetActive(false);
    }
    public IEnumerator usedKeyFlash()
    {
        GameManager.instance.usedKeyPopup.SetActive(true);
        yield return new WaitForSeconds(2f);
        GameManager.instance.usedKeyPopup.SetActive(false);
    }
    public IEnumerator NeedMoreGems()
    {
        needMoreGemsPopup.SetActive(true);
        yield return new WaitForSeconds(2f);
        needMoreGemsPopup.SetActive(false);
    }
    public void usedKey1()
    {
        StartCoroutine(usedKeyFlash());
    }

    public void StartSlowMotion(float sloMoTimeScale = 0.5f)
    {
        sloMoTimeScale = Mathf.Clamp(sloMoTimeScale, 0, 1);
        Time.timeScale = sloMoTimeScale;
        Time.fixedDeltaTime = timeFixedOriginal * sloMoTimeScale;
    }

    public void StopSlowMotion()
    {
        Time.timeScale = timeScaleOriginal;
        Time.fixedDeltaTime = timeFixedOriginal;
    }
    public void UpdateScore(int scorechange)
    {
        scoreCount += scorechange;
        scoreCountText.text = GameManager.instance.scoreCount.ToString("F0");
    }
    public void SetScore(int scorechange)
    {
        scoreCount = scorechange;
        if (scoreCount < 0)
        {
            scoreCount = 0;
        }
        scoreCountText.text = GameManager.instance.scoreCount.ToString("F0");
    }

    public void GemPickup()
    {
        GameUpdateGoal(-1);
    }
}
