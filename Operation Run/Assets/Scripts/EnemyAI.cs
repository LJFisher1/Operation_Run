using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("--Components--")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("--Wizard Stats--")]
    [SerializeField] Transform headPosition;
    [SerializeField] int HP;
    [SerializeField] int roamDistance;
    [SerializeField] int sightAngle;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int waitTime;

    [Header("--Attack Stats--")]
    [SerializeField] float attackRate;
    [SerializeField] int attackDistance;
    [SerializeField] GameObject projectile;
    [SerializeField] int projectileSpeed;
    [SerializeField] Transform projectilePosition;

    bool isAttacking;
    bool playerInRange;
    bool destinationChosen;
    float angleToPlayer;
    float stoppingDistanceOrigin;
    Vector3 playerDirection;
    Vector3 startingPosition;


    // Start is called before the first frame update
    void Start()
    {
        stoppingDistanceOrigin = agent.stoppingDistance;
        startingPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
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

    IEnumerator Roam()
    {
        if (!destinationChosen && agent.remainingDistance < 0.05f)
        {
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
        playerDirection = (GameManager.instance.player.transform.position - headPosition.position);
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }

    void FacePlayer()
    {
        playerDirection.y = 0;
        Quaternion rot = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }


    IEnumerator FlashMat()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        GameObject bulletClone = Instantiate(projectile, projectilePosition.position, projectile.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;
        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }
}
