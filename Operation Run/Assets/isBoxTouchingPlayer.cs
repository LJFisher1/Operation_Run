using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isBoxTouchingPlayer : MonoBehaviour
{
    [SerializeField] GameObject ceiling;
    public bool touchingPlayer;

    private void Awake()
    {
        touchingPlayer = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !touchingPlayer)
        {
            StartCoroutine(ceiling.GetComponent<CrushingCeiling>().DropCeiling());
            touchingPlayer = true;
        }
    }
}
