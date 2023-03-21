using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMagic : MonoBehaviour, IBullet
{
    int damage;
    float duration;
    bool dashing;
    float dashSpeed;
    [SerializeField] float bounceBackDamping;
    [SerializeField] public Vector3 bouncBackDirInfluence;
    [SerializeField] Collider chargeCollder;
    [SerializeField] Collider dashCollider;
    [SerializeField] GameObject hitEffect;
    CharacterController cc;
    public Vector3 dashDir;
    public void Initialize(Weapon creator)
    {
        damage = creator.damage;
        duration = creator.duration;
        dashSpeed = creator.bulletSpeed;
        cc = GameManager.instance.player.GetComponent<CharacterController>();
        StartCoroutine(Charge());
    }
    private void Update()
    {
        transform.position = GameManager.instance.playerController.shootPointVisual.position;
    }
    public void OnTriggerStay(Collider other)
    {
        if (!dashing)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<IDamage>().TakeDamage(damage/2);
            }
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        if (dashing)
        {
            if (other.CompareTag("Enemy") || other.CompareTag("breakablewall"))
            {
                IDamage dam = other.GetComponent<IDamage>();
                if (dam!= null)
                {
                    dam.TakeDamage(damage);
                }
            }
            GameManager.instance.playerController.ApplyForce((-dashDir + bouncBackDirInfluence) * dashSpeed * bounceBackDamping);
            Instantiate(hitEffect, transform.position, hitEffect.transform.rotation);
            EndDash();
        }
    }

    IEnumerator Charge()
    {
        //Debug.Log("Charge");
        yield return new WaitForSeconds(duration);
        //Debug.Log("ChargeComplete");
        StartCoroutine(Dash());

        
    }
    IEnumerator Dash()
    {
        dashing = true;
        dashDir = Camera.main.transform.forward;
        Camera.main.GetComponent<CameraController>().enabled = false;
        GameManager.instance.playerController.ApplyForce(dashDir * dashSpeed);
        //chargeCollder.enabled = false;
        //dashCollider.enabled = true;
        yield return new WaitForSeconds(duration/2);
        EndDash();
    }

    void EndDash()
    {
        Camera.main.GetComponent<CameraController>().enabled = true;
        Destroy(gameObject);
    }
}
