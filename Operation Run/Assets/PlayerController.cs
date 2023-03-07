using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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

    


    // Start is called before the first frame update
    void Start()
    {
        hpMax = hp;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

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
}
