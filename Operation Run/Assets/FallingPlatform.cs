using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [Header("params")]
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
        yield return new WaitForSeconds(fallDelay);
        isCrumbling = false;
        isFalling = true;
        Destroy(fallingBody.gameObject, duration);
    }
    private void Update()
    {
        if (isCrumbling)
        {
            //shake
            //play audio
            //play particle
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
