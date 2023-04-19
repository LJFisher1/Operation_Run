using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobUpAndDown : MonoBehaviour
{
    // Start is called before the first frame update
    Transform body;
    [SerializeField] float yOff;
    [SerializeField] bool useParent;
    [SerializeField] float bobSpeed;
    [SerializeField] float randomOffset;
    [SerializeField] float randomSpeed;
    float rOff;
    float rBob;
    bool movingUp;
    Vector3 posOg;
    void Start()
    {
        body = transform;
        if (useParent) body = transform.parent;
        posOg = body.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (movingUp)
        {
            body.position = Vector3.Lerp(body.position, Vector3.up * yOff + posOg, (bobSpeed + rBob) * (bobSpeed + rBob) / (body.position - (Vector3.up * (yOff + rOff) + posOg)).magnitude * Time.deltaTime);
            if((body.position - (Vector3.up * (yOff + rOff) + posOg)).magnitude < .1)
            {
                movingUp = false;
                rOff = Random.Range(-randomOffset, randomOffset);
                rBob = Random.Range(-randomOffset, randomOffset);
            }
        }
        else
        {
            body.position = Vector3.Lerp(body.position, Vector3.down * yOff + posOg, (bobSpeed + rBob) * (bobSpeed + rBob) / (body.position - (Vector3.down * (yOff + rOff) + posOg)).magnitude * Time.deltaTime);
            if ((body.position - (Vector3.down * (yOff + rOff) + posOg)).magnitude < .1)
            {
                movingUp = true;
                rOff = Random.Range(-randomOffset, randomOffset);
                rBob = Random.Range(-randomOffset, randomOffset);
            }
        }

        
    }
}
