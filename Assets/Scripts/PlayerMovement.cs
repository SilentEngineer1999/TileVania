using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    bool isAlive = true;
    UnityEngine.Vector2 moveInput;
    float climbSpeed = 5f;
    float moveSpeed = 5f;
    float gravityScaleAtStart;
    Rigidbody2D myRigidBody;
    Animator playerAnimatorController;
    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D myBoxCollider;
    [SerializeField] UnityEngine.Vector2 deathKick = new UnityEngine.Vector2(0, 20f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    float jumpSpeed = 25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();
        playerAnimatorController = GetComponent<Animator>();
        gravityScaleAtStart = myRigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) {return;}
        Run();
        FlipSprite();
        ClimbLadder();
        Die();    
    }

    void ClimbLadder()
    {
        if (!myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            playerAnimatorController.speed = 1;
            playerAnimatorController.SetBool("isClimbing", false);
            myRigidBody.gravityScale = gravityScaleAtStart;
            return;
        }
        UnityEngine.Vector2 climbVelocity = new UnityEngine.Vector2 (myRigidBody.linearVelocity.x, moveInput.y * climbSpeed);
        myRigidBody.linearVelocity = climbVelocity; 
        myRigidBody.gravityScale = 0f;
        playerAnimatorController.SetBool("isClimbing", true);
        bool playerIsClimbing = Mathf.Abs(myRigidBody.linearVelocity.y) > Mathf.Epsilon;
        if (!playerIsClimbing)
        {
            playerAnimatorController.speed = 0;
        }
        else
        {
            playerAnimatorController.speed = 1;
        }
    }

    void OnFire()
    {
        if (!isAlive) {return;}
        Instantiate(bullet, gun.position, transform.rotation);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.linearVelocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed) 
        {
            transform.localScale = new UnityEngine.Vector2(Mathf.Sign(myRigidBody.linearVelocity.x), 1f);
        }
    }

    void Run()
    {
        UnityEngine.Vector2 playerVelocity = new UnityEngine.Vector2 (moveInput.x * moveSpeed, myRigidBody.linearVelocityY);
        myRigidBody.linearVelocity = playerVelocity; 
        
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.linearVelocity.x) > Mathf.Epsilon;
        playerAnimatorController.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) {return;}
        moveInput = value.Get<UnityEngine.Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) {return;}
        Debug.Log("triggered");
        if (!myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        if (value.isPressed)
        {
            myRigidBody.linearVelocityY += jumpSpeed;
        }
    }

    void Die()
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"))){
            isAlive = false;
            playerAnimatorController.SetTrigger("Dying");
            myRigidBody.linearVelocity = deathKick;
            FindFirstObjectByType<GameSession>().ProcessPlayerDeath();
        }
    }
}
