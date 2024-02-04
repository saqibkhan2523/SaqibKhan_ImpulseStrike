using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public Transform tragetTransform;
    private NavMeshAgent agent;
    public float maxHealth = 70f;
    public float enemyHealth = 70f;
    private Animator animator;
    private SpawnManager spawnManager;
    public bool moveAllowed = false;
    public bool isCageDoorOpen = false;

    private FloatingHealthBar floatingHealthBar;
    // Start is called before the first frame update
    void Start()
    {
        floatingHealthBar = GetComponentInChildren<FloatingHealthBar>();
        enemyHealth = maxHealth;
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        agent = GetComponent<NavMeshAgent>();
        tragetTransform = GameObject.Find("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawnManager.gameOver && !spawnManager.gamePause)
        {
            
            
            if (enemyHealth <= 0)
            {
                animator.SetBool("Death", true);
                //Destroy(gameObject);
            }

            if (moveAllowed && enemyHealth > 0)
            {
                if (tragetTransform != null)
                {
                    agent.destination = tragetTransform.position;
                }
                else
                {
                    SetTargetToFollow(spawnManager.ReturnTargetForEnemy());
                }
            }

            if (isCageDoorOpen)
            {
                if (Vector3.Distance(agent.transform.position, tragetTransform.position) <= agent.stoppingDistance)
                {
                    //print("Distance <= ");
                    EnemyMove(false);
                    EnemyAttack(true);
                }
                else if (Vector3.Distance(agent.transform.position, tragetTransform.position) > agent.stoppingDistance)
                {
                    //print("Distance > ");
                    EnemyAttack(false);
                    EnemyMove(true);
                }
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Enemy was hit");
        if(other.gameObject.CompareTag("Bullet") && moveAllowed)
        {
            //print("hitting enemy");
            DecreaseEnemyHealth(20);

        }

        //if(other.gameObject.CompareTag("Player") && moveAllowed)
        //{
        //    tragetTransform.GetComponent<PlayerController>().playerHealth -= 20f;
        //}
    }

    public void DecreaseEnemyHealth(int decreaseAmount)
    {
        //Debug.Log("inside the health function");
        floatingHealthBar.UpdateHealth(enemyHealth, maxHealth);
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

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public void DisableEnemyColliderAndNavMesh()
    {
        this.GetComponent<NavMeshAgent>().enabled = false;
        this.GetComponent<CapsuleCollider>().enabled = false;
    }
}
