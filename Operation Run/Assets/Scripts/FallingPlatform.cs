using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [Header("FX")]
    [SerializeField] AudioClip[] crumbleSFX;
    [SerializeField] AudioSource aud;
    [SerializeField] ParticleSystem particleFX;
    [Header("Params")]
    [SerializeField] float fallDelay;
    [SerializeField] float fallSpeed;
    [Header("Options")]
    [SerializeField] bool useParent;
    [SerializeField] Transform movePoint;
    [SerializeField] float duration;
    

    Transform fallingBody;
    bool isFalling;
    bool hasFallen;
    bool isCrumbling;
    // Start is called before the first frame update
    void Start()
    {
        if (useParent)
        {
            fallingBody = gameObject.transform.parent;
        }
        else
        {
            fallingBody = transform;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasFallen && other.CompareTag("Player"))
        {
            StartCoroutine(Crumble());
        }
    }

    IEnumerator Crumble()
    {
        isCrumbling = true;
        aud.PlayOneShot(crumbleSFX[Random.Range(0, crumbleSFX.Length)]);
        yield return new WaitForSeconds(fallDelay);
        isCrumbling = false;
        isFalling = true;
        particleFX.Play();
        if(duration > 0) Destroy(fallingBody.gameObject, duration);
    }
    private void Update()
    {
        if (isCrumbling)
        {
            //shake
            fallingBody.position = Vector3.Lerp(fallingBody.position, new(fallingBody.position.x + Random.Range(-2, 2), fallingBody.position.y, fallingBody.position.z + Random.Range(-2, 2)), fallSpeed * 2 * Time.deltaTime);
        }
        if (isFalling)
        {
            if (movePoint != null)
            {
                if ((fallingBody.position - movePoint.position).magnitude < .1)
                {
                    isFalling = false;
                    hasFallen = true;
                }
                else
                {
                    fallingBody.position = Vector3.Lerp(fallingBody.position, movePoint.position, fallSpeed * Time.deltaTime);
                }
            }
            else
            {
                fallingBody.position = Vector3.Lerp(fallingBody.position, fallingBody.position + Vector3.down, fallSpeed * Time.deltaTime);
            }
        }
        
    }
}
