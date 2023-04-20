using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour , iDataPersistence
{
    public static GameManager instance;

    int sceneIndex;
    public bool isPaused;

    [Header("--- Player ---")]
    [SerializeField] float deathEffectDuration;
    [SerializeField] GameObject deadBody;
    public Camera mainCam;
    [HideInInspector] public GameObject player;
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public GameObject playerSpawnPosition;
    [HideInInspector] public GameObject deadBodyClone;

    [Header("--- Menus ---")]
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject settingsMenu;
    public GameObject guideMenu;
    
    public GameObject sensitivitySlider;
    [HideInInspector] public GameObject activeMenu;

    [Header("--- HUD ---")]
    public GameObject startPopup;
    public GameObject checkpointPopup;
    public GameObject playerHitFlash;
    public Image playerHealthBar;
    public Image playerHealthChangeBar;
    public Image playerManaBar;
    public Image playerManaChangeBar;
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
    public GameObject jumpPip1;
    public GameObject jumpPip2;
    public TextMeshProUGUI FPSText;

    [Header("--- Win Screen ---")]
    public TextMeshProUGUI enemyDefeatAmount;
    public TextMeshProUGUI enemyDefeatScore;
    public TextMeshProUGUI goldCollectedAmount;
    public TextMeshProUGUI goldCollectedScore;
    public TextMeshProUGUI gemsCollectedAmount;
    public TextMeshProUGUI gemsCollectedScore;
    public TextMeshProUGUI pickupsCollectedAmount;
    public TextMeshProUGUI pickupsCollectedScore;
    public TextMeshProUGUI keysCollectedAmount;
    public TextMeshProUGUI keysCollectedScore;
    public TextMeshProUGUI doorsOpenedAmount;
    public TextMeshProUGUI doorsOpenedScore;
    public TextMeshProUGUI wallsBustedAmount;
    public TextMeshProUGUI wallsBustedScore;
    public TextMeshProUGUI healsUsedAmount;
    public TextMeshProUGUI healsUsedScore;
    public TextMeshProUGUI timesDiedAmount;
    public TextMeshProUGUI timesDiedScore;
    public TextMeshProUGUI timeBonusScore;
    public TextMeshProUGUI totalScore;
    public TextMeshProUGUI Grade;
    public int enemysDefeated;
    public int goldCollected;
    public int gemsCollected;
    public int pickupsCollected;
    public int keysCollected;
    public int doorsOpened;
    public int wallsBusted;
    public int healsUsed;
    public int deaths;

    [Header("--- Guide Menu ---")]
    public TextMeshProUGUI gemsTip;
    public TextMeshProUGUI goldTip;
    public TextMeshProUGUI keysTip;
    public TextMeshProUGUI doorsTip;
    public TextMeshProUGUI healthTip;
    public TextMeshProUGUI manaTip;
    public TextMeshProUGUI wallsTip;
    public TextMeshProUGUI enemiesTip;
    public TextMeshProUGUI staffsTip;
    public TextMeshProUGUI hourglassesTip;
    public TextMeshProUGUI windTip;
    public TextMeshProUGUI guideTips;
    public GameObject levelLocked;
    public GameObject nextLevelButton;
    public GameObject mainMenuButton;

    [Header("--- Game Goals--- ")]
    public int GemsRemaining;
    public bool levelCompleted;

    private float timeScaleOriginal;
    private float timeFixedOriginal;

    [Header("--- Score ---")]
    public int scoreCount;
    public int PlayerHighScore;
    public int S = 300;
    public int A = 250;
    public int B = 200;
    public int C = 175;
    public int D = 150;

    Dictionary<string, int> scoretable = new Dictionary<string, int>()
    {
        {"default", 0},{"Gold", 0},{"Gem", 0},{"Door", 0},{"Key", 0},{"Pickup", 0},{"Wall", 0},{"Heal", 0},{"Death", 0},{"Time", 0}
    };
    Dictionary<string, int> rankTable;

    [Header("--- Timer ---")]
    public int timeScoreMax;
    public int timeScoreDedectionRate;
    public float currentTime;
    public TextMeshProUGUI timerText;

    [Header("--- Tutorials ---")]
    //public bool resetTutorials;
    public GameObject TutorialPopupGUI;
    public TextMeshProUGUI TutorialPopText;
    public TutorialManager tutorialManager;


    private void Awake()
    {
        rankTable = new() { {"S",S }, {"A",A}, {"B",B }, {"C",C }, {"D",D }, {"F", 0 } };
        instance = this;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        mainCam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = PlayerController.FindObjectOfType<PlayerController>();
        playerSpawnPosition = GameObject.FindGameObjectWithTag("Player Spawn Position");
        objectiveText.text = ("Remaining Gems:");
        KeyCountText.text = playerController.keysInPossession.ToString("F0");
        HealCountText.text = playerController.healItemCount.ToString("F0");

        timeScaleOriginal = Time.timeScale;
        timeFixedOriginal = Time.fixedDeltaTime;
    }
    private void Start()
    {
        GameUnpaused();

        if (SceneManager.GetActiveScene().buildIndex == 2) // level 1 after tutorial
        {
            StartCoroutine(FlashStartPopup());
        }
        SetScore(0);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {

            if (!isPaused)
            {
                GamePaused();
                
            }
            else
            {
                GameUnpaused();
            }

            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);

        }
        if (!isPaused)
        {
            currentTime += Time.deltaTime;
            UpdateTimer(currentTime);
        }

    }
    public void UpdateTimer(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        float fraction = (time * 10) % 10;
        if (fraction > 9) fraction = 0;

        timerText.text = string.Format("{0:00}:{1:00}.{2:0}", minutes, seconds, fraction);
    }
    public int CalculateTimeScore()
    {
        int bonus = (int)(timeScoreMax - (currentTime * timeScoreDedectionRate));
        if (bonus < 0) bonus = 0;
        return bonus;
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
    public IEnumerator FlashStartPopup()
    {
        startPopup.SetActive(true);
        yield return new WaitForSeconds(5f);
        startPopup.SetActive(false);

    }
    public IEnumerator FlashTutorialPopup(string name)
    {
        TutorialPopText.text = tutorialManager.GetTutText(name);
        TutorialPopupGUI.SetActive(true);
        yield return new WaitForSeconds(tutorialManager.TutorialDisplayTime);
        TutorialPopupGUI.SetActive(false);
        TutorialPopText.text = "tutorial text";
    }
    public IEnumerator FlashKeyPopup() //Key
    {
        playerKeyPopup.SetActive(true);
        yield return new WaitForSeconds(1f);
        playerKeyPopup.SetActive(false);
    }
    public IEnumerator FlashHealItemPopup() //Heal
    {
        HealItemPopup.SetActive(true);
        if (!tutorialManager.CheckCompleted("Heal Cap"))
        {
            StartCoroutine(FlashTutorialPopup("Heal Cap"));
            tutorialManager.SetTutorialCompletion("Heal Cap", true);
        }
        yield return new WaitForSeconds(1f);
        HealItemPopup.SetActive(false);
    }

    public IEnumerator FlashWeaponText(string name)
    {
        GameManager.instance.weaponChangeText.text = $"Got Weapon: {name}";
        if (!tutorialManager.CheckCompleted(name))
        {
            StartCoroutine(FlashTutorialPopup(name));
            tutorialManager.SetTutorialCompletion(name, true);
        }
        GameManager.instance.weaponChangePopup.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        GameManager.instance.weaponChangePopup.SetActive(false);
    }

    public void GamePaused()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void GameUnpaused()
    {
        isPaused = false;
        Time.timeScale = 1;
        UpdateSensitivity();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if(activeMenu != null) activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void PlayerWin() //here is where the score is calculated (Joe)
    {
        UpdateScore(CalculateTimeScore(), "Time");
        enemyDefeatAmount.text = enemysDefeated.ToString("F0");
        enemyDefeatScore.text = "X";
        goldCollectedAmount.text = goldCollected.ToString("F0");
        goldCollectedScore.text = scoretable["Gold"].ToString("F0");
        gemsCollectedAmount.text = gemsCollected.ToString("F0");
        gemsCollectedScore.text = scoretable["Gem"].ToString("F0");
        pickupsCollectedAmount.text = pickupsCollected.ToString("F0");
        pickupsCollectedScore.text = scoretable["Pickup"].ToString("F0");
        keysCollectedAmount.text = keysCollected.ToString("F0");
        keysCollectedScore.text = scoretable["Key"].ToString("F0");
        doorsOpenedAmount.text = doorsOpened.ToString("F0");
        doorsOpenedScore.text = scoretable["Door"].ToString("F0");
        wallsBustedAmount.text = wallsBusted.ToString("F0");
        wallsBustedScore.text = scoretable["Wall"].ToString("F0");
        healsUsedAmount.text = healsUsed.ToString("F0");
        healsUsedScore.text = scoretable["Heal"].ToString("F0");
        timesDiedAmount.text = deaths.ToString("F0");
        timesDiedScore.text = scoretable["Death"].ToString("F0");
        timeBonusScore.text = scoretable["Time"].ToString("F0");
        totalScore.text = scoreCount.ToString("F0");
        Grade.text = GetRank(scoreCount);
        Debug.Log(sceneIndex + 1);
        Debug.Log(SceneManager.sceneCountInBuildSettings);
        if (sceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("next level");
            mainMenuButton.SetActive(false);
            nextLevelButton.SetActive(true);
        }
        else
        {
            Debug.Log("return to main");
            nextLevelButton.SetActive(false);
            mainMenuButton.SetActive(true);
        }
        activeMenu = winMenu;
        activeMenu.SetActive(true);
        GamePaused();
        if (DataPersistence.instance != null)
        {
            DataPersistence.instance.SaveGame();
        }
        else
        {
            Debug.Log("No DataPersistance Instance found. Unable to save game.");
        }
    }

    string GetRank(int score)
    {
        foreach(KeyValuePair<string,int> kvp in rankTable)
        {
            if (scoreCount >= kvp.Value) return kvp.Key;
        }
        return "F";
    }

    public IEnumerator PlayerHitFlash()
    {
        playerHitFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerHitFlash.SetActive(false);
    }

    
    public IEnumerator PlayerDead()
    {
        deadBodyClone = Instantiate(deadBody, Camera.main.transform.position, Camera.main.transform.rotation);
        deadBodyClone.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2));
        deadBodyClone.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2));
        Camera.main.enabled = false;
        yield return new WaitForSeconds(deathEffectDuration);
        GamePaused();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
        SetScore(scoreCount - 50);
    }

    public void UpdateSensitivity()
    {
        if(Camera.main) Camera.main.GetComponent<CameraController>().UpdateSensitivity(sensitivitySlider.GetComponent<Slider>().value * 2);

    }

    public IEnumerator HPFlash()
    {
        GameManager.instance.hpPickup.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameManager.instance.hpPickup.SetActive(false);
    }
    public IEnumerator ManaFlash()
    {
        GameManager.instance.manaPickup.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameManager.instance.manaPickup.SetActive(false);
    }

    public IEnumerator NoKeysFlash()
    {
        GameManager.instance.noKeysPopup.SetActive(true);
        yield return new WaitForSeconds(2f);
        GameManager.instance.noKeysPopup.SetActive(false);
    }
    public IEnumerator UsedKeyFlash()
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
    public void UsedKey()
    {
        StartCoroutine(UsedKeyFlash());
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
    public void UpdateScore(int scorechange, string type = "default")
    {
        scoreCount += scorechange;
        scoreCountText.text = GameManager.instance.scoreCount.ToString("F0");
        if(scoretable.ContainsKey(type))
        {
            scoretable[type] += scorechange;
        }
    }
    public void SetScore(int scorechange)
    {
        scoreCount = scorechange;
        if (scoreCount < 0)
        {
            scoreCount = 0;
        }
        scoreCountText.text = GameManager.instance.scoreCount.ToString();
    }

    public void GemPickup()
    {
        GameUpdateGoal(-1);
    }

    public IEnumerator hpLossFlash()
    {
        playerHealthChangeBar.fillAmount = instance.playerController.playerHealthChange;
        yield return new WaitForSeconds(0.5f);
        playerHealthChangeBar.fillAmount = 0;
    }

    public IEnumerator manaLossFlash()
    {
        playerManaChangeBar.fillAmount = instance.playerController.playerManaChange;
        yield return new WaitForSeconds(0.5f);
        playerManaChangeBar.fillAmount = 0;
    }

    public void LoadData( GameData data)
    {
        Debug.Log(sceneIndex);
        this.PlayerHighScore = data.levels[sceneIndex].highScore;
        this.levelCompleted = data.levels[sceneIndex].completed;
        this.sensitivitySlider.GetComponent<Slider>().value = data.sensitivity / 2;
        Debug.Log(data.levels[sceneIndex].highScore);
    }

    public void SaveData(ref GameData data)
    {
        Debug.Log("Trying to save " + scoreCount);
        //if level has not been completed before or the you got a new high score
        if (!data.levels[sceneIndex].completed || scoreCount > PlayerHighScore)
        {
            data.CompleteLevel(sceneIndex, scoreCount);
        }
    }
    public void SaveSettings(ref GameData data) 
    {
        
    }
}
