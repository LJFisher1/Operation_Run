using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] Transform chestLid;
    [SerializeField] Transform openQuaternion;
    [SerializeField] BoxCollider trigger;
    private Quaternion chestOpenRotation;

    private void Awake()
    {
        chestOpenRotation = openQuaternion.rotation;
        active = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            active = true;
        }
    }

    private void Update()
    {
        if(active)
        {
            chestLid.rotation = Quaternion.Lerp(chestLid.rotation, chestOpenRotation, Time.deltaTime * 5);
            StartCoroutine(TurnOff());
        }
    }

    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(1);
        trigger.enabled = false;
        active = false;
        giveLoot();
    }

    void giveLoot()
    {

    }

}
