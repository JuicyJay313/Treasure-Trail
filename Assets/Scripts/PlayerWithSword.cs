using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWithSword : MonoBehaviour
{
    // Config params
    [Header("Player Movement")]
    [SerializeField] float runSpeed = 1f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float groundRadius = 0.1f;

    [Header("Player Attack")]
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] float drawSwordTime = 2.4f;
    [SerializeField] int swordDamage = 1;
    [SerializeField] float delayBewtweenAttack = 0.2f;

    [Header("Damage Received")]
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);
    [SerializeField] float hurtingPeriod = 1f;
    [SerializeField] float slowMoMultiplier = 0.2f;
    [SerializeField] float invicibilityPeriod = 1f;

    private int damageFromHazards = 1;

    // State
    private bool canInput = true;
    private bool isGrounded;
    private float velocityX;
    private bool hasCollided = false;
    private float disableMovementTimer = 0.0f;
    private int attackNumber = 1;
    

    // Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    BoxCollider2D myBoxCollider;
    CapsuleCollider2D myCapsuleCollider;
    float gravityScaleNotGrounded;
    Vector3 capsuleOffset;
    Health myHealth;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponentInChildren<Animator>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        myHealth = FindObjectOfType<Health>();
        capsuleOffset = new Vector3(myCapsuleCollider.offset.x, myCapsuleCollider.offset.y);
        gravityScaleNotGrounded = myRigidBody.gravityScale;
    }

    void Update()
    {

        if (!canInput) { return; }
        disableMovementTimer -= Time.deltaTime;
        delayBewtweenAttack += Time.deltaTime;
        Run();
        FlipSprite();
        HandleInput();
        HandleAnimations();

    }
    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleAnimations()
    {
        if (!isGrounded)
        {
            myAnimator.SetBool("isGrounded", false);
            myAnimator.SetFloat("velocityY", 1 * Mathf.Sign(myRigidBody.velocity.y));
            myRigidBody.gravityScale = gravityScaleNotGrounded;
        }

        if (isGrounded)
        {
            myAnimator.SetBool("isGrounded", true);
            myAnimator.SetFloat("velocityY", -0.1f);
            if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Slopes")) ||
                myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Slopes")))
            {
                myRigidBody.gravityScale = 0f;
            }

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            if (!hasCollided)
            {
                hasCollided = true;
                if (myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
                {
                    myHealth.LoseHealth(collision.GetComponent<Enemy>().GetDamage());
                }
                else if (myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
                {
                    myHealth.LoseHealth(damageFromHazards);
                }


                if (myHealth.GetCurrentHealth() <= 0)
                {
                    Die();
                }
                else
                {
                    GetHit();
                }

            }
        }
        if (collision.name == "Death Collider")
        {
            if (!hasCollided)
            {
                hasCollided = true;
                Die();
            }  
        }
    }

    private void Die()
    {
        canInput = false;
        Time.timeScale = slowMoMultiplier;
        myAnimator.SetTrigger("Dying");
        gameObject.layer = 14;
        // Death VFX
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    private void GetHit()
    {
        if (!IsFacingRight())
        {
            myCapsuleCollider.enabled = false;
            myRigidBody.velocity = deathKick;
            StartCoroutine(Hurt());
            myCapsuleCollider.enabled = true;
        }
        else
        {
            myCapsuleCollider.enabled = false;
            Vector2 leftDeathKick = new Vector2(-deathKick.x, deathKick.y);
            myRigidBody.velocity = leftDeathKick;
            StartCoroutine(Hurt());
            myCapsuleCollider.enabled = true;
        }
    }

    IEnumerator Hurt()
    {
        canInput = false;
        myAnimator.SetTrigger("Hurting");
        gameObject.layer = 14;
        yield return new WaitForSeconds(hurtingPeriod);
        gameObject.layer = 10;
        canInput = true;
        hasCollided = false;
    }

    public void DrawSword()
    {
        StartCoroutine(DrawSwordDelay());
    }

    IEnumerator DrawSwordDelay()
    {
        canInput = false;
        gameObject.layer = 14;
        yield return new WaitForSeconds(drawSwordTime);
        gameObject.layer = 10;
        canInput = true;
    }
    private bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void HandleMovement()
    {

        isGrounded = Physics2D.OverlapCircle(transform.position + capsuleOffset, groundRadius,
            LayerMask.GetMask("Ground", "Slopes"));
        
    }


    private void HandleInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (isGrounded && delayBewtweenAttack > 0.2f)
        {
            delayBewtweenAttack = 0.0f;

            // disable movements
            disableMovementTimer = 0.35f;
            myRigidBody.velocity = new Vector2(0, 0);
            

            myAnimator.SetTrigger("Attacking" + attackNumber);

            attackNumber = attackNumber == 2 ? 1 : 2;
            Debug.Log("Attack Number: " + attackNumber);
     
            Vector3 rayStart = transform.position;
            RaycastHit2D hit;
            if (IsFacingRight())
            {
            hit = Physics2D.Raycast(rayStart, Vector2.right, attackRange,
                LayerMask.GetMask("Enemy"));
            }
            else
            {
            hit = Physics2D.Raycast(rayStart, Vector2.left, attackRange,
                LayerMask.GetMask("Enemy"));
            }

            if (hit.collider != null)
            {
            Debug.Log(hit.collider.name + " was hit");
            hit.collider.GetComponent<Enemy>().TakeDamage(swordDamage);
            }         
        }
    }



    private void Run()
    {
        if (disableMovementTimer < 0.0f)
        {
            velocityX = Input.GetAxis("Horizontal");
            Vector2 playerVelocity = new Vector2(velocityX * runSpeed, myRigidBody.velocity.y);
            myRigidBody.velocity = playerVelocity;
        }
          
    }

    private void Jump()
    {
        if (isGrounded)
        {
            Vector2 jumpVelocity = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity = jumpVelocity;
        }
        else
        {
            return;
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            myAnimator.SetBool("Running", true);
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
        else
        {
            myAnimator.SetBool("Running", false);
        }
    }
}
