using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : MonoBehaviour, IEnemyAttack
{
    private int damage;
    [SerializeField] float duration;
    [SerializeField] int range;
    [SerializeField] LineRenderer lineRend;
    [SerializeField] float lineDecaySpeed;
    [SerializeField] GameObject hitEffect;
    [SerializeField] float followSpeed;
    [SerializeField] float aimTime;
    [SerializeField] Gradient aimColor;
    bool takingAim;
    bool hasFired;
    EnemyAI enemy;

    public void Initialize(EnemyAI creator)
    {
        damage = creator.attackDamage;
        range = creator.attackDistance;
        enemy = creator;

        lineRend.SetPosition(0, enemy.projectilePosition.position);
        lineRend.SetPosition(1, GameManager.instance.player.transform.position);
        StartCoroutine(Aim());
    }
    private void Update()
    {
        if (takingAim)
        {
            lineRend.SetPosition(0, enemy.projectilePosition.position);
            lineRend.SetPosition(1, Vector3.Lerp(lineRend.GetPosition(1), GameManager.instance.player.transform.position, followSpeed * Time.deltaTime));

            //lineRend.endColor = Color.Lerp(Color.clear, Color.blue, lineDecaySpeed*10 * Time.deltaTime);
            //lineRend.startColor = Color.Lerp(Color.clear, Color.blue, lineDecaySpeed*10 * Time.deltaTime);
            //lineRend.endWidth = Mathf.Lerp(0, 5, lineDecaySpeed * Time.deltaTime);
            //lineRend.startWidth = Mathf.Lerp(0, 3, lineDecaySpeed * Time.deltaTime);
        }
        if (hasFired)
        {
            lineRend.endColor = Color.Lerp(lineRend.endColor, Color.clear, lineDecaySpeed * Time.deltaTime);
            lineRend.startColor = Color.Lerp(lineRend.startColor, Color.clear, lineDecaySpeed * Time.deltaTime);
            lineRend.endWidth = Mathf.Lerp(lineRend.endWidth, 0, lineDecaySpeed * Time.deltaTime);
            lineRend.startWidth = Mathf.Lerp(lineRend.startWidth, 0, lineDecaySpeed * Time.deltaTime);
        }

    }

    IEnumerator Aim()
    {
        takingAim = true;
        Gradient linecolor = lineRend.colorGradient;
        lineRend.colorGradient = aimColor;
        yield return new WaitForSeconds(aimTime);
        lineRend.colorGradient = linecolor;
        takingAim = false;
        Fire();
    }
    void Fire()
    {
        //play sound
        //play particle effect
        RaycastHit hit;
        Vector3 direction = (lineRend.GetPosition(1) - enemy.projectilePosition.position ).normalized;
        Ray ray = new(enemy.projectilePosition.position, direction);
        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log(hit.transform.name);
            var target = hit.collider.GetComponent<IDamage>();
            if (target != null && hit.transform.CompareTag("Player"))
            {
                target.TakeDamage(damage);
                //if (hit.transform.GetComponent<TeleportBarrel>() != null || !target.IsAlive)
                //{
                //    TeleportAbility(hit.transform.position);
                //}
            }
            Instantiate(hitEffect, hit.point, enemy.transform.rotation);
        }
        hasFired = true;
        Destroy(gameObject, duration);
    }

    //void TeleportAbility(Vector3 pos)
    //{
    //    enemy.agent.enabled = false;
    //    enemy.transform.position = pos;
    //    enemy.agent.enabled = true;
    //}
}
