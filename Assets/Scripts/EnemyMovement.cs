using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Congig parameters
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float hurtAnimationTime = 1f;

    private bool canMove = true;

    // Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    
    
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    
    void Update()
    {
        if(canMove)
        {
            if (IsFacingRight())
            {
                myRigidBody.velocity = new Vector2(moveSpeed, 0f);
            }
            else
            {
                myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
            }
        }  
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    public void HurtAnimation()
    {
        myAnimator.SetTrigger("Take Hit");
        StartCoroutine(HurtDelay());

    }

    IEnumerator HurtDelay()
    {
        canMove = false;
        myRigidBody.velocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(hurtAnimationTime);
        canMove = true;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);
    }

    
}
