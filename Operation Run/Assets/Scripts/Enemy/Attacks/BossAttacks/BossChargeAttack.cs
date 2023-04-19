using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChargeAttack : MonoBehaviour, IBossAttack1
{
    int damage;

    bool dashing;
    bool hasHit;
    float dashSpeed;
    [SerializeField] float duration;
    [SerializeField] float bounceBackMultiplier;
    [SerializeField] float verticalInfluenceMult;
    [SerializeField] public Vector3 bouncBackDirInfluence;
    [SerializeField] Collider dashCollider;
    [SerializeField] GameObject shield;
    [SerializeField] float knockBackForce;
    GameObject shieldClone;
    [SerializeField] GameObject hitEffect;
    Vector3 dashDir;
    BossIA boss;

    public void Initialize(BossIA creator)
    {
        boss = creator;
        damage = creator.attackDamage;
        dashSpeed = creator.projectileSpeed;
        StartCoroutine(Charge());
    }
    private void Update()
    {
        if (boss != null)
        {
            transform.position = boss.projectilePosition.position;
            transform.rotation = boss.transform.rotation;
            if (!boss.IsAlive)
            {
                EndDash();
            }
        }
        if (boss == null) EndDash();

        if (dashing && boss != null)
        {
            boss.agent.Move(dashDir * dashSpeed * Time.deltaTime);
            //shieldClone.transform.position = transform.position;
            //shieldClone.transform.rotation = transform.rotation;
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        if (dashing && !hasHit)
        {
            hasHit = true;
            //Debug.Log(other.name);
            if (other.CompareTag("Player"))
            {

                IDamage dam = other.GetComponent<IDamage>();
                if (dam != null)
                {
                    dam.TakeDamage(damage);
                }
                GameManager.instance.playerController.ApplyForce(knockBackForce * (boss.playerDirection + bouncBackDirInfluence));
            }
            else if (other.CompareTag("breakablewall"))
            {
                IDamage dam = other.GetComponent<IDamage>();
                if (dam != null)
                {
                    dam.TakeDamage(damage, boss.gameObject);
                }
            }
            //enemy.agent.isStopped = false;
            //enemy.agent.Move(bounceBackMultiplier * dashSpeed * (-dashDir + bouncBackDirInfluence));
            //enemy.agent.enabled = true;
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
        //enemy.agent.isStopped = true;
        dashing = true;
        dashDir = boss.playerDirection;
        dashDir = new(dashDir.x, dashDir.y * verticalInfluenceMult, dashDir.z);
        //shieldClone = Instantiate(shield,transform.position,transform.rotation);
        dashCollider.GetComponent<MeshRenderer>().enabled = true;
        dashCollider.enabled = true;
        yield return new WaitForSeconds(duration / 2);
        dashing = false;
        EndDash();
    }

    void EndDash()
    {
        Destroy(shieldClone, duration / 5);
        Destroy(gameObject);
    }
}
