using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    [Header("* Controls")]
    [SerializeField] bool activated;
    [Header("* Settings")]
    [SerializeField] int speed;
    [Range(0, 1)][SerializeField] float volume;
    [Header("* Components")]
    [SerializeField] Transform doorLeft;
    [SerializeField] Transform doorRight;
    [SerializeField] Transform openRotationL;
    [SerializeField] Transform openRotationR;
    [SerializeField] AudioClip[] audioClips;
    BoxCollider trigger;
    AudioSource audioSource;

    private void Awake()
    {
        activated = false;
        trigger = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
        trigger.enabled = true;
        audioSource.pitch = (Random.Range(0.5f, 0.7f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trigger.enabled = false;
            activated = true;
            audioSource.PlayOneShot(audioClips[0]);
            StartCoroutine(TurnOff());
        }
    }

    private void Update()
    {
        if(activated)
        {
            doorLeft.rotation = Quaternion.Lerp(doorLeft.rotation, openRotationL.rotation, Time.deltaTime * speed);
            doorRight.rotation = Quaternion.Lerp(doorRight.rotation, openRotationR.rotation, Time.deltaTime * speed);
        }
    }

    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(1f);
        activated = false;
    }
}
