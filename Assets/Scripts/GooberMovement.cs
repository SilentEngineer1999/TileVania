using System;
using UnityEngine;

public class GooberMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    Rigidbody2D myRigidBody;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidBody.linearVelocityX = moveSpeed;  
    }

    void OnTriggerExit2D(Collider2D other) {
        moveSpeed = -moveSpeed;
        FlipEnemySprite();
    }

    void FlipEnemySprite()
    {
        transform.localScale = new Vector2(-Mathf.Sign(myRigidBody.linearVelocity.x), 1f);
    }
}
