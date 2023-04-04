using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBarrel : MonoBehaviour, IDamage
{
    [Header("----- Required Componets -----")]
    [SerializeField] Collider colider;
    [Header("----- Options -----")]
    [SerializeField] bool destroyOnUse;
    [Header("----- Visual Effect -----")]
    [SerializeField] float scaleLerpRate = 50;
    [Range(0,.5f)][SerializeField] float rotLerpRate = .1f;
    [SerializeField] float angleMin;
    [SerializeField] float angleMax;
    float targetAngle;
    bool isTeleporting;
    public void TakeDamage(int dmg)
    {
        if(GameManager.instance.playerController.weapon.canTeleport)
        {
            StartCoroutine(SwapPlaces());
        }
    }
    IEnumerator SwapPlaces()
    {
        colider.enabled = false;
        Vector3 scaleOG = transform.localScale;
        Quaternion rotOG = transform.rotation;
        targetAngle = Random.Range(angleMin, angleMax);
        Vector3 newPos = GameManager.instance.player.transform.position;

        isTeleporting = true;
        yield return new WaitForSeconds(GameManager.instance.playerController.weapon.bulletSpeed+0.1f);
        if (destroyOnUse)  Destroy(gameObject); 
        isTeleporting = false;
        transform.rotation = rotOG;
        transform.position = newPos;
        transform.localScale = scaleOG;
        colider.enabled = true;
        
    }
    private void Update()
    {
        if (isTeleporting)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, scaleLerpRate * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, new(targetAngle,targetAngle,targetAngle,0), rotLerpRate * Time.deltaTime);
        }
    }
}
