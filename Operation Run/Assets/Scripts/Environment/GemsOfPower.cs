using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemsOfPower : MonoBehaviour
{
    [SerializeField] AudioClip[] gemPickup;
    [Range(0, 1)] [SerializeField] float audioVol;
    private void Start()
    {
        GameManager.instance.GameUpdateGoal(1);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.GemPickup();
            GameManager.instance.playerController.PlayAud(gemPickup, audioVol);
            Destroy(gameObject);
            GameManager.instance.UpdateScore(10);
        }
    }
}
