using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy state", order = 0)]
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool isDistracted = false;
    public enum State{
        idle,
        roaming,
        attacking,
        distracted,
        patrolling
    }

    public State state;

    [Header("AI settings", order = 1)]
    [SerializeField][Range(1, 10)] private float agentBaseMoveSpeed = 2;
    [SerializeField][Range(1, 10)] private float agentAttackMoveSpeed = 3;
    [SerializeField][Range(1, 100)] private float roamRange;
    [SerializeField][Range(1, 30)] private float roamRateSeconds;
    [SerializeField][Range(1, 30)] private float distractTime = 10;
    [SerializeField] private bool patrolMode = false;

    [Header("Target objects", order = 2)]
    [SerializeField][Tooltip("Object of the player's MODEL only")] private GameObject playerModelRef;
    [SerializeField][Tooltip("Array of the distraction objects")] private List<GameObject> distractionObjects = new List<GameObject>();
    [SerializeField][Tooltip("Array of the points of a patrolling path")] private List<GameObject> patrollingPoints = new List<GameObject>();

    [Header("Debug status variables", order = 3)]
    [SerializeField] private int currentPatrolPoint = 1;
    [SerializeField] private GameObject currentDistractionObject;

    private NavMeshAgent agent;
    private FieldOfView fov;
    private NavMeshPath path;
    private float intervalTimer = 0.0f;
    private float distractTimer = 0.0f;
    private RaycastHit hit;

    private void Start(){
        agent = gameObject.GetComponent<NavMeshAgent>();
        fov = gameObject.GetComponent<FieldOfView>();
        path = new NavMeshPath();
    }

    private void Update(){
        StateHandler();

        if (state == State.idle || !canMove)
            agent.isStopped = true;

        if (state == State.roaming && !fov.canSeePlayer && canMove)
            Roam();

        if (state == State.attacking && canMove && fov.canSeePlayer)
            Attack();

        if (state == State.distracted && canMove && !fov.canSeePlayer)
            MoveToDistraction(currentDistractionObject);

        if (state == State.patrolling && canMove && !fov.canSeePlayer)
            Patrol();
    }

    //Changes the enemy AI's state depending on the situation
    //Is called in Update()
    private void StateHandler(){
        if (!canMove)
            state = State.idle;
        if (fov.canSeePlayer && canMove)
            state = State.attacking;
        if (!fov.canSeePlayer && !isDistracted && canMove && !patrolMode)
            state = State.roaming;
        if (!fov.canSeePlayer && isDistracted && canMove)
            state = State.distracted;
        if (!fov.canSeePlayer && !isDistracted && canMove && patrolMode)
            state = State.patrolling;
    }

    //Handles the roaming AI and it's functions (Go to a random position)
    //Called in Update()
    private void Roam(){
        agent.isStopped = false;
        SetDestinationRandomPointInNavSurface();
        agent.speed = agentBaseMoveSpeed;
    }

    //Handles the attack AI (Runs towards the player)
    //Called in Update()
    private void Attack(){
        agent.isStopped = false;
        isDistracted = false;
        agent.destination = fov.playerModelRef.transform.position;
        agent.speed = agentAttackMoveSpeed;
    }

    //Handles the distraction AI (Move towards the distraction)
    //Called in Update()
    private void MoveToDistraction(GameObject currentDistractionObjectMoveTo){
        if (distractionObjects == null){
            if (!patrolMode)
                state = State.roaming;
            if (patrolMode)
                state = State.patrolling;
            return;
        }

        agent.isStopped = false;
        agent.destination = currentDistractionObjectMoveTo.transform.position;
        agent.speed = agentBaseMoveSpeed;

        if (ReachedDestinationOrGaveUp(agent))
        {
            distractTimer += Time.deltaTime;
            if (distractTimer > distractTime)
            {
                distractTimer = 0;
                isDistracted = false;
                if (!patrolMode)
                    state = State.roaming;
                if (patrolMode)
                    state = State.patrolling;
            }
        }
    }

    //Will distract the enemy to a certain object
    //Can be called from any other script
    public void Distract(GameObject distractionObject){
        isDistracted = true;
        currentDistractionObject = distractionObject;
    }

    //Handles the patrolling AI (Move from patrol point to patrol point)
    //Called in Update()
    private void Patrol(){
        if (patrollingPoints == null){
            state = State.roaming;
            return;
        }

        agent.isStopped = false;
        agent.speed = agentBaseMoveSpeed;
        if (ReachedDestinationOrGaveUp(agent)){
            agent.destination = patrollingPoints[currentPatrolPoint - 1].transform.position;

            if (currentPatrolPoint < patrollingPoints.Count)
                currentPatrolPoint++;
            else if (currentPatrolPoint >= patrollingPoints.Count)
                currentPatrolPoint = 1;
        }
    }




    //Finds a random point in the navmesh to change the destination of the enemy's agent
    //Called by Roam()
    private void SetDestinationRandomPointInNavSurface(){
        Vector3 point;
        intervalTimer += Time.deltaTime;
        if (intervalTimer > roamRateSeconds){
            intervalTimer -= roamRateSeconds;
            if (GetRandomPoint(GetZoneVectorEnemyIsStandingOn(), roamRange, out point)){
                NavMesh.CalculatePath(GetZoneVectorEnemyIsStandingOn(), point, NavMesh.AllAreas, path);
                agent.destination = point;
            }
        }
    }

    //Returns a random position inside the current enemy"s standing zone
    //Called by FindRandomPointInZone()
    bool GetRandomPoint(Vector3 center, float range, out Vector3 result){
        for (int i = 0; i < 30; i++){
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)){
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    //Returns the position vector of the current zone that the enemy is standing on
    //Called by FindRandomPointInZone()
    private Vector3 GetZoneVectorEnemyIsStandingOn(){
        Physics.Raycast(transform.position, Vector3.down, out hit, 5 + 0.1f);
        if (hit.collider != null)
            return hit.transform.gameObject.transform.position;
        return Vector3.zero;
    }

    //Checks if enemy has reached destination
    //Called by Patrol()
    public bool ReachedDestinationOrGaveUp(NavMeshAgent navMeshAgent){
        if (!navMeshAgent.pathPending){
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance){
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f) { return true; }
            }
        }
        return false;
    }
}
