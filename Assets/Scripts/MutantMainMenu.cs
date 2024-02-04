using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantMainMenu : MonoBehaviour
{
    private Animator animator;
    private float flexAnimTimer = 2f;
    private float maxFlexAnim = 15f;
    private float minFlexAnim = 10f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        //StartCoroutine(FlexAnimation());
    }

    //Instead of co routine make timer in Update.
    private void Update()
    {
        StartCoroutine(FlexAnimation());
    }

    public void SetFlexAnimBool()
    {
        animator.SetBool("Flex", false);
        flexAnimTimer = Random.Range(minFlexAnim, maxFlexAnim);
    }

    IEnumerator FlexAnimation()
    {
        print(flexAnimTimer);
        yield return new WaitForSeconds(flexAnimTimer);
        
        animator.SetBool("Flex", true);
    }

    
}
