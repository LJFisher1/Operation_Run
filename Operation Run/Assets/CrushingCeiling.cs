using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class CrushingCeiling : MonoBehaviour
{
    [SerializeField] bool position;// true = up, false = down
    [SerializeField] float speedDown;
    [SerializeField] float speedUp;
    [SerializeField] float resetTime;
    [SerializeField] int damage;
    [Header("* Components")]
    [SerializeField] BoxCollider trigger;
    [SerializeField] GameObject floor;
    [SerializeField] Transform down;
    private Vector3 up;

    private void Awake()
    {
        up = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(DamagePlayer(other.GetComponent<IDamage>()));
        }
    }

    IEnumerator DamagePlayer(IDamage other)
    {
        trigger.enabled = false;
        other.TakeDamage(damage);
        yield return new WaitForSeconds(0f);
        trigger.enabled = true;
    }

    public IEnumerator DropCeiling()
    {
        yield return new WaitForSeconds(resetTime);
        floor.GetComponent<isBoxTouchingPlayer>().touchingPlayer = false;
    }

    private void Update()
    {
        position = floor.GetComponent<isBoxTouchingPlayer>().touchingPlayer;
        if(position)
        {
            transform.position = Vector3.Lerp(transform.position, down.position, Time.deltaTime * speedDown);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, up, Time.deltaTime * speedUp);
        }
    }
}
