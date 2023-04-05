using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] Transform chestLid;
    [SerializeField] Quaternion chestOpenRotation;

    private void Awake()
    {
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
            chestLid.rotation = Quaternion.Lerp(chestLid.rotation, chestOpenRotation, Time.deltaTime);
        }
    }
}
