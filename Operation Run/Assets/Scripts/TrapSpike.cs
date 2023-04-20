using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpike : MonoBehaviour
{
    [Header("* Controls")]
    [SerializeField] bool activate;
    [SerializeField] bool position;
    [Header("* Settings")]
    [SerializeField] float speedUp;
    [SerializeField] float speedDown;
    [SerializeField] float resetTime;
    [Header("* Components")]
    [SerializeField] Collider trigger;
    [SerializeField] Transform spikes;
    [SerializeField] Transform up;
    [SerializeField] Transform down;
    [SerializeField] AudioSource audioSource;
    [Range(0, 1)][SerializeField] float audioVolume;
    [SerializeField] AudioClip[] audioClips;

    private void Awake()
    {
        activate = false;
        trigger.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(Activate());
        }
    }

    IEnumerator Activate()
    {
        audioSource.PlayOneShot(audioClips[0], audioVolume);
        position = true;
        activate = true;
        trigger.enabled = false;
        yield return new WaitForSeconds(1);
        position = false;
        yield return new WaitForSeconds(resetTime);
        activate = false;
        trigger.enabled = true;
    }

    private void Update()
    {
        if (activate)
        {
            if(position)
            {
                spikes.position = Vector3.Lerp(spikes.position, up.position, Time.deltaTime * speedUp);
            }
            else
            {
                spikes.position = Vector3.Lerp(spikes.position, down.position, Time.deltaTime * speedDown);
            }
        }
    }
}
