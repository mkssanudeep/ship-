using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum AIState
{
    Chase,
    Patrol,
    Search
}

public class StalwartController : MonoBehaviour
{
    public PlayerController player;
    private NavMeshAgent agent;
    public ParticleSystem zap;
    [SerializeField]
    private bool stunned;
    public List<GameObject> patrolQueue;
    public int queueIndex;

    private Vector3 lastKnownPlayerPosition;
    private AIState state;

    public Material patrol;
    public Material stun;
    public Material chase;

    private bool alerted;

    private bool touchingPlayer;
    public float range;

    public MeshRenderer stunLight;
    public Attack attack;

    public FieldOfView fov;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        state = AIState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stunned)
        {
            //path towards the player and stop when colliding
            if (Detect())
            {
                
                lastKnownPlayerPosition = player.transform.position;
                state = AIState.Chase;
                agent.speed = 6f;
            }
            else
            {
                stunLight.material = patrol;
                agent.speed = 3;
            }
            if (state == AIState.Chase)
            {
                
                if (Vector3.Distance(transform.position, lastKnownPlayerPosition) < 1)
                {
                    
                    StartCoroutine(Reset());
                    state = AIState.Search;
                }

                stunLight.material = chase;
                agent.SetDestination(lastKnownPlayerPosition);
                agent.isStopped = touchingPlayer;

                //attack once windup is complete, but only if still near the player
                if (touchingPlayer && attack.Ready)
                {
                    Attack();
                }
            }
            else if (state == AIState.Search)
            {
                transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y+0.01f, transform.rotation.z, transform.rotation.w);
            }
            else if (state == AIState.Patrol)
            {
                agent.SetDestination(patrolQueue[queueIndex].transform.position);

                if (Vector3.Distance(transform.position, patrolQueue[queueIndex].transform.position) < 1)
                {

                    StartCoroutine(ChangeQueueIndex());
                    state = AIState.Search;
                }
            }
            
        }
        else
        {
            agent.SetDestination(transform.position);
        }
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("returning to patrol");
        state = AIState.Patrol;
    }

    private bool Detect()
    {
        Vector3 targetDir = (player.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(targetDir, transform.forward);
        if (angle < 60)
        {
            if (HasLineOfSight())
            {
                
                return true;
            }
        }
        return false;
    }

    private bool HasLineOfSight()
    {
        int layermask = LayerMask.GetMask("Characters", "Walls");
        Ray ray = new Ray(transform.position, player.transform.position - transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,  range, layermask))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private void Attack()
    {
        if (attack.active)
        {
            attack.active = false;
            ParticleSystem p = Instantiate(zap);
            p.gameObject.transform.position = player.transform.position;
            p.Play();
            player.Hit(attack.damage);
        }
        
    }

    public void Stun()
    {
        stunned = true;
        stunLight.material = stun;
        StartCoroutine(Unstun());
    }

    private IEnumerator Unstun()
    {
        yield return new WaitForSeconds(2);
        stunned = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        touchingPlayer = true;
        attack.StartWindUp();
    }

    private void OnCollisionExit(Collision collision)
    {
        touchingPlayer = false;
    }

    private IEnumerator ChangeQueueIndex()
    {
        yield return new WaitForSeconds(3);
        queueIndex++;
        if (queueIndex == patrolQueue.Count)
        {
            queueIndex = 0;
        }
        state = AIState.Patrol;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Ray(transform.position, player.transform.position-transform.position));
    }
}
