using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    /*TODO
     * Clean up weapon script/interaction
     * try to move weapon specific logic out of UseWeapon and into their own scripts
     */
    [Header("----- Components -----")]
    [SerializeField] Collider playerCollider;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator weaponAnim;
    [SerializeField] Renderer model;

    //public Rigidbody deadHeadbody;
    Vector3 headPosition;
    
    /// <summary>
    /// to shoot from the weapon model 
    /// </summary>
    public Transform shootPointVisual;
    /// <summary>
    /// to shoot from the center of reticle/camera
    /// </summary>
    public Transform shootPointCenter;
    public Weapon startingWeapon;
    Vector3 appliedForce;
    [Header("---- Sounds ----")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] soundGoldPickUp;
    [Range(0, 1)][SerializeField] float volumeGoldPickUp;
    [SerializeField] AudioClip[] soundUseHeal;
    [Range(0, 1)][SerializeField] float volumeUseHeal;
    [SerializeField] AudioClip[] soundKeyUse;
    [Range(0, 1)][SerializeField] float volumeUseKey;
    [SerializeField] AudioClip[] playerFootstep;
    [Range(0, 1)] [SerializeField] float footstepVolume;
    [SerializeField] AudioClip[] playerJump;
    [Range(0, 1)] [SerializeField] float jumpVolume;
    [SerializeField] AudioClip[] playerDeath;
    [Range(0, 1)] [SerializeField] float deathVolume;
    //public AudioClip BackgroundMusic;
    //public AudioClip WinMenuMusic;

    [Header("----- Player Stats -----")]
    [Range(0, 100)] [SerializeField] float walkSpeed;
    [Range(10, 35)] [SerializeField] float gravity;
    [Range(5, 15)] [SerializeField] float jumpSpeed;
    [Range(1, 3)] [SerializeField] int jumpsMax;
    [Range(1, 100)] [SerializeField] int healPower;
    int jumpsCur;
    Vector3 move;
    Vector3 playerVelocity;
    public int keysInPossession;
    public int healItemCount;
    [Range(0, 100)] public int hpMax;
    int hp;
    int mana;
    [Range(0, 100)] public int manaMax;
    [Range(0, 100)] [SerializeField] int lowManaPercent = 25;
    int LowManaThreshold { get => manaMax / (100 / lowManaPercent); }
    public int gemCount;
    [Header("----- Forces -----")]
    [Range(2,5)][SerializeField] int forceDamingRate; // Lower # means farther push
    [Range(0, 5)][SerializeField] float forceCullingThreshold;
    [Range(0, 360)] [SerializeField] float ResitingForceAnge;
    [SerializeField] float resistMultiplier;
    [SerializeField] float resistGroundDrag;
    [SerializeField] Vector3 debugForce;
    [SerializeField] float debugMagnitude;
    [SerializeField] float resistThreshold;

    bool isPlayingFootsteps;

    public bool IsAlive {get => (hp > 0);}

    public int HP
    {
        set
        {
            hp = value;
            GameManager.instance.playerHealthBar.fillAmount = (float)hp / (float)hpMax;
        }
        get 
        { 
            return hp; 
        }
    }
    public int MANA
    {
        set
        {
            mana = value;
            GameManager.instance.playerManaBar.fillAmount = (float)mana / (float)manaMax;
        }
        get
        {
            return mana;
        }
    }
    public float playerHealthChange;
    public float playerManaChange;

    [Header("----- Weapon Stats -----")]
    [SerializeField] GameObject fizzleEffect;
    [SerializeField] float wUseTime;
    [SerializeField] float wRange;
    [SerializeField] int wDamage;
    [SerializeField] MeshFilter wModel;
    [SerializeField] MeshRenderer wMaterial;
    [SerializeField] public Weapon weapon;
    

    public bool isUsingWeapon;
    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider.enabled = true;
        GameManager.instance.FPSText.enabled=false;
        
        HP = hpMax;
        MANA = manaMax;
        //headPosition = deadHeadbody.position;
        //source.Play(BackgroundMusic);
        if (GameManager.instance.playerSpawnPosition != null) // stops game from breaking if no spawn point set. Helps with testing.
        {
            SpawnPlayer();
        }
        if(startingWeapon != null)
        {
            ChangeWeapon(startingWeapon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.F1)))
        {
           if(GameManager.instance.FPSText.enabled==true)
           {
            GameManager.instance.FPSText.enabled=false;

           }
            
            else
            GameManager.instance.FPSText.enabled=true;
        }

        if (!GameManager.instance.isPaused && IsAlive)
        {
            ItemControls();
            Movement();
        }
      
    }

    private void ItemControls()
    {
        if (Input.GetButtonDown("Heal") && healItemCount >= 1)
        {
            UseHealItem();
            GameManager.instance.UpdateScore(-10, "Heal");
        }
        if (Input.GetButton("Fire1") & !isUsingWeapon)
        {
            StartCoroutine(UseWeapon());
        }
    }

    void Movement()
    {

        //gravity and jumping
        if(controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
            jumpsCur = 0;
            GameManager.instance.jumpPip1.SetActive(true);
            GameManager.instance.jumpPip2.SetActive(true);
            if (!isPlayingFootsteps && move.normalized.magnitude > 0.5f)
            {
                StartCoroutine(PlayerFootsteps());
            }
        }
        else
        {
            playerVelocity.y -= gravity * Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && jumpsCur < jumpsMax)
        {
            PlayAud(playerJump, jumpVolume);
            playerVelocity.y = jumpSpeed;
            ++jumpsCur;
            if (jumpsCur == 1)
            {
                GameManager.instance.jumpPip1.SetActive(false);
            }
            if (jumpsCur ==2)
            {
                GameManager.instance.jumpPip2.SetActive(false);
            }
        }

        //player movement input
        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        move = move.normalized;
        move *= Time.deltaTime * walkSpeed;

        //applied force
        if (appliedForce != Vector3.zero)
        {
            appliedForce = Vector3.Lerp(appliedForce, Vector3.zero, Time.deltaTime * forceDamingRate);
            float angle = Vector3.Angle(appliedForce, -move);
            if(angle < ResitingForceAnge && move != Vector3.zero && appliedForce.magnitude < resistThreshold)
            {
                Debug.Log("resisting");
                appliedForce = Vector3.Lerp(appliedForce, Vector3.zero, move.magnitude * resistMultiplier);
                if (controller.isGrounded) appliedForce = Vector3.Lerp(appliedForce, Vector3.zero, move.magnitude * resistGroundDrag);
            }
            debugForce = appliedForce;
            debugMagnitude = appliedForce.magnitude;
            if (appliedForce.magnitude <= forceCullingThreshold)
            {
                appliedForce = Vector3.zero;
            }
        }

        //move controller
        controller.Move(move);
        controller.Move((playerVelocity + appliedForce) * Time.deltaTime); // this needs to come after movement or it causes issues.
    }

    IEnumerator UseWeapon()
    {
        isUsingWeapon = true;
        if (weapon != null)
        {
            if (MANA >= 1)
            {
                playerManaChange = MANA;
                StartCoroutine(GameManager.instance.manaLossFlash());
                MANA -= 1;
                weaponAnim.SetTrigger("Use Weapon");
                GameObject bulletClone = Instantiate(weapon.bullet, shootPointVisual.position, weapon.bullet.transform.rotation);
                IBullet specialBullet = bulletClone.GetComponent<IBullet>();
                if (specialBullet != null)
                {
                    specialBullet.Initialize(weapon);
                }
            }
            else
            {
                weaponAnim.SetTrigger("No Mana");
                Instantiate(fizzleEffect, shootPointVisual.position, transform.rotation);
            }
            yield return new WaitForSeconds(wUseTime);
            
        }
        isUsingWeapon = false;
    }

    public void TakeDamage(int dmg)
    {
        if(IsAlive) StartCoroutine(GameManager.instance.PlayerHitFlash());
        playerHealthChange = HP;
        HP -= dmg;
        StartCoroutine(GameManager.instance.hpLossFlash());
        if (!IsAlive) // death
        {
            playerCollider.enabled = false;
            controller.enabled = false;
            model.enabled = false;
            wMaterial.enabled = false;
            PlayAud(playerDeath, deathVolume);
            GameManager.instance.UpdateScore(-20, "Death");
            ++GameManager.instance.deaths;
            StartCoroutine(GameManager.instance.PlayerDead());
        }
    }

    public void Heal(int heal)
    {
        if (HP < hpMax)
        {
            StartCoroutine(GameManager.instance.HPFlash());
            HP += heal;
            if (HP > hpMax)
            {
                HP = hpMax;
            }
        }
    }
    public void AddMana()
    {
        StartCoroutine(GameManager.instance.ManaFlash());
        MANA += 5;
        if (MANA > manaMax)
        {
            MANA = manaMax;
        }
    }
    public void PickupHealItem()
    {
        StartCoroutine(GameManager.instance.FlashHealItemPopup());
        ++healItemCount;
        GameManager.instance.HealCountText.text = healItemCount.ToString("F0");
    }

    void UseHealItem()
    {
        PlayAud(soundUseHeal, volumeUseHeal);
        Heal(healPower);
        --healItemCount;
        GameManager.instance.HealCountText.text = healItemCount.ToString("F0");
        ++GameManager.instance.healsUsed;
    }

    public void SpawnPlayer()
    {
        playerCollider.enabled = true;
        controller.enabled = true;
        model.enabled = true;
        wMaterial.enabled = true;
        HP = hpMax;
        if(GameManager.instance.deadBodyClone != null)
        {
            GameManager.instance.deadBodyClone.GetComponent<Camera>().enabled = false;
            GetComponentInChildren<Camera>().enabled = true;
            Destroy(GameManager.instance.deadBodyClone);
        }
        Camera.main.GetComponent<CameraController>().enabled = true;
        if (MANA < LowManaThreshold) MANA = LowManaThreshold;
        controller.enabled = false;
        if(GameManager.instance.playerSpawnPosition != null) transform.position = GameManager.instance.playerSpawnPosition.transform.position;
        controller.enabled = true;
        move = Vector3.zero;
        playerVelocity = Vector3.zero;
        appliedForce = Vector3.zero;
    }


    public void Teleport(Vector3 pos)
    {
        controller.enabled = false;
        transform.position = pos;
        controller.enabled = true;
    }
    public void KeyPickup()
    {
        StartCoroutine(GameManager.instance.FlashKeyPopup());
        ++keysInPossession;
        GameManager.instance.KeyCountText.text = keysInPossession.ToString("F0");
    }

    public void KeyUsed()
    {
        PlayAud(soundKeyUse, volumeUseKey);
        --keysInPossession;
        GameManager.instance.KeyCountText.text = keysInPossession.ToString("F0");
    }
    
    public void ChangeWeapon(Weapon weap)
    {
        //Debug.Log("ChangeWeapon");
        if(weapon != weap)
        {
            weapon = weap;
            wDamage = weap.damage;
            wRange = weap.range;
            wUseTime = weap.useTime;

            wModel.sharedMesh = weap.model.GetComponent<MeshFilter>().sharedMesh;
            wMaterial.sharedMaterial = weap.model.GetComponent<MeshRenderer>().sharedMaterial;
            weaponAnim.SetTrigger("Change Weapon");
            if(GameManager.instance != null && gameObject.activeSelf) StartCoroutine(GameManager.instance.FlashWeaponText(weapon.name));
        }
    }


    public void ApplyForce(Vector3 amount)
    {
        appliedForce += amount;
    }

    public void PlayAud(AudioClip[] audioClips, float volume)
    {
        audioSource.PlayOneShot(audioClips[UnityEngine.Random.Range(0, audioClips.Length - 1)], volume);
    }

    IEnumerator PlayerFootsteps()
    {
        isPlayingFootsteps = true;
        audioSource.PlayOneShot(playerFootstep[UnityEngine.Random.Range(0, playerFootstep.Length - 1)], footstepVolume);
        yield return new WaitForSeconds(0.5f);
        isPlayingFootsteps = false;
    }
}
