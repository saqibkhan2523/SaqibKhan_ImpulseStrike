using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public Transform tragetTransform;
    private NavMeshAgent agent;
    public float enemyHealth = 70f;
    private Animator animator;
    private SpawnManager spawnManager;
    public bool moveAllowed = false;
    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        agent = GetComponent<NavMeshAgent>();
        tragetTransform = GameObject.Find("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveAllowed && !spawnManager.gameOver && !spawnManager.gamePause)
        {
            
            if (tragetTransform != null)
            {
                agent.destination = tragetTransform.position;
            }
            else
            {
                SetTargetToFollow(spawnManager.ReturnTargetForEnemy());
            }
            if (enemyHealth <= 0)
            {
                Destroy(gameObject);
            }

            if(Vector3.Distance(agent.transform.position, tragetTransform.position) <= agent.stoppingDistance)
            {
                print("Distance <= ");
                EnemyMove(false);
                EnemyAttack(true);
            }
            else if(Vector3.Distance(agent.transform.position, tragetTransform.position) > agent.stoppingDistance)
            {
                print("Distance > ");
                EnemyAttack(false);
                EnemyMove(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Bullet") && moveAllowed)
        {
            print("hitting enemy");
            DecreaseEnemyHealth(20);
        }

        //if(other.gameObject.CompareTag("Player") && moveAllowed)
        //{
        //    tragetTransform.GetComponent<PlayerController>().playerHealth -= 20f;
        //}
    }

    public void DecreaseEnemyHealth(int decreaseAmount)
    {
        enemyHealth -= decreaseAmount;
    }

    public void EnemyMove(bool state)
    {
        moveAllowed = state;
        animator.SetBool("Run", state);
    }

    public void EnemyAttack(bool state)
    {
        animator.SetBool("Attack", state);
    }

    public void SetTargetToFollow(Transform target)
    {
        tragetTransform = target;
       
    }
}
