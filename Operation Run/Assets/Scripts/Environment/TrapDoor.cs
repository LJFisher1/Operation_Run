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
    [Header("* Components")]
    [SerializeField] Transform doorLeft;
    [SerializeField] Transform doorRight;
    [SerializeField] Transform openRotationL;
    [SerializeField] Transform openRotationR;
    [SerializeField] BoxCollider trigger;

    private void Awake()
    {
        activated = false;
        trigger = GetComponent<BoxCollider>();
        trigger.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && trigger.enabled)
        {
            activated = true;
            trigger.enabled = false;
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
        yield return new WaitForSeconds(1);
        activated = false;
    }
}
