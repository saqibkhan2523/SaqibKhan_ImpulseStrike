using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private SpawnManager spawnManager;

    private void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Friend"))
        {
            other.gameObject.GetComponent<FriendlyScript>().DecreaseFriendlyHealth(70);
            spawnManager.GameLoseCondition();
        }
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().playerHealth -= 20;
        }
    }
}
