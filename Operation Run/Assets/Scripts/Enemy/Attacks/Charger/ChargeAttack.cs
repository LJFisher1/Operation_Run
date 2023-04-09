using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : MonoBehaviour, IEnemyAttack
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
    GameObject shieldClone;
    [SerializeField] GameObject hitEffect;
    Vector3 dashDir;
    EnemyAI enemy;
    public void Initialize(EnemyAI creator)
    {
        enemy = creator;
        damage = creator.attackDamage;
        dashSpeed = creator.projectileSpeed;
        StartCoroutine(Charge());
    }
    private void Update()
    {
        if (enemy != null)
        {
            transform.position = enemy.projectilePosition.position;
            transform.rotation = enemy.transform.rotation;
            if (!enemy.IsAlive)
            {
                EndDash();
            }
        }

        if(dashing)
        {
            enemy.agent.Move(dashDir * dashSpeed * Time.deltaTime);
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
                if (dam!= null)
                {
                    dam.TakeDamage(damage);
                }
            }
            else if(other.CompareTag("breakablewall"))
            {
                IDamage dam = other.GetComponent<IDamage>();
                if (dam != null)
                {
                    dam.TakeDamage(damage,enemy.gameObject);
                }
            }
            //enemy.agent.isStopped = false;
            enemy.agent.Move(bounceBackMultiplier * dashSpeed * (-dashDir + bouncBackDirInfluence));
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
        dashDir = enemy.playerDirection;
        dashDir = new(dashDir.x, dashDir.y * verticalInfluenceMult, dashDir.z);
        //shieldClone = Instantiate(shield,transform.position,transform.rotation);
        dashCollider.GetComponent<MeshRenderer>().enabled = true;
        dashCollider.enabled = true;
        yield return new WaitForSeconds(duration/2);
        dashing = false;
        EndDash();
    }

    void EndDash()
    {
        Destroy(shieldClone, duration/5);
        Destroy(gameObject);
    }
}
