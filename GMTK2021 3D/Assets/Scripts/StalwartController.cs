using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StalwartController : MonoBehaviour
{
    public PlayerController player;
    private NavMeshAgent agent;
    private bool stunned;

    private bool touchingPlayer;

    public GameObject stunLight;
    public Attack attack;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        stunLight.SetActive(stunned);
        if (!stunned)
        {
            //path towards the player and stop when colliding
            agent.SetDestination(player.transform.position);
            agent.isStopped = touchingPlayer;

            //attack once windup is complete, but only if still near the player
            if (touchingPlayer && attack.Ready)
            {
                Attack();
            }
        }
        
    }

    private void Attack()
    {
        if (attack.active)
        {
            attack.active = false;
            player.Hit(attack.damage);
        }
        
    }

    public void Stun()
    {
        stunned = true;
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
}
