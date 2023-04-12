using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour, IDamage
{
    [Header("--Components--")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] GameObject deathEffect;
    [SerializeField] Rigidbody rb;
    [SerializeField] Weapon weaponDrop;

    [Header("--Ghost Stats--")]
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
    [SerializeField] bool kamikaze;
    [SerializeField] float attackDelay;
    [SerializeField] float seekingSpeed;
    [SerializeField] float speedGrowthRate;
    [SerializeField] float seekingStrength;
    [SerializeField] int attackDamage;

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
        stoppingDistanceOrigin = agent.stoppingDistance;
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            //animator.SetFloat("Speed", agent.velocity.normalized.magnitude);

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
        if (isAttacking)
        {
            Debug.Log("Attack charge");
            playerDirection = GetPlayerDirection();
            FacePlayer();
            rb.velocity = Vector3.Lerp(rb.velocity, playerDirection * seekingSpeed, seekingStrength * Time.deltaTime);
            seekingSpeed += speedGrowthRate * Time.deltaTime;
            seekingStrength = Mathf.Lerp(seekingStrength, seekingStrength + 1, 5 * Time.deltaTime);
        }
    }

    Vector3 GetPlayerDirection()
    {
        return (GameManager.instance.player.transform.position - headPosition.position).normalized;
    }
    IEnumerator Roam()
    {
        if (hasRoute == false)
        {
            if (!destinationChosen && agent.remainingDistance < 0.05f)
            {
                //animator.SetFloat("Speed", agent.velocity.normalized.magnitude);
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
            //animator.SetFloat("Speed", agent.velocity.normalized.magnitude);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }
    bool CanSeePlayer()
    {
        Debug.Log("checking sight");
        playerDirection = GetPlayerDirection();
        //Debug.DrawRay(headPosition.position, playerDirection*200);
        //Debug.DrawLine(headPosition.position, GameManager.instance.player.transform.position, Color.blue);
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(headPosition.position, playerDirection, out hit))
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
        //if(isAttacking) rb.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    IEnumerator FlashMat()
    {
        Color oColor = model.material.color;
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        model.material.color = oColor;
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackDelay);
        yield return new WaitForEndOfFrame();
        Debug.Log("Attacking");
        isAttacking = true;
        agent.enabled = false;
        rb.isKinematic = false;
        //StopAllCoroutines();

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!kamikaze) return;
        if (collision.transform.CompareTag("Player"))
        {
            IDamage damAble = GameManager.instance.player.GetComponent<IDamage>();
            if(damAble != null)
            {
                damAble.TakeDamage(attackDamage);
                Death();
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (kamikaze) return;
        if (collision.transform.CompareTag("Player"))
        {
            IDamage damAble = GameManager.instance.player.GetComponent<IDamage>();
            if (damAble != null)
            {
                //play damage effect?
                damAble.TakeDamage(attackDamage);
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(FlashMat());
        //animator.SetTrigger("TakeDamage");
        StartCoroutine(Attack());
        if (!IsAlive)
        {
            StopAllCoroutines();
            HP = 0;
            if (HP == 0)
            {
                ++GameManager.instance.enemysDefeated;
                if (weaponDrop != null && GameManager.instance.player != null) GameManager.instance.playerController.ChangeWeapon(weaponDrop);
                Death();
            }
        }
    }

    void Death()
    {
        Instantiate(deathEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
