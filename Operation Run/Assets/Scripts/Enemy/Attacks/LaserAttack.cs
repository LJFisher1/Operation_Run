using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : MonoBehaviour, IEnemyAttack
{

    [SerializeField] float followSpeed;
    [SerializeField] float aimTime;
    [SerializeField] float telegraphTime;
    [SerializeField] float decayAfterFire;
    private int damage;

    [SerializeField] float duration;
    [SerializeField] int range;
    [SerializeField] LineRenderer lineRend;
    [SerializeField] float lineDecaySpeed;
    [SerializeField] GameObject hitEffect;

    [SerializeField] Gradient aimColor;
    [SerializeField] Gradient telegraphFlashColor;
    [SerializeField] Gradient fireColor;
    [SerializeField] AudioClip aimSound;
    [Range(0,1)][SerializeField] float aimVol;
    [SerializeField] AudioClip fireSound;
    [Range(0, 1)] [SerializeField] float fireVol;
    [SerializeField] AudioSource aud;

    bool takingAim;
    bool telegraphing;
    bool hasFired;
    EnemyAI enemy;
    bool HasEnemy { get => (enemy == null || !enemy.IsAlive); }

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
        if (!HasEnemy)
        {
            if (takingAim || telegraphing)
            {
                lineRend.SetPosition(0, enemy.projectilePosition.position);
                Vector3 endpoint = GameManager.instance.player.transform.position;
                WallCheck(ref endpoint);
                lineRend.SetPosition(1, Vector3.Lerp(lineRend.GetPosition(1), endpoint, followSpeed * Time.deltaTime));

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
        else
        {
            this.StopAllCoroutines();
            Destroy(gameObject);
        }
        
    }

    IEnumerator Aim()
    {
        if (!HasEnemy)
        {
            aud.PlayOneShot(aimSound,aimVol);
            takingAim = true;
            lineRend.colorGradient = aimColor;
            yield return new WaitForSeconds(aimTime);
            takingAim = false;
        }
        StartCoroutine(FireTelegraph());
    }

    IEnumerator FireTelegraph()
    {
        if (!HasEnemy)
        {
            telegraphing = true;
            lineRend.colorGradient = telegraphFlashColor;
            yield return new WaitForSeconds(telegraphTime / 3);
            lineRend.colorGradient = aimColor;
            yield return new WaitForSeconds(telegraphTime / 3);
            lineRend.colorGradient = telegraphFlashColor;
            yield return new WaitForSeconds(telegraphTime / 3);
            telegraphing = false;
        }
        StartCoroutine(Fire());
    }
    IEnumerator Fire()
    {
        if (!HasEnemy)
        {
            aud.PlayOneShot(fireSound,aimVol);
            Instantiate(hitEffect, enemy.projectilePosition.position, enemy.transform.rotation);
            enemy.animator.SetTrigger("Shoot");
            lineRend.colorGradient = fireColor;
            RaycastHit hit;
            Vector3 direction = (lineRend.GetPosition(1) - enemy.projectilePosition.position).normalized;
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
            yield return new WaitForSeconds(decayAfterFire);
            hasFired = true;
        }
        Destroy(gameObject, duration);
    }

    bool WallCheck(ref Vector3 wallhit)
    {
        RaycastHit hit;
        Ray ray = new(enemy.projectilePosition.position, enemy.playerDirection);
        if (Physics.Raycast(ray, out hit, range))
        {
            if (!hit.transform.CompareTag("Player"))
            {
                wallhit = hit.point;
                return false;
            }
        }
        return true;
    }

    //void TeleportAbility(Vector3 pos)
    //{
    //    enemy.agent.enabled = false;
    //    enemy.transform.position = pos;
    //    enemy.agent.enabled = true;
    //}
}
