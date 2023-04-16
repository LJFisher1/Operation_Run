using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallDissappearingFloor : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] MeshCollider meshCollider;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] new ParticleSystem particleSystem;
    [SerializeField] AudioSource audioSource;
    [Range(0, 1)][SerializeField] float volume;
    [SerializeField] AudioClip[] audioClip;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(Dissappear());
        }
    }

    IEnumerator Dissappear()
    {
        boxCollider.enabled = false;
        audioSource.PlayOneShot(audioClip[0], volume);
        particleSystem.Play();
        yield return new WaitForSeconds(1f);
        meshRenderer.enabled = false;
        yield return new WaitForSeconds(.1f);
        meshRenderer.enabled = true;
        yield return new WaitForSeconds(.1f);
        meshRenderer.enabled = false;
        yield return new WaitForSeconds(.1f);
        meshRenderer.enabled = true;
        yield return new WaitForSeconds(.1f);
        meshCollider.enabled = false;
        meshRenderer.enabled = false;
        particleSystem.Stop();
        Destroy(gameObject);
    }
}
