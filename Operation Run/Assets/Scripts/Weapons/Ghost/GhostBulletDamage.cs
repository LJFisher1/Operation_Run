using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBulletDamage : MonoBehaviour
{
    [SerializeField] GhostBullet gb;
    private void OnTriggerStay(Collider other)
    {
        IDamage damAble = other.GetComponent<IDamage>();
        if(damAble != null)
        {
            damAble.TakeDamage(gb.damage);
            GhostBullet.playerHasGhost = false;
            if(!damAble.IsAlive) Destroy(gb.gameObject);
        }
    }
}
