using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("* Controls")]
    [SerializeField] bool active;
    [SerializeField] GameObject spawnObject;
    [Header("* Stuff")]
    [SerializeField] Transform chestLid;
    [SerializeField] Transform openQuaternion;
    [SerializeField] Transform spawnPosition;
    [SerializeField] BoxCollider trigger;
    [SerializeField] AudioClip[] audioOpen;
    [Range(0, 1)][SerializeField] float volume;
    private Quaternion chestOpenRotation;

    private void Awake()
    {
        chestOpenRotation = openQuaternion.rotation;
        active = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && trigger.enabled == true)
        {
            active = true;
            StartCoroutine(TurnOff());
        }
    }

    private void Update()
    {
        if(active)
        {
            chestLid.rotation = Quaternion.Lerp(chestLid.rotation, chestOpenRotation, Time.deltaTime * 5);
        }
    }

    IEnumerator TurnOff()
    {
        giveLoot();
        yield return new WaitForSeconds(1);
        trigger.enabled = false;
        active = false;
    }

    void giveLoot()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.PlayOneShot(audioOpen[0], volume);
        Instantiate(spawnObject, spawnPosition.position, Random.rotation);
        Debug.Log("giveloot");
    }

}
