using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("--Components--")]
    [SerializeField] Renderer model;
    public NavMeshAgent agent;
    public  Animator animator;
    [SerializeField] GameObject deathEffect;
    

    [Header("--Wizard Stats--")]
    [SerializeField] Transform headPosition;
    public int HP; //making this public for now to test teleport ability.
    [SerializeField] int roamDistance;
    [SerializeField] int kiteDistance;
    [SerializeField] int varStopLocation;
    [SerializeField] int sightAngle;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int waitTime;
    [SerializeField] bool hasRoute;
    [SerializeField] GameObject[] routePositions;
    [SerializeField] int posItter;

    [Header("--Attack Stats--")]
    public bool canBreakWalls;
    public float attackRate;
    public int attackDistance;
    public int attackDamage;
    public GameObject projectile;
    public int projectileSpeed;
    public Transform projectilePosition;
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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        stoppingDistanceOrigin = agent.stoppingDistance;
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            animator.SetFloat("Speed", agent.velocity.normalized.magnitude);

            if (playerInRange)
            {
                if (!CanSeePlayer() && Vector3.Distance(GameManager.instance.player.transform.position, startingPosition) > kiteDistance)
                {
                    StartCoroutine(Roam());
                }
            }
            else
            {
                StartCoroutine(Roam());
            }
        }
    }

    IEnumerator Roam()
    {
        if (hasRoute == false)
        {
            if (!destinationChosen && agent.remainingDistance < 0.05f)
            {
                animator.SetFloat("Speed", agent.velocity.normalized.magnitude);
                destinationChosen = true;
                agent.stoppingDistance = 1;
                yield return new WaitForSeconds(waitTime);
                destinationChosen = false;

                Vector3 randDirection = Random.insideUnitSphere * roamDistance;
                randDirection += startingPosition;
                NavMeshHit hit;
                NavMesh.SamplePosition(randDirection, out hit, roamDistance, 1);

                agent.SetDestination(hit.position);
            }
        }
        else
        {
            animator.SetFloat("Speed", agent.velocity.normalized.magnitude);
            agent.stoppingDistance = 1;

            agent.SetDestination(routePositions[posItter].transform.position);
            if (!destinationChosen && agent.remainingDistance < 0.05f)
            {
                destinationChosen = true;
                yield return new WaitForSeconds(waitTime);
                if (posItter < routePositions.Length - 1)
                {
                    posItter++;
                }
                else
                {
                    posItter = 0;
                }
                destinationChosen = false;
            }
        }
    }

    bool CanSeePlayer()
    {
        playerDirection = (GameManager.instance.player.transform.position - projectilePosition.position).normalized;
        Debug.DrawRay(headPosition.position, playerDirection*200);
        Debug.DrawLine(headPosition.position, GameManager.instance.player.transform.position, Color.blue);
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

    IEnumerator FlashMat()
    {
        model.GetComponentInChildren<Renderer>().material.shader = Shader.Find("Standard");
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        model.material.color = Color.white;
        model.GetComponentInChildren<Renderer>().material.shader = Shader.Find("PolyArtMaskTint");
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        if(!hasExternalFiringAnimation) animator.SetTrigger("Shoot");
        CreateBullet();
        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }

    public void CreateBullet()
    {
        GameObject attackClone = Instantiate(projectile, projectilePosition.position, projectile.transform.rotation);
        attackClone.GetComponent<IEnemyAttack>().Initialize(this);
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(FlashMat());
        animator.SetTrigger("TakeDamage");
        if(agent.enabled) agent.SetDestination(GameManager.instance.player.transform.position);
        if (!IsAlive)
        {
            StopAllCoroutines();
            HP = 0;
            if (HP == 0)
            {
                ++GameManager.instance.enemysDefeated;
                Instantiate(deathEffect, transform.position, transform.rotation);
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
    }
}
