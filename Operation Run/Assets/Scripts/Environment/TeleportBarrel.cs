using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBarrel : MonoBehaviour, IDamage
{
    [Header("----- Required Componets -----")]
    [SerializeField] Collider colider;
    [Header("----- Options -----")]
    [SerializeField] bool destroyOnUse;
    [SerializeField] Transform respawnLocation; //for when the barrel may fall out of bounds;
    [Header("----- Visual Effect -----")]
    [SerializeField] float scaleLerpRate = 50;
    [Range(0,.5f)][SerializeField] float rotLerpRate = .1f;
    [SerializeField] float angleMin;
    [SerializeField] float angleMax;
    float targetAngle;
    bool isTeleporting;
    Vector3 posOG;
    
    private void Start()
    {
        posOG = transform.position;
    }
    public void TakeDamage(int dmg)
    {
        if(GameManager.instance.playerController.weapon.canTeleport)
        {
            StartCoroutine(SwapPlaces(GameManager.instance.player.transform.position, GameManager.instance.playerController.weapon.bulletSpeed + 0.1f));
        }
    }
    IEnumerator SwapPlaces(Vector3 newPos, float delay)
    {
        colider.enabled = false;
        Vector3 scaleOG = transform.localScale;
        Quaternion rotOG = transform.rotation;
        targetAngle = Random.Range(angleMin, angleMax);

        isTeleporting = true;
        yield return new WaitForSeconds(delay);
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

    public void ReturnFromOutOfBounds()
    {
        Vector3 pos = posOG;
        if (respawnLocation != null) pos = respawnLocation.position;
        StartCoroutine(SwapPlaces(pos,1));
    }
}
