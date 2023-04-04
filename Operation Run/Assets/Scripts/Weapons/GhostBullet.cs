using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBullet : MonoBehaviour, IBullet
{
    public Rigidbody rb;
    [Range(0,10)] public float seekingStrength;
    [Range(0, .5f)][SerializeField] float cosmeticOffset;
    [HideInInspector] public int damage;
    [HideInInspector] public float bulletSpeed;
    float range;
    Vector3 cosmeticOffsetVector;
    float duration;
    bool ghostActive;
    [HideInInspector] public static bool playerHasGhost;
    public void Initialize(Weapon creator)
    {
        if (playerHasGhost)
        {
            GameManager.instance.playerController.MANA += 1;
            Destroy(gameObject);
            return;
        }
        playerHasGhost = true;
        damage = creator.damage;
        bulletSpeed = creator.bulletSpeed;
        duration = creator.duration;
        range = creator.range;
        transform.localScale = Vector3.zero;
        cosmeticOffsetVector = new Vector3(Random.Range(-cosmeticOffset, cosmeticOffset), Random.Range(-cosmeticOffset, cosmeticOffset), Random.Range(-cosmeticOffset, cosmeticOffset));
    }
    IEnumerator DestroyAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        DestroyGhost();
    }
    void FireBullet()
    {
        ghostActive = true;
        rb.velocity = Camera.main.transform.forward * bulletSpeed;
        StartCoroutine(DestroyAfterDuration());
    }
    private void Update()
    {
        if (!ghostActive)
        {
            transform.position = GameManager.instance.playerController.shootPointVisual.position + cosmeticOffsetVector;
            transform.rotation = GameManager.instance.playerController.transform.rotation;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one/2, Time.deltaTime);
            if(Input.GetButtonUp("Fire1"))
            {
                FireBullet();
            }
        }
        if (playerHasGhost && GhostOutOfRange())
        {
            DestroyGhost();
        }

    }
    bool GhostOutOfRange()
    {
        float dist = (transform.position - GameManager.instance.playerController.transform.position).magnitude;
        return dist >= range;
    }
    void DestroyGhost()
    {
        playerHasGhost = false;
        Destroy(gameObject);
    }
}
