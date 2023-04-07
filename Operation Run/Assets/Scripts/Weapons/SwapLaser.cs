using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapLaser : MonoBehaviour, IBullet
{
    private int damage;
    Camera cam;
    private float duration;
    private int range;
    float sloMoDuration;
    float teleportDelay;
    [SerializeField] LineRenderer lineRend;
    [SerializeField] float lineDecaySpeed;
    [SerializeField] GameObject hitEffect;
    [Range(-60, 60)] [SerializeField] float fovWarpAmount;
    [SerializeField] float fovWarpSpeed;
    float orignalFov;
    bool cameraWarping;

    public void Initialize(Weapon creator)
    {
        cam = Camera.main;
        damage = creator.damage;
        sloMoDuration = creator.duration;
        teleportDelay = creator.bulletSpeed;
        range = creator.range;
        orignalFov = cam.fieldOfView;

        RaycastHit hit;
        Ray ray = new(GameManager.instance.playerController.shootPointCenter.position, cam.transform.forward);
        lineRend.SetPosition(0, GameManager.instance.playerController.shootPointVisual.position);
        lineRend.SetPosition(1, cam.transform.forward * range);
        if (Physics.Raycast(ray, out hit, range))
        {
            var target = hit.collider.GetComponent<IDamage>();
            //Debug.Log(hit.transform.name);
            if (target != null)
            {
                target.TakeDamage(damage);
                if( hit.transform.GetComponent<TeleportBarrel>() != null || !target.IsAlive)
                {
                    StartCoroutine(TeleportAbility(hit.transform.position));
                }
            }
            Instantiate(hitEffect, hit.point, GameManager.instance.player.transform.rotation);
        }
        
    }
    private void Update()
    {
        lineRend.endColor = Color.Lerp(lineRend.endColor, Color.clear, lineDecaySpeed);
        lineRend.startColor = Color.Lerp(lineRend.startColor, Color.clear, lineDecaySpeed);
        lineRend.endWidth = Mathf.Lerp(lineRend.endWidth, 0, lineDecaySpeed);
        lineRend.startWidth = Mathf.Lerp(lineRend.startWidth, 0, lineDecaySpeed);

    }
    void FixedUpdate()
    {
        if (cameraWarping)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, orignalFov + fovWarpAmount , fovWarpSpeed * Time.deltaTime);
        }
    }

    IEnumerator TeleportAbility(Vector3 pos)
    {
        cameraWarping = true;
        StartCoroutine(SlowMo());
        yield return new WaitForSecondsRealtime(teleportDelay);
        cameraWarping = false;
        cam.fieldOfView = orignalFov;
        GameManager.instance.playerController.Teleport(pos);
        
    }
    IEnumerator SlowMo()
    {
        GameManager.instance.StartSlowMotion(.1f);
        yield return new WaitForSecondsRealtime(sloMoDuration);
        GameManager.instance.StopSlowMotion();
        Destroy(gameObject);
    }



}
