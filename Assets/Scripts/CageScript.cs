using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class CageScript : MonoBehaviour
{
    public Transform characterSpwanPoint;

    public GameObject character;

    private Animator animator;

    private bool doorReadyToOpen = false;

    private SpawnManager spawnManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !doorReadyToOpen)
        {
            Debug.Log("Player Collided");
            doorReadyToOpen = true;
        }
    }

    public void OpenDoor()
    {
        if(doorReadyToOpen)
        {
            animator.SetBool("OpenDoor", true);
            doorReadyToOpen = false;
        }
    }

    public void ActivateCharacter()
    {
        if(character.gameObject.CompareTag("Enemy"))
        {
            //character.GetComponent<EnemyScript>().moveAllowed = true;
            character.GetComponent<EnemyScript>().EnemyMove(true);
            character.GetComponent<EnemyScript>().SetTargetToFollow(spawnManager.ReturnTargetForEnemy());
        }
        if (character.gameObject.CompareTag("Friend"))
        {
            spawnManager.activeFriends.Add(character.gameObject);
            character.GetComponent<FriendlyScript>().moveAllowed = true;
        }

        character.transform.SetParent(null);
    }
}
