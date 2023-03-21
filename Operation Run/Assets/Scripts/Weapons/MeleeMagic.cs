using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMagic : MonoBehaviour, IBullet
{
    int damage;
    float duration;
    bool dashing;
    float dashSpeed;
    [SerializeField] float bounceBackMultiplier;
    [SerializeField] public Vector3 bouncBackDirInfluence;
    [SerializeField] Collider dashCollider;
    [SerializeField] GameObject hitEffect;
    CharacterController cc;
    Vector3 dashDir;
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
        transform.rotation = GameManager.instance.playerController.transform.rotation;
        if (!GameManager.instance.playerController.IsAlive)
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (dashing)
        {
            Debug.Log(other.name);
            if (other.CompareTag("Enemy") || other.CompareTag("breakablewall"))
            {
                
                IDamage dam = other.GetComponent<IDamage>();
                if (dam!= null)
                {
                    dam.TakeDamage(damage);
                }
            }
            GameManager.instance.playerController.ApplyForce((-dashDir + bouncBackDirInfluence) * dashSpeed * bounceBackMultiplier);
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
        dashCollider.GetComponent<MeshRenderer>().enabled = true;
        Camera.main.GetComponent<CameraController>().enabled = false;
        GameManager.instance.playerController.ApplyForce(dashDir * dashSpeed);
        yield return new WaitForSeconds(duration/2);
        dashing = false;
        EndDash();
    }

    void EndDash()
    {
        Camera.main.GetComponent<CameraController>().enabled = true;
        Destroy(gameObject);
    }
}
