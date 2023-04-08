using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorOpen : MonoBehaviour
{
    [Header("---- Settings ----")]
    [SerializeField] float doorSpeed;
    public Transform movePosition;
    Vector3 moveVector;
    bool moving;
    [SerializeField] BoxCollider boxTrigger;

    [Header("---- Audio ----")]
    [SerializeField] AudioClip[] doorOpen;
    [Range(0, 1)] [SerializeField] float volumeAudio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.instance.playerController.keysInPossession >= 1)
        {
            GameManager.instance.playerController.PlayAud(doorOpen, volumeAudio);
            GameManager.instance.playerController.KeyUsed();
            GameManager.instance.UsedKey();
            GameManager.instance.UpdateScore(5);
            moving = true;
            boxTrigger.enabled = false;
        }
        else if (other.CompareTag("Player") && GameManager.instance.playerController.keysInPossession <= 0)
        {
            StartCoroutine(GameManager.instance.NoKeysFlash());
        }
    }

    private void Start()
    {
        moveVector = movePosition.position;
    }

    private void Update()
    {
        if(moving)
        {
            slideTheDoor();
        }
    }

    void slideTheDoor()
    {
        float distance = Vector3.Distance(transform.position, moveVector);
        //Debug.Log(distance);
        if (distance > 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, moveVector, Time.deltaTime * doorSpeed);
        }
        else { Destroy(gameObject); }
    }
}