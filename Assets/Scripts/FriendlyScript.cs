using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FriendlyScript : MonoBehaviour
{
    private Transform playerTransform;
    private NavMeshAgent agent;
    public float maxHealth = 70f;
    public float friendlyHealth = 70f;
    public bool moveAllowed = false;
    private Animator animator;
    private SpawnManager spawnManager;
    public bool isCageDoorOpen = false;

    private FloatingHealthBar floatingHealthBar;
    // Start is called before the first frame update
    void Start()
    {
        floatingHealthBar = GetComponentInChildren<FloatingHealthBar>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveAllowed && !spawnManager.gameOver && !spawnManager.gamePause)
        {
            agent.destination = playerTransform.position;
            if (friendlyHealth <= 0)
            {
                Destroy(gameObject);
                spawnManager.GameLoseCondition();
                //pause menu
            }
        }

        if (isCageDoorOpen)
        {
            if (Vector3.Distance(agent.transform.position, playerTransform.position) <= agent.stoppingDistance)
            {
                FriendlyStop();
            }
            else if (Vector3.Distance(agent.transform.position, playerTransform.position) > agent.stoppingDistance)
            {
                FriendlyMove();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("Bullet") && moveAllowed)
        //{
        //    DecreaseFriendlyHealth(70);

        //}
    }

    public void DecreaseFriendlyHealth(int decreaseAmount)
    {
        friendlyHealth -= decreaseAmount;
    }

    public void FriendlyMove()
    {
        moveAllowed = true;
        animator.SetBool("Run", true);
    }

    public void FriendlyStop()
    {
        moveAllowed = false;
        animator.SetBool("Run", false);
    }
}
