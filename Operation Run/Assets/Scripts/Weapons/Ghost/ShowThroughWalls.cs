using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowThroughWalls : MonoBehaviour
{
    [SerializeField] Material[] wallMaterials;
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
            foreach (Material m in wallMaterials) m.SetFloat(SizeID, viewSize);
        }
        else
        {
            foreach (Material m in wallMaterials) m.SetFloat(SizeID, 0);
        }

        foreach (Material m in wallMaterials) m.SetVector(PosID, view);

    }
    private void OnDestroy()
    {
        foreach (Material m in wallMaterials)
        {
            m.SetFloat(SizeID, 0);
            m.SetVector(PosID, Vector4.zero);
        }
    }
}
