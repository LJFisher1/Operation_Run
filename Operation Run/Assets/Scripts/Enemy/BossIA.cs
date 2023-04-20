using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossIA : MonoBehaviour, IDamage
{
    [Header("--Components--")]
    [SerializeField] Renderer model;
    public NavMeshAgent agent;
    public Animator animator;
    [SerializeField] GameObject deathEffect;

    [Header("--Boss Stats--")]
    [SerializeField] Transform headPosition;
    public int HP; //making this public for now to test teleport ability.
    [SerializeField] int kiteDistance;
    [SerializeField] int varStopLocation;
    [SerializeField] int sightAngle;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int waitTime;
    [SerializeField] bool hasRoute;
    [SerializeField] GameObject[] routePositions;
    [SerializeField] int posItter;

    [Header("--Attack Stats--")]
    public float attackRate;
    public int attackDistance;
    public int attackDamage;
    public GameObject projectile;
    public GameObject sniper;
    public GameObject shotgun;
    public GameObject ram;
    public GameObject sniperw;
    public GameObject shottyw;
    public GameObject ramw;
    public int projectileSpeed;
    public Transform projectilePosition;
    public Transform spLaser;
    public Transform spShotgun;
    public Transform spRam;
    [SerializeField] bool hasExternalFiringAnimation;

    bool isAttacking;
    public bool playerInRange;
    bool destinationChosen;
    float angleToPlayer;
    float stoppingDistanceOrigin;
    public Vector3 playerDirection;
    Vector3 startingPosition;
    Vector3 faceDirection;
    public bool IsAlive { get => (HP > 0); }
    bool fighting = true;
    bool hasSpawnedLaser = false;
    bool hasSpawnedBlast = false;
    bool hasSpawnedRam = false;
    bool HasSpawnedGhost = false;
    public GameObject middle;
    public GameObject spawner1;
    public GameObject spawner2;
    public GameObject spawner3;
    public GameObject ramEnemy1;
    public GameObject ramEnemy2;
    public GameObject laserEnemy1;
    public GameObject laserEnemy2;
    public GameObject blastEnemy1;
    public GameObject blastEnemy2;
    public GameObject ghostEnemy1;
    public GameObject ghostEnemy2;
    public GameObject ghostEnemy3;
    public GameObject ghostEnemy4;
    public GameObject ghostEnemy5;
    public GameObject key1;
    public GameObject sheildEffect;
    public GameObject sheildEffectFinal;

    // Start is called before the first frame update
    void Start()
    {
        agent.enabled = true;
        animator = GetComponent<Animator>();
        stoppingDistanceOrigin = agent.stoppingDistance;
        startingPosition = transform.position;
        projectile = sniper;
        sniperw.SetActive(true);
        projectilePosition = spLaser;
        attackRate = 8;
        attackDamage = 8;
    }

    // Update is called once per frame
    void Update()
    {
        if(HP <= 20)
        {
            fighting = false;
        }
        if (agent.isActiveAndEnabled)
        {
            animator.SetFloat("Speed", agent.velocity.normalized.magnitude);

            if (playerInRange)
            {
                if (!hasSpawnedRam)
                {
                    SpawnRam();
                    hasSpawnedRam = true;
                }
                if (!hasSpawnedLaser && HP <=100)
                {
                    SpawnLaser();
                    hasSpawnedLaser = true;
                }
                if(!hasSpawnedBlast && HP <= 60)
                {
                    SpawnBlast();
                    hasSpawnedBlast = true;
                }
                if(!HasSpawnedGhost && HP <= 20)
                {
                    SpawnGhost();
                    HasSpawnedGhost = true;
                }
                if (fighting == true)
                {
                    if (!CanSeePlayer() && Vector3.Distance(GameManager.instance.player.transform.position, startingPosition) > kiteDistance)
                    {
                        return;
                    }
                }
            }
            else
            {
                return;
            }
        }
    }

    bool CanSeePlayer()
    {
        playerDirection = (GameManager.instance.player.transform.position - projectilePosition.position).normalized;
        //Debug.DrawRay(headPosition.position, playerDirection*200);
        //Debug.DrawLine(headPosition.position, GameManager.instance.player.transform.position, Color.blue);
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(projectilePosition.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightAngle)
            {
                agent.stoppingDistance = stoppingDistanceOrigin;
                agent.SetDestination(GameManager.instance.player.transform.position + (Random.insideUnitSphere * varStopLocation));
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    FacePlayer();
                }
                if (!isAttacking)
                {
                    StartCoroutine(Attack());
                }
                return true;
            }
        }
        return false;
    }

    void FacePlayer()
    {
        faceDirection = (new Vector3(playerDirection.x, 0, playerDirection.z));
        Quaternion rot = Quaternion.LookRotation(faceDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        if (!hasExternalFiringAnimation) animator.SetTrigger("Shoot");
        CreateBullet();
        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }

    public void CreateBullet()
    {
        GameObject attackClone = Instantiate(projectile, projectilePosition.position, projectile.transform.rotation);
        attackClone.GetComponent<IBossAttack1>().Initialize(this);
    }

    public void TakeDamage(int dmg)
    {
        animator.SetTrigger("TakeDamage");
        if(HP > 100 && GameManager.instance.playerController.weapon.name == "Charging Ram")
        {
            HP -= dmg;
            StartCoroutine(FlashMat());
            if (HP <= 100 && HP > 60)
            {
                if (projectile != shotgun)
                {
                    ChangeToShotgun();
                }
            }

        }
        else if(HP <= 100 && HP > 60 && GameManager.instance.playerController.weapon.name == "Laser Swapper")
        {
           
            HP -= dmg;
            StartCoroutine(FlashMat());
            if (HP <= 60 && HP > 0)
            {
                if (projectile != ram)
                {
                    ChangeToRam();
                }
            }
        }
        else if(HP <= 60 && HP > 20 && GameManager.instance.playerController.weapon.name == "Blasting Staff")
        {
            HP -= dmg;
            StartCoroutine(FlashMat());
        }
        else if(HP <= 20 && GameManager.instance.playerController.weapon.name != "Ghost Staff")
        {
            StartCoroutine(FlashSheild());
            if (agent.enabled)
            {
                StartCoroutine(TakeAKnee());
            }
        }
        else if(HP <= 20 && GameManager.instance.playerController.weapon.name == "Ghost Staff")
        {
            HP -= dmg;
            StartCoroutine(FlashMat());
            if (agent.enabled) agent.SetDestination(GameManager.instance.player.transform.position);
            if (!IsAlive)
            {
               
                if (HP <= 0)
                {
                    StopAllCoroutines();
                    ++GameManager.instance.enemysDefeated;
                    Instantiate(deathEffect, transform.position, transform.rotation);
                    Instantiate(key1, position: middle.transform.position, middle.transform.rotation);
                    Destroy(gameObject.transform.parent.gameObject);
                }
            }
        }
        else
        {
            StartCoroutine(FlashSheild());
        }
    }

    IEnumerator FlashMat()
    {
        model.GetComponentInChildren<Renderer>().material.shader = Shader.Find("Standard");
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        model.material.color = Color.white;
        model.GetComponentInChildren<Renderer>().material.shader = Shader.Find("PolyArtMaskTint");
    }

    void ChangeToShotgun()
    {
        projectile = shotgun;
        sniperw.SetActive(false);
        shottyw.SetActive(true);
        projectilePosition = spShotgun;
        attackDamage = 3;
        attackRate = 3;
        projectileSpeed = 25;
    }

    void ChangeToRam()
    {
        projectile = ram;
        shottyw.SetActive(false);
        ramw.SetActive(true);
        projectilePosition = spRam;
        attackDamage = 6;
        attackRate = 5;
        projectileSpeed = 100;
    }

    IEnumerator TakeAKnee()
    {
        if (agent.enabled) agent.SetDestination(middle.transform.position);
        yield return new WaitForSeconds(4);
        animator.SetFloat("Speed", 0);
        animator.SetTrigger("Beat");
        agent.enabled = false;
        sheildEffectFinal.SetActive(true);
    }

    GameObject curEnemy1;
    GameObject curEnemy2;
    void SpawnRam()
    {
        curEnemy1 = Instantiate(ramEnemy1, position: spawner1.transform.position, spawner1.transform.rotation);
        curEnemy2 = Instantiate(ramEnemy2, position: spawner2.transform.position, spawner2.transform.rotation);

    }

    void SpawnLaser()
    {
        ClearWave();
        curEnemy1 = Instantiate(laserEnemy1, position: spawner1.transform.position, spawner1.transform.rotation);
        curEnemy2 = Instantiate(laserEnemy2, position: spawner2.transform.position, spawner2.transform.rotation);
    }

    void SpawnBlast()
    {
        ClearWave();
        curEnemy1 = Instantiate(blastEnemy1, position: spawner1.transform.position, spawner1.transform.rotation);
        curEnemy2 = Instantiate(blastEnemy2, position: spawner2.transform.position, spawner2.transform.rotation);
    }

    void SpawnGhost()
    {
        ClearWave();
        Instantiate(ghostEnemy1, position: spawner1.transform.position, spawner1.transform.rotation);
        Instantiate(ghostEnemy2, position: spawner1.transform.position, spawner1.transform.rotation);
        Instantiate(ghostEnemy3, position: spawner2.transform.position, spawner2.transform.rotation);
        Instantiate(ghostEnemy4, position: spawner2.transform.position, spawner2.transform.rotation);
        Instantiate(ghostEnemy5, position: spawner3.transform.position, spawner3.transform.rotation);

    }

    IEnumerator FlashSheild()
    {
        sheildEffect.SetActive(true);
        yield return new WaitForSeconds(.3f);
        sheildEffect.SetActive(false);
    }

    void ClearWave()
    {
        if (curEnemy1 != null) Destroy(curEnemy1);
        if (curEnemy2 != null) Destroy(curEnemy2);
    }
}
