using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitscan : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage;
    public string hitTag = "Player";
    public float duration;
    public int range;
    public LineRenderer lineRend;

    private void Start()
    {
        Debug.Log("Hitscan created");
        RaycastHit hit;
        Ray ray = new(GameManager.instance.playerController.shootPointCenter.position, Camera.main.transform.forward);
        lineRend.enabled = false;
        if (Physics.Raycast(ray, out hit, range))
        {
            lineRend.enabled = true;
            lineRend.SetPosition(0, GameManager.instance.playerController.shootPointVisual.position);
            lineRend.SetPosition(1, hit.point);
            var target = hit.collider.GetComponent<IDamage>();
            Debug.Log(hit.transform.name);
            if (target != null && hit.collider.CompareTag(hitTag)) 
            {
                target.TakeDamage(damage);
            }
        }
        Destroy(gameObject, duration);
    }

    
    

}
