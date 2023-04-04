using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBulletSeeking : MonoBehaviour
{
    [SerializeField] GhostBullet gb;
    GameObject target;
    IDamage targetDam;
    private void OnTriggerEnter(Collider other)
    {
        if (target == null)
        {
            targetDam = other.GetComponent<IDamage>();
            if (targetDam != null)
            {
                target = other.gameObject;
            }
        }
    }
    private void Update()
    {
        if (target != null)
        {
            gb.rb.velocity = Vector3.Lerp(gb.rb.velocity, GetDirection() * gb.bulletSpeed, gb.seekingStrength * Time.deltaTime);
            
            if (!targetDam.IsAlive) target = null;
            gb.seekingStrength = Mathf.Lerp(gb.seekingStrength, gb.seekingStrength + 1, 5 * Time.deltaTime);
        }
    }

    Vector3 GetDirection()
    {
        return (target.transform.position - transform.position).normalized;
    }
}
