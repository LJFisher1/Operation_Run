using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    /*TODO
     * Clean up weapon script/interaction
     * try to move weapon specific logic out of UseWeapon and into their own scripts
     */
    [Header("----- Componets -----")]
    [SerializeField] CharacterController controller;
    /// <summary>
    /// to shoot from the weapon model 
    /// </summary>
    public Transform shootPointVisual;
    /// <summary>
    /// to shoot from the center of reticle/camera
    /// </summary>
    public Transform shootPointCenter;
    public Weapon startingWeapon;

    Vector3 windPush;


    [Header("----- Player Stats -----")]
    [Range(0, 100)] [SerializeField] float walkSpeed;
    [Range(10, 35)] [SerializeField] float gravity;
    [Range(5, 15)] [SerializeField] float jumpSpeed;
    [Range(1, 3)] [SerializeField] int jumpsMax;
    int jumpsCur;
    Vector3 move;
    Vector3 playerVelocity;
    public int keysInPossession;
    int hp;
    [Range(2,5)][SerializeField] int pushbackTime; // Lower # means farther push

    public int HP
    {
        set
        {
            hp = value;
            GameManager.instance.playerHealthBar.fillAmount = (float)hp / (float)hpMax;
        }
        get { return hp; }
        
    }
    [Range(0,100)] public int hpMax;

    [Header("----- Weapon Stats -----")]
    [SerializeField] float wUseTime;
    [SerializeField] float wRange;
    [SerializeField] int wDamage;
    [SerializeField] MeshFilter wModel;
    [SerializeField] MeshRenderer wMaterial;
    [SerializeField] Weapon weapon;

    bool isUsingWeapon;

    // Start is called before the first frame update
    void Start()
    {
        HP = hpMax;
        if(GameManager.instance.playerSpawnPosition != null) // stops game from breaking if no spawn point set. Helps with testing.
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
            Movement();
            Weapons();
        }
      
    }

    private void Weapons()
    {
        if (Input.GetButton("Fire1") & !isUsingWeapon)
        {
            StartCoroutine(UseWeapon());
        }
    }

    void Movement()
    {
        windPush = Vector3.Lerp(windPush, Vector3.zero, Time.deltaTime * pushbackTime);
        //gravity and jumping
        if(controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
            jumpsCur = 0;
        }
        else
        {
            playerVelocity.y -= gravity * Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && jumpsCur < jumpsMax)
        {
            playerVelocity.y = jumpSpeed;
            ++jumpsCur;
        }

        //player movement input
        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        move = move.normalized;

        //move controller
        controller.Move(move * Time.deltaTime * walkSpeed);
        controller.Move((playerVelocity + windPush) * Time.deltaTime); // this needs to come after movement or it causes issues.
    }

    IEnumerator UseWeapon()
    {
        isUsingWeapon = true;
        if (weapon != null)
        {
            GameObject bulletClone = Instantiate(weapon.bullet, shootPointVisual.position, weapon.bullet.transform.rotation);
            bulletClone.GetComponent<IBullet>().Initialize(weapon);

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

    public void SpawnPlayer()
    {
        HP = hpMax;
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPosition.transform.position;
        controller.enabled = true;
    }


    public void Teleport(Vector3 pos)
    {
        controller.enabled = false;
        transform.position = pos;
        controller.enabled = true;
    }
    public void KeyPickup()
    {
        Debug.Log("flashKEY");
        StartCoroutine(GameManager.instance.FlashKeyPopup());
        keysInPossession++;
        GameManager.instance.KeyCountText.text = keysInPossession.ToString("F0");
    }

    public void KeyUsed()
    {
        keysInPossession--;
        GameManager.instance.KeyCountText.text = keysInPossession.ToString("F0");
    }

    

    public void ChangeWeapon(Weapon weap)
    {
        weapon = weap;
        wDamage = weap.damage;
        wRange = weap.range;
        wUseTime = weap.useTime;

        wModel.sharedMesh = weap.model.GetComponent<MeshFilter>().sharedMesh;
        wMaterial.sharedMaterial = weap.model.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void windPushback(Vector3 amount)
    {
        windPush += amount;
    }
}
