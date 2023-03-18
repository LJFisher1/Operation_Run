using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMagic : MonoBehaviour, IBullet
{
    int damage;
    float duration;
    bool dashing;
    float dashSpeed;
    [SerializeField] Collider chargeCollder;
    [SerializeField] Collider dashCollider;
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
        if(dashing)
        {
            Debug.Log("dash");
            cc.Move(dashDir * dashSpeed * Time.deltaTime);
        }
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
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<IDamage>().TakeDamage(damage);
            }
        }
    }

    IEnumerator Charge()
    {
        Debug.Log("Charge");
        yield return new WaitForSeconds(duration);
        Debug.Log("ChargeComplete");
        StartCoroutine(Dash());

        
    }
    IEnumerator Dash()
    {
        dashing = true;
        dashDir = Camera.main.transform.forward;
        Camera.main.GetComponent<CameraController>().enabled = false;
        //chargeCollder.enabled = false;
        //dashCollider.enabled = true;
        yield return new WaitForSeconds(duration/2);
        Camera.main.GetComponent<CameraController>().enabled = true;
        dashing = false;
        Destroy(gameObject,1);
    }
}
