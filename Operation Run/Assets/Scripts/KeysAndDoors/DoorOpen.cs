using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] Door door;
    [SerializeField] Keys key;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.instance.playerController.keyList.Contains(key))
        {
            //KEYS WITH LESS USES THAN THE DOOR COST CAN OPEN THE DOOR STILL. THIS IS INTENTIONAL FOR STRATIZISATION. 
            key.keyUses = -door.keyCost;
            if (key.keyUses <= 0)
            {
                GameManager.instance.playerController.keyList.Remove(key);
            }
            Destroy(gameObject);
        }
    }
}