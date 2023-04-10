using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunAttack : MonoBehaviour,IEnemyAttack
{
    int damage;
    [SerializeField] float duration;
    [SerializeField] int numShots;
    [SerializeField] GameObject bullet;
    [SerializeField] float spreadAngle;
    [SerializeField] float spreadOffset;

    EnemyAI enemy;

    public void Initialize(EnemyAI creator)
    {
        enemy = creator;
        damage = creator.attackDamage;
        for (int i=0; i < numShots; i++)
        {
            GameObject cbullet = Instantiate(bullet, transform.position, transform.rotation,transform);
            cbullet.GetComponent<Rigidbody>().velocity = GetRandomDirectionCone() * creator.projectileSpeed;
        }
        Destroy(gameObject, duration);

    }
    Vector3 GetRandomDirectionCone()
    {
        float angle = Random.Range(-spreadAngle, spreadAngle);
        Quaternion q = Quaternion.Euler(angle, 0, 0);
        return q * enemy.playerDirection + RandomSpreadOffset();
    }

    Vector3 RandomSpreadOffset()
    {
        return new Vector3(Random.Range(-spreadOffset, spreadOffset), Random.Range(-spreadOffset, spreadOffset), Random.Range(-spreadOffset, spreadOffset));
    }
}
