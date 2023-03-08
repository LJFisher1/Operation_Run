using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Componets -----")]
    [SerializeField] CharacterController controller;

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
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Weapons();
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
        }

        //player movement
        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        move = move.normalized;

        //move controller
        controller.Move(move * Time.deltaTime * walkSpeed);
        controller.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator UseWeapon()
    {
        isUsingWeapon = true;
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f,0.5f)), out hit, range))
        {
            Debug.Log(hit.transform.name);
            var target = hit.collider.GetComponent<IDamage>();
            if (target != null)
            {
                target.TakeDamage(weaponDamage);
            }
        }
        yield return new WaitForSeconds(useTime);
        isUsingWeapon = false;
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            GameManager.instance.PlayerDead();
        }
    }
}
