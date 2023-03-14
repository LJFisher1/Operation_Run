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
    [SerializeField] float decaySpeed;

    private void Start()
    {
        RaycastHit hit;
        Ray ray = new(GameManager.instance.playerController.shootPointCenter.position, Camera.main.transform.forward);
        lineRend.SetPosition(0, GameManager.instance.playerController.shootPointVisual.position);
        lineRend.SetPosition(1,  Camera.main.transform.forward * range);
        if (Physics.Raycast(ray, out hit, range))
        {
            var target = hit.collider.GetComponent<IDamage>();
            Debug.Log(hit.transform.name);
            if (target != null && hit.collider.CompareTag(hitTag)) 
            {
                target.TakeDamage(damage);
            }
        }
        Destroy(gameObject, duration);
        
    }
    private void Update()
    {
        lineRend.endColor = Color.Lerp(lineRend.endColor, Color.clear, decaySpeed);
        lineRend.startColor = Color.Lerp(lineRend.startColor, Color.clear, decaySpeed);
    }




}
