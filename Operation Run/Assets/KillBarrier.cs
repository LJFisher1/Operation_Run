using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBarrier : MonoBehaviour
{
    [SerializeField] float damageDelay;
    [SerializeField] int damage = int.MaxValue;
    bool isKilling;
    private void OnTriggerStay(Collider other)
    {
       if(!isKilling) StartCoroutine(Kill(other));
    }
    IEnumerator Kill(Collider other)
    {
        isKilling = true;
        IDamage damAble = other.GetComponent<IDamage>();
        yield return new WaitForSeconds(damageDelay);
        if (damAble != null)
        {
            damAble.TakeDamage(damage);
        }
        isKilling = false;
    }

}
