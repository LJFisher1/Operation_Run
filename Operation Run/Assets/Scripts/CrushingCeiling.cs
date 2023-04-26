using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.ProBuilder;

public class CrushingCeiling : MonoBehaviour
{
    [Header("* Controls")]
    [SerializeField] bool active;// true = up, false = down
    [Header("* Settings")]
    [Range(0.1f,5)][SerializeField] float speedDown;
    [Range(0.1f, 5)][SerializeField] float speedUp;
    [Range(0.1f, 3)][SerializeField] float resetTime;
    [Range(0.1f, 100)][SerializeField] int damage;
    [Range(0, 1)][SerializeField] float damageRate;
    [Range(0, 1)][SerializeField] float volume;
    [Header("* Components")]
    [SerializeField] BoxCollider ceiling;
    [SerializeField] BoxCollider floor;
    [SerializeField] Transform down;
    [SerializeField] AudioClip[] audioClips;
    private AudioSource audioSource;
    private Vector3 up;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        active = false;
        ceiling.enabled = false;
        floor.enabled = true;
        up = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(active)
            {
                if(ceiling.enabled)
                StartCoroutine(DamagePlayer(other.GetComponent<IDamage>()));
            }
            else
            {
                DropCeiling();
            }
        }
    }

    void DropCeiling()
    {
        audioSource.pitch = Random.Range(1f, 0.5f);
        audioSource.PlayOneShot(audioClips[0]);
        active = true;
        ceiling.enabled = true;
        floor.enabled = false;
    }

    IEnumerator RaiseCeiling()
    {
        yield return new WaitForSeconds(resetTime);
        ceiling.enabled = false;
        floor.enabled = true;
    }

    IEnumerator DamagePlayer(IDamage other)
    {
        audioSource.pitch = Random.Range(1f, 0.5f);
        audioSource.PlayOneShot(audioClips[1]);
        ceiling.enabled = false;
        other.TakeDamage(damage);
        yield return new WaitForSeconds(damageRate);
        ceiling.enabled = true;
    }

    private void Update()
    {
        if (active)
        {
            if(floor.enabled)
            {
                transform.position = Vector3.Lerp(transform.position, up, Time.deltaTime * speedUp);
                checkProgress(transform.position, up);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, down.position, Time.deltaTime * speedDown);
                checkProgress(transform.position, down.position);
            }
        }
    }

    void checkProgress(Vector3 current, Vector3 destination)
    {
        if ((current - destination).magnitude < .1)
        {
            if(floor.enabled)
            {
                active = false;
            }
            else
            {
                StartCoroutine(RaiseCeiling());
            }
        }
    }
}
