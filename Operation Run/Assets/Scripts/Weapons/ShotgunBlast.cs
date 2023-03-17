using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBlast : MonoBehaviour,IBullet
{
    public int damage;
    public string hitTag = "Enemy";
    public float duration;
    public int numShots;
    public GameObject bullet;
    public float spreadAngle;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        
        if (other.CompareTag(hitTag))
        {
            other.GetComponent<IDamage>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    void IBullet.Initialize(Weapon creator)
    {
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
