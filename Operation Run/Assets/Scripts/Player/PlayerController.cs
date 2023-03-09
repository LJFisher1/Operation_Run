using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Componets -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] LineRenderer lineRend;
    [SerializeField] Transform shootPoint;

    


    [Header("----- Player Stats -----")]
    [Range(0, 100)] [SerializeField] float walkSpeed;
    [Range(10, 35)] [SerializeField] float gravity;
    [Range(5, 15)] [SerializeField] float jumpSpeed;
    [Range(1, 3)] [SerializeField] int jumpsMax;
    int jumpsCur;
    Vector3 move;
    Vector3 playerVelocity;

    int hpMax;
    [Range(0,100)] [SerializeField] int hp;

    [Header("----- Weapon Stats -----")]
    [SerializeField] float useTime;
    [SerializeField] float range;
    [SerializeField] int weaponDamage;

    bool isUsingWeapon;

    // Start is called before the first frame update
    void Start()
    {
        hpMax = hp;
        UpdateHealthUI();
        if(GameManager.instance.playerSpawnPosition != null) // stops game from breaking if no spawn point set. Helps with testing.
        {
            SpawnPlayer();
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
        controller.Move(playerVelocity * Time.deltaTime); // this needs to come after movement or it causes issues.
    }

    IEnumerator UseWeapon()
    {
        isUsingWeapon = true;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f,0.5f)), out hit, range))
        {
            lineRend.enabled = true;
            lineRend.SetPosition(0, shootPoint.position);
            lineRend.SetPosition(1, hit.point);
            var target = hit.collider.GetComponent<IDamage>();
            Debug.Log(hit.transform.name);
            if (target != null)
            {
                target.TakeDamage(weaponDamage);
            }
        }
        yield return new WaitForSeconds(useTime);
        lineRend.enabled = false;
        isUsingWeapon = false;
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        UpdateHealthUI();
        StartCoroutine(GameManager.instance.PlayerHitFlash());

        if (hp <= 0)
        {
            GameManager.instance.PlayerDead();
        }
    }

    public void SpawnPlayer()
    {
        hp = hpMax;
        UpdateHealthUI();
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPosition.transform.position;
        controller.enabled = true;
    }

    public void UpdateHealthUI()
    {
        GameManager.instance.playerHealthBar.fillAmount = (float)hp / (float)hpMax;
    }
}
