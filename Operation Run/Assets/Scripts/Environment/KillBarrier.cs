using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBarrier : MonoBehaviour
{
    [SerializeField] float damageDelay;
    [SerializeField] int damage = int.MaxValue;
    [SerializeField] bool hideOnPlay;
    [SerializeField] bool isOutOfBounds;
    //[SerializeField] bool hurtEnemies;
    bool isKilling;
    private void Start()
    {
        if (hideOnPlay)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
       if(!isKilling && other.transform.CompareTag("Player")) StartCoroutine(Kill(other));
    }
    IEnumerator Kill(Collider other)
    {
        if(!GameManager.instance.tutorialManager.CheckCompleted("Kill Barrier"))
        {
            StartCoroutine(GameManager.instance.FlashTutorialPopup("Kill Barrier"));
            GameManager.instance.tutorialManager.SetTutorialCompletion("Kill Barrier", true);
        }
        isKilling = true;
        IDamage damAble = other.GetComponent<IDamage>();
        yield return new WaitForSeconds(damageDelay);
        if (damAble != null)
        {
            if(damAble.IsAlive) damAble.TakeDamage(damage);
        }
        isKilling = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isOutOfBounds)
        {
            TeleportBarrel tpb = other.GetComponent<TeleportBarrel>();
            if(tpb != null) tpb.ReturnFromOutOfBounds();
        }
    }
}
