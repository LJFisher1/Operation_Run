using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowThroughWalls : MonoBehaviour
{
    [SerializeField] Material wallMaterial;
    [SerializeField] float viewSize;
    [SerializeField] LayerMask mask;
    [SerializeField] float shrinkAlpha;

    static Camera cam;
    static int PosID = Shader.PropertyToID("_Position");
    static int SizeID = Shader.PropertyToID("_Size");
    void Start()
    {
        if(cam == null)
        {
            cam = Camera.main;
        }
        
    }
    private void Update()
    {
        Vector3 view  = cam.WorldToViewportPoint(transform.position);
        Vector3 dir = (cam.transform.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, dir);

        if (Physics.Raycast(ray, 3000, mask))
        {
            wallMaterial.SetFloat(SizeID, viewSize);
        }
        else
        {
            wallMaterial.SetFloat(SizeID, 0);
        }

        wallMaterial.SetVector(PosID, view);

    }
    private void OnDestroy()
    {
        wallMaterial.SetFloat(SizeID, 0);
    }
}
