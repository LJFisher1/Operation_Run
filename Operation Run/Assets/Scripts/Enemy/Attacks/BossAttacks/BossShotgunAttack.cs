using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShotgunAttack : MonoBehaviour, IBossAttack1
{
    int damage;
    [SerializeField] float duration;
    [SerializeField] int numShots;
    [SerializeField] GameObject bullet;
    [SerializeField] float spreadAngle;
    [SerializeField] float spreadOffset;

    BossIA boss;

    public void Initialize(BossIA creator)
    {
        boss = creator;
        damage = creator.attackDamage;
        for (int i = 0; i < numShots; i++)
        {
            GameObject cbullet = Instantiate(bullet, transform.position, transform.rotation, transform);
            cbullet.GetComponent<Rigidbody>().velocity = GetRandomDirectionCone() * creator.projectileSpeed;
        }
        Destroy(gameObject, duration);

    }
    Vector3 GetRandomDirectionCone()
    {
        float angle = Random.Range(-spreadAngle, spreadAngle);
        Quaternion q = Quaternion.Euler(angle, 0, 0);
        return q * boss.playerDirection + RandomSpreadOffset();
    }

    Vector3 RandomSpreadOffset()
    {
        return new Vector3(Random.Range(-spreadOffset, spreadOffset), Random.Range(-spreadOffset, spreadOffset), Random.Range(-spreadOffset, spreadOffset));
    }
}
