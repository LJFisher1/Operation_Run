using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] Renderer model;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.instance.playerSpawnPosition.transform.position = transform.position;
            StartCoroutine(Flash());
            Destroy(GetComponent<BoxCollider>());
        }
    }

    IEnumerator Flash()
    {
        model.material.color = Color.red;
        GameManager.instance.checkpointPopup.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        model.material.color = Color.white;
        GameManager.instance.checkpointPopup.SetActive(false);
    }
}
