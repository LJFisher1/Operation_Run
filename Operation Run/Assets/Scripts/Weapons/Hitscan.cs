using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitscan : MonoBehaviour, IBullet
{
    private int damage;
    public string hitTag = "Enemy";
    private float duration;
    private int range;
    [SerializeField] float SlomoDurationOnTP;
    public LineRenderer lineRend;
    [SerializeField] float decaySpeed;
    [SerializeField] GameObject hitEffect;

    public void Initialize(Weapon creator)
    {
        damage = creator.damage;
        duration = creator.duration;
        range = creator.range;

        RaycastHit hit;
        Ray ray = new(GameManager.instance.playerController.shootPointCenter.position, Camera.main.transform.forward);
        lineRend.SetPosition(0, GameManager.instance.playerController.shootPointVisual.position);
        lineRend.SetPosition(1, Camera.main.transform.forward * range);
        if (Physics.Raycast(ray, out hit, range))
        {
            var target = hit.collider.GetComponent<IDamage>();
            Debug.Log(hit.transform.name);
            if (target != null && hit.collider.CompareTag(hitTag))
            {
                target.TakeDamage(damage);
                if(hit.transform.GetComponent<EnemyAI>().HP <= 0 )
                {
                    TeleportAbility(hit.transform.position);
                }
            }
            Instantiate(hitEffect, hit.point, transform.rotation);
        }
        Destroy(gameObject, duration);
    }
    private void Update()
    {
        lineRend.endColor = Color.Lerp(lineRend.endColor, Color.clear, decaySpeed);
        lineRend.startColor = Color.Lerp(lineRend.startColor, Color.clear, decaySpeed);
        lineRend.endWidth = Mathf.Lerp(lineRend.endWidth, 0, decaySpeed);
        lineRend.startWidth = Mathf.Lerp(lineRend.startWidth, 0, decaySpeed);
    }

    void TeleportAbility(Vector3 pos)
    {
        GameManager.instance.playerController.Teleport(pos);
        StartCoroutine(SlowMo());
    }
    IEnumerator SlowMo()
    {
        GameManager.instance.StartSlowMotion(.1f);
        yield return new WaitForSecondsRealtime(SlomoDurationOnTP);
        GameManager.instance.StopSlowMotion();
    }



}
