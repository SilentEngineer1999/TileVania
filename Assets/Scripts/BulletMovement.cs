using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    PlayerMovement player;
    float xspeed = 0f;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        player = FindFirstObjectByType<PlayerMovement>();
        xspeed = player.transform.localScale.x * 7f;
    }

    void Update()
    {
        myRigidBody.linearVelocity = new Vector2(xspeed, 0f);        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }
}
