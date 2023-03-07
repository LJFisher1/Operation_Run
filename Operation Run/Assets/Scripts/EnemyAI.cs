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

    [Header("-----Gun Stats-----")]
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
        StartCoroutine(Roam());
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
}
