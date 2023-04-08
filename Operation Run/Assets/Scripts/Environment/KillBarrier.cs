using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBarrier : MonoBehaviour
{
    [SerializeField] float damageDelay;
    [SerializeField] int damage = int.MaxValue;
    [SerializeField] bool hideOnPlay;
    bool isKilling;
    private void Start()
    {
        if (hideOnPlay)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
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
            if(damAble.IsAlive) damAble.TakeDamage(damage);
        }
        isKilling = false;
    }

}
