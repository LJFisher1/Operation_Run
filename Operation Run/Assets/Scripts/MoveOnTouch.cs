using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnTouch : MonoBehaviour
{
    [SerializeField] Transform movePoint;
    Vector3 movePos;
    [SerializeField] float moveSpeed;
    bool isMoving;
    private void Start()
    {
        movePos = movePoint.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isMoving = true;
    }
    private void Update()
    {
        if (!isMoving) return;
        if((transform.position - movePos).magnitude > .1)
        {
            transform.position = Vector3.Lerp(transform.position, movePos, moveSpeed * Time.deltaTime);
        }
        else
        {
            this.enabled = false;
        }
    }
}
