using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("--Components--")]
    [SerializeField] Renderer model;
    public NavMeshAgent agent;
    [SerializeField] Animator animator;

    [Header("--Wizard Stats--")]
    [SerializeField] Transform headPosition;
    public int HP; //making this public for now to test teleport ability.
    [SerializeField] int roamDistance;
    [SerializeField] int sightAngle;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int waitTime;

    [Header("--Attack Stats--")]
    [SerializeField] float attackRate;
    [SerializeField] int attackDistance;
    [SerializeField] int attackDamage;
    [SerializeField] GameObject projectile;
    [SerializeField] int projectileSpeed;
    [SerializeField] Transform projectilePosition;

    bool isAttacking;
    public bool playerInRange;
    bool destinationChosen;
    float angleToPlayer;
    float stoppingDistanceOrigin;
    Vector3 playerDirection;
    Vector3 startingPosition;
    Vector3 faceDirection;


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
                if (!CanSeePlayer())
                {
                    StartCoroutine(Roam());
                }
            }
            else if (agent.destination != GameManager.instance.player.transform.position)
            {
                StartCoroutine(Roam());
            }
        }
    }

    IEnumerator Roam()
    {
        if (!destinationChosen && agent.remainingDistance < 0.05f)
        {
            animator.SetFloat("Speed", agent.velocity.normalized.magnitude);
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(waitTime);
            destinationChosen = false;

            Vector3 randDirection = Random.insideUnitSphere * roamDistance;
            randDirection += startingPosition;
            NavMeshHit hit;
            NavMesh.SamplePosition(randDirection, out hit, roamDistance, 1);

            agent.SetDestination(hit.position);
        }
    }

    bool CanSeePlayer()
    {
        playerDirection = (GameManager.instance.player.transform.position - headPosition.position).normalized;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(headPosition.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightAngle)
            {
                agent.stoppingDistance = stoppingDistanceOrigin;
                agent.SetDestination(GameManager.instance.player.transform.position);
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
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
        model.GetComponentInChildren<Renderer>().material.shader = Shader.Find("PolyArtMaskTint");
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Shoot");
        createBullet();
        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }

    public void createBullet()
    {
        GameObject attackClone = Instantiate(projectile, projectilePosition.position, projectile.transform.rotation);
        attackClone.GetComponent<Rigidbody>().velocity = playerDirection * projectileSpeed;
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(FlashMat());
        animator.SetTrigger("TakeDamage");
        if(agent.enabled) agent.SetDestination(GameManager.instance.player.transform.position);
        if (HP <= 0)
        {
            HP = 0;
            if (HP == 0)
            {
                GameManager.instance.UpdateScore(10);
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
    }
}
