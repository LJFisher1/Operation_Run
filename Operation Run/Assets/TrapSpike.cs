using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpike : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] BoxCollider box;

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Player"))
        {
            Debug.Log("SPIKY");
        }
    }

    private void Awake()
    {
        animator.SetTrigger("open");
    }

    private void boxOn()
    {
        box.enabled = true;
    }

    private void boxOff()
    {
        box.enabled = false;
    }
}
