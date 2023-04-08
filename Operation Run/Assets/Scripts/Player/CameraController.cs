using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float sensX, sensY;
    [SerializeField] float lockMin, lockMax;
    PlayerController player;
    float xRotation;
    [SerializeField] bool invertY;

    public void Start()
    {
        sensX = 200;
        sensY = 200;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player = GameManager.instance.playerController;
    }

    public void Update()
    {
        if (player.IsAlive)
        {
            float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;

            if (!invertY) mouseY *= -1;

            xRotation += mouseY;
            xRotation = Mathf.Clamp(xRotation, lockMin, lockMax);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0); // rotate camera
            transform.parent.Rotate(Vector3.up * mouseX); // rotate player
        }
    }

    public void UpdateSensitivity(float value)
    {
        sensX = value * 1000 + 200;
        sensY = value * 1000 + 200;
    }
}
