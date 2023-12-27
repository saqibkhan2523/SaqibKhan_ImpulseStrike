using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoorScript : MonoBehaviour
{
    private Animator animator;
    private SpawnManager spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            animator.SetInteger("Open Door", 1);
        }
        if(other.gameObject.CompareTag("Friend"))
        {
            spawnManager.activeFriends.Remove(other.gameObject);
            spawnManager.totalFriends--;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetInteger("Open Door", 2);
        }
    }
}
