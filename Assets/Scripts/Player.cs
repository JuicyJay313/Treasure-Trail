using Cinemachine;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // To Instantiate when the player collides with the "Sword Pedestal" game object
    [SerializeField] PlayerWithSword heroWithSword;
    // Config params
    [Header ("Player Movement")]
    [SerializeField] float runSpeed = 1f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float groundRadius = 0.1f;

    [Header("Player Sound FX")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] [Range(0, 1)] float jumpSoundVolume = 0.5f;
    //[SerializeField] AudioClip footstepSound;
    //[SerializeField] [Range(0, 1)] float footstepSoundVolume = 0.5f;
    //[SerializeField] AudioClip landingSound;
    //[SerializeField] [Range(0, 1)] float landingSoundVolume = 0.5f;
    
    [Header ("Damage Received")]
    [SerializeField] Vector2 hurtKick = new Vector2(25f, 25f);
    [SerializeField] float hurtingPeriod = 1f;
    [SerializeField] float slowMoMultiplier = 0.2f;



    private int damageFromHazards = 1;

    // State
    private bool canInput = true;
    private bool isGrounded;
    private float velocityX;
    private bool hasCollided = false;

    // Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    BoxCollider2D myBoxCollider;
    CapsuleCollider2D myCapsuleCollider;
    Health myHealth;
    CinemachineStateDrivenCamera myStateDrivenCamera;

    float gravityScaleNotGrounded;
    Vector3 capsuleOffset;
    

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        myHealth = GetComponent<Health>();
        capsuleOffset = new Vector3(myCapsuleCollider.offset.x, myCapsuleCollider.offset.y);
        gravityScaleNotGrounded = myRigidBody.gravityScale;
        myStateDrivenCamera = FindObjectOfType<CinemachineStateDrivenCamera>();
    }

    void Update()
    {
        
        if (!canInput) { return; }
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
            if(myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Slopes")) || 
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
                
                
                if(myHealth.GetCurrentHealth() <= 0)
                {
                    Die();
                }
                else
                {
                    GetHit();
                }
                
            }
        }
        if(collision.name == "Sword Pedestal")
        {
            if (!hasCollided)
            {
                hasCollided = true;
                collision.GetComponent<SwordPedestal>().ChangeSprite();
                var newHero = Instantiate(heroWithSword, new Vector3(collision.transform.position.x,
                    collision.transform.position.y -1.5f, 
                    collision.transform.position.z), collision.transform.rotation);
                newHero.GetComponent<Health>().SetHearts(myHealth.GetHeartImages());
                myStateDrivenCamera.m_AnimatedTarget = newHero.GetComponent<Animator>();
                myStateDrivenCamera.m_Follow = newHero.transform;
                myStateDrivenCamera.m_LookAt = newHero.transform;
                Destroy(gameObject);
                newHero.GetComponent<Animator>().SetTrigger("Draw Sword");
                newHero.DrawSword();
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
        // Trigger Death animation/ Death VFX
        FindObjectOfType<GameSession>().ProcessPlayerDeath(); 
    }

    private void GetHit()
    {
        if (!IsFacingRight())
        {
            myCapsuleCollider.enabled = false;
            myRigidBody.velocity = hurtKick;
            StartCoroutine(Hurt());
            myCapsuleCollider.enabled = true;
        }
        else
        {
            myCapsuleCollider.enabled = false;
            Vector2 leftHurtKick = new Vector2(-hurtKick.x, hurtKick.y);
            myRigidBody.velocity = leftHurtKick;
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
    private bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void HandleMovement()
    {
        
        isGrounded = Physics2D.OverlapCircle(transform.position + capsuleOffset, groundRadius,
            LayerMask.GetMask("Ground", "Slopes"));
        
        velocityX = Input.GetAxis("Horizontal");
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(velocityX * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;    
    }

    private void Jump()
    {
        if (isGrounded)
        {
            Vector2 jumpVelocity = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity = jumpVelocity;
            AudioSource.PlayClipAtPoint(jumpSound, Camera.main.transform.position, jumpSoundVolume);
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
