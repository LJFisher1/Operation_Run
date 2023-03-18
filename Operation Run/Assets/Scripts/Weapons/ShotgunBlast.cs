using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBlast : MonoBehaviour,IBullet
{
    int damage;
    float duration;
    [SerializeField] string hitTag = "Enemy";
    [SerializeField] int numShots;
    [SerializeField] GameObject bullet;
    [SerializeField] float spreadAngle;
    [SerializeField] float pushBackPower;

    void IBullet.Initialize(Weapon creator)
    {
        pushBackPower = creator.bulletSpeed / 2;
        GameManager.instance.playerController.ApplyForce(-Camera.main.transform.forward * pushBackPower);
        damage = creator.damage;
        duration = creator.duration;
        for (int i=0; i < numShots; i++)
        {
            GameObject cbullet = Instantiate(bullet, transform.position, transform.rotation,transform);
            cbullet.GetComponent<Rigidbody>().velocity = GetRandomDirectionCone() * creator.bulletSpeed;
        }
        Destroy(gameObject, duration);

    }
    Vector3 GetRandomDirectionCone()
    {
        float angle = Random.Range(-spreadAngle, spreadAngle);
        Quaternion q = Quaternion.Euler(angle, 0, 0);
        return q * Camera.main.transform.forward;
    }
}
