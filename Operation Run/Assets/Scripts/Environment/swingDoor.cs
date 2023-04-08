using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class swingDoor : MonoBehaviour
{
    [Header("* Controls")]
    [SerializeField] bool interaction;
    [SerializeField] bool doorLocked;
    [SerializeField] bool doorOpen;
    [Header("* Settings")]
    [SerializeField] float speed;
    [SerializeField] Quaternion rotationClose;
    [Header("* Components")]
    [SerializeField] Transform rotationOpenL;
    [SerializeField] Transform rotationOpenR;
    [SerializeField] Transform doorLeft;
    [SerializeField] Transform doorRight;
    [SerializeField] GameObject padlock;
    [SerializeField] AudioClip[] audioUnlock;
    [SerializeField] AudioClip[] audioOpenDoor;
    [SerializeField] AudioClip[] audioCloseDoor;
    [SerializeField] AudioSource audioSource;
    [Range(0, 1)][SerializeField] float volume;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rotationClose = doorLeft.rotation;
        interaction = false;
        doorOpen = false;
        if(doorLocked)
        {
            padlock.SetActive(true);
        }
        else
        {
            padlock.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") & doorOpen == false)
        {
            StartCoroutine(TurnOffDoor());
            if (doorLocked && GameManager.instance.playerController.keysInPossession > 0)
            {
                StartCoroutine(UnlockDoor());
            }
            else if (doorLocked == false)
            {
                interaction = true;
                doorOpen = true;
            }           
        }
    }

    private void Update()
    {
        if(interaction)
        {
            MoveDoor();           
        }
    }

    IEnumerator UnlockDoor()
    {
        GameManager.instance.playerController.KeyUsed();
        doorLocked = false;
        padlock.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        interaction = true;
        doorOpen = true;
    }

    void MoveDoor()
    {
        if (doorOpen)
        {
            doorLeft.rotation = Quaternion.Lerp(doorLeft.rotation, rotationOpenL.transform.rotation, Time.deltaTime * speed);
            doorRight.rotation = Quaternion.Lerp(doorRight.rotation, rotationOpenR.transform.rotation, Time.deltaTime * speed);
        }
        else
        {
            doorLeft.rotation = Quaternion.Lerp(doorLeft.rotation, rotationClose, Time.deltaTime * speed);
            doorRight.rotation = Quaternion.Lerp(doorRight.rotation, rotationClose, Time.deltaTime * speed);
        }
    }

    IEnumerator TurnOffDoor()
    {
        audioSource.PlayOneShot(audioOpenDoor[0], volume);
        yield return new WaitForSeconds(1f);
        interaction = false;
    }
}
