using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMagic : MonoBehaviour, IBullet
{
    int damage;
    float duration;
    bool dashing;
    bool hasHit;
    float dashSpeed;
    [SerializeField] float bounceBackMultiplier;
    [SerializeField] float verticalInfluenceMult;
    [SerializeField] public Vector3 bouncBackDirInfluence;
    [SerializeField] Collider dashCollider;
    [SerializeField] GameObject shield;
    GameObject shieldClone;
    [SerializeField] GameObject hitEffect;
    PlayerController player;
    Vector3 dashDir;
    public void Initialize(Weapon creator)
    {
        damage = creator.damage;
        duration = creator.duration;
        dashSpeed = creator.bulletSpeed;
        player = GameManager.instance.playerController;
        StartCoroutine(Charge());
    }
    private void Update()
    {
        transform.position = player.shootPointVisual.position;
        transform.rotation = player.transform.rotation;
        if (shieldClone)
        {
            shieldClone.transform.position = transform.position;
            shieldClone.transform.rotation = transform.rotation;
        }
        if (!player.IsAlive)
        {
            EndDash();
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (dashing && !hasHit)
        {
            hasHit = true;
            Debug.Log(other.name);
            if (other.CompareTag("Enemy") || other.CompareTag("breakablewall"))
            {
                
                IDamage dam = other.GetComponent<IDamage>();
                if (dam!= null)
                {
                    dam.TakeDamage(damage);
                }
            }
            player.ApplyForce(bounceBackMultiplier * dashSpeed * (-dashDir + bouncBackDirInfluence));
            Instantiate(hitEffect, transform.position, hitEffect.transform.rotation);
            EndDash();
        }
    }

    IEnumerator Charge()
    {
        yield return new WaitForSeconds(duration);
        StartCoroutine(Dash());
    }
    IEnumerator Dash()
    {
        dashing = true;
        dashDir = Camera.main.transform.forward;
        dashDir = new(dashDir.x, dashDir.y * verticalInfluenceMult, dashDir.z);
        shieldClone = Instantiate(shield,transform.position,transform.rotation);
        dashCollider.GetComponent<MeshRenderer>().enabled = true;
        dashCollider.enabled = true;
        Camera.main.GetComponent<CameraController>().enabled = false;
        player.ApplyForce(dashDir * dashSpeed);
        yield return new WaitForSeconds(duration/2);
        dashing = false;
        EndDash();
    }

    void EndDash()
    {
        Camera.main.GetComponent<CameraController>().enabled = true;
        Destroy(shieldClone, duration/5);
        Destroy(gameObject);
    }
}
