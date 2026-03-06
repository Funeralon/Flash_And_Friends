using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPCBehavior : MonoBehaviour
{
    [Header("Routine du PNJ")]
    public float waitTimeAtWaypoint = 2f;

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;

    private NavMeshAgent agent;
    public Animator animator;
    private bool isInteracting = false;
    public bool isPosing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject[] foundPOIs = GameObject.FindGameObjectsWithTag("POI");
        waypoints = new Transform[foundPOIs.Length];

        for (int i = 0; i < foundPOIs.Length; i++)
        {
            waypoints[i] = foundPOIs[i].transform;
        }

        MelangerDestinations();

        if (waypoints.Length > 0 && agent.isOnNavMesh)
        {
            agent.SetDestination(waypoints[0].position);
        }
        else if (!agent.isOnNavMesh)
        {
            Debug.LogWarning("Attention : Le PNJ " + gameObject.name + " n'est pas posé sur le sol bleu (NavMesh) !");
        }
    }

    void Update()
    {
        if (animator != null) animator.SetFloat("Speed", agent.velocity.magnitude);

        if (!isInteracting && agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StartCoroutine(WaitAndGoToNext());
        }
    }

    IEnumerator WaitAndGoToNext()
    {
        isInteracting = true;
        yield return new WaitForSeconds(waitTimeAtWaypoint);

        if (waypoints.Length > 0 && agent.isOnNavMesh)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
        isInteracting = false;
    }

    public void ReactToCamera(Vector3 playerPosition)
    {
        if (!isInteracting) StartCoroutine(PoseForCamera(playerPosition));
    }

    IEnumerator PoseForCamera(Vector3 playerPosition)
    {
        isInteracting = true;
        if (agent.isOnNavMesh) agent.isStopped = true;

        Vector3 direction = (playerPosition - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(direction);

        if (animator != null) animator.SetTrigger("TriggerPose");

        isPosing = true;
        yield return new WaitForSeconds(3f);
        isPosing = false;

        if (agent.isOnNavMesh) agent.isStopped = false;
        isInteracting = false;
    }

    void MelangerDestinations()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            Transform temp = waypoints[i];
            int randomIndex = Random.Range(i, waypoints.Length);
            waypoints[i] = waypoints[randomIndex];
            waypoints[randomIndex] = temp;
        }
    }
}