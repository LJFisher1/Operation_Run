using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int explosionStrength;
    [SerializeField] bool pull;
    [SerializeField] Renderer model;
    [SerializeField] Collider col;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        model.enabled = false;
        col.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.instance.playerController.TakeDamage(damage);
            if (pull) // Pulls inwards
            {
                GameManager.instance.playerController.windPushback((transform.position - GameManager.instance.playerController.transform.position).normalized * explosionStrength);
            }
            else // Pushes outward
            {
                GameManager.instance.playerController.windPushback((transform.position + GameManager.instance.playerController.transform.position).normalized * explosionStrength);
            }
        }
    }
}
