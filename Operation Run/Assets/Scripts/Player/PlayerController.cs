using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    /*TODO
     * Clean up weapon script/interaction
     * try to move weapon specific logic out of UseWeapon and into their own scripts
     */
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Animator weaponAnim;
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
    public int gemCount;
    [Range(2,5)][SerializeField] int forceDamingRate; // Lower # means farther push

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

    [Header("----- Weapon Stats -----")]
    [SerializeField] float wUseTime;
    [SerializeField] float wRange;
    [SerializeField] int wDamage;
    [SerializeField] MeshFilter wModel;
    [SerializeField] MeshRenderer wMaterial;
    [SerializeField] public Weapon weapon;

    public bool isUsingWeapon;

    // Start is called before the first frame update
    void Start()
    {
        HP = hpMax;
        MANA = manaMax;
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
        if (!GameManager.instance.isPaused)
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
            GameManager.instance.UpdateScore(-10);
        }
        if (Input.GetButton("Fire1") & !isUsingWeapon)
        {
            StartCoroutine(UseWeapon());
        }
    }

    void Movement()
    {
        appliedForce = Vector3.Lerp(appliedForce, Vector3.zero, Time.deltaTime * forceDamingRate);
        //gravity and jumping
        if(controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
            jumpsCur = 0;
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
        }

        //player movement input
        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        move = move.normalized;

        //move controller
        controller.Move(move * Time.deltaTime * walkSpeed);
        controller.Move((playerVelocity + appliedForce) * Time.deltaTime); // this needs to come after movement or it causes issues.
    }

    IEnumerator UseWeapon()
    {
        isUsingWeapon = true;
        if (weapon != null && mana >= 1)
        {
            MANA -= 1;
            weaponAnim.SetTrigger("Use Weapon");
            GameObject bulletClone = Instantiate(weapon.bullet, shootPointVisual.position, weapon.bullet.transform.rotation);
            IBullet specialBullet = bulletClone.GetComponent<IBullet>();
            if (specialBullet != null)
            {
                specialBullet.Initialize(weapon);
            }


            yield return new WaitForSeconds(wUseTime);
        }
        isUsingWeapon = false;
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(GameManager.instance.PlayerHitFlash());

        if (HP <= 0)
        {
            GameManager.instance.UpdateScore(-20);
            GameManager.instance.PlayerDead();
        }
    }

    public void Heal(int heal)
    {
        if (HP < hpMax)
        {
            StartCoroutine(GameManager.instance.hpFlash());
            HP += heal;
            if (HP > hpMax)
            {
                HP = hpMax;
            }
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
    }

    public void SpawnPlayer()
    {
        HP = hpMax;
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPosition.transform.position;
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
        keysInPossession++;
        GameManager.instance.KeyCountText.text = keysInPossession.ToString("F0");
    }

    public void KeyUsed()
    {
        PlayAud(soundKeyUse, volumeUseKey);
        keysInPossession--;
        GameManager.instance.KeyCountText.text = keysInPossession.ToString("F0");
    }
    
    public void ChangeWeapon(Weapon weap)
    {
        Debug.Log("ChangeWeapon");
        weapon = weap;
        wDamage = weap.damage;
        wRange = weap.range;
        wUseTime = weap.useTime;

        wModel.sharedMesh = weap.model.GetComponent<MeshFilter>().sharedMesh;
        wMaterial.sharedMaterial = weap.model.GetComponent<MeshRenderer>().sharedMaterial;
        weaponAnim.SetTrigger("Change Weapon");
        StartCoroutine(FlashWeaponText(weapon.name));
    }

    IEnumerator FlashWeaponText(string name)
    {
        if (GameManager.instance.weaponChangePopup != null) // another bizzare check needed to prevent errors when stoping the game in editor due to GiveWeaponOnDestory
        {
            GameManager.instance.weaponChangeText.text = $"Got Weapon: {weapon.name}";
            GameManager.instance.weaponChangePopup.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            GameManager.instance.weaponChangePopup.SetActive(false);
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
